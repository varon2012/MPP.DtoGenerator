using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp;

namespace DTOGenerator
{
    public class DTOGenerator
    {
    
        private readonly string dtoNamespace;
        private readonly object SyncList = new object();
        private SimpleLockThreadPool threadPool;

        public DTOGenerator()
        {
            int threadPoolLimit;
            if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("threadPoolLimit"), out threadPoolLimit))
            {
                throw new InvalidOperationException("threadPoolLimit is not found in app.config");    
            }
            threadPool = new SimpleLockThreadPool(threadPoolLimit);


            dtoNamespace = ConfigurationManager.AppSettings.Get("dtoNamespace");
            if ((dtoNamespace == null) || (dtoNamespace == string.Empty))
            {
                throw new InvalidOperationException("dtoNamespace is not found in app.config");
            }           
        }

        public List<DTODescription> GenerateCode(List<ClassDescription> classesList)
        {
            List<DTODescription> classUnits = new List<DTODescription>();

            foreach (ClassDescription classDescription in classesList)
            {
                DTODescription dtoDescription = new DTODescription(classDescription.ClassName);
                classUnits.Add(dtoDescription);
                ThreadPool.QueueUserWorkItem(new WaitCallback(GenerateClassCode), new object[] {classDescription, dtoDescription});
            }
            threadPool.Dispose();
            return classUnits;
        } 

        private void GenerateClassCode(Object state)
        {
            object[] array = state as object[];
            ClassDescription classDescription = (ClassDescription) array[0];
            DTODescription dtoDescription = (DTODescription)array[1];

            SyntaxList<MemberDeclarationSyntax> memberDeclarationSyntaxesList = new SyntaxList<MemberDeclarationSyntax>();

            foreach (PropertyDescription propertyDescription in classDescription.PropertyDescriptions)
            {
                memberDeclarationSyntaxesList.Add(GeneratePropertyCode(propertyDescription));
            }

            CompilationUnitSyntax compilationUnit = CompilationUnit()
            .WithUsings(
                SingletonList<UsingDirectiveSyntax>(
                    UsingDirective(
                        IdentifierName("System"))))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    NamespaceDeclaration(
                        IdentifierName(dtoNamespace))
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            ClassDeclaration(classDescription.ClassName)
                            .WithMembers(memberDeclarationSyntaxesList)))))
            .NormalizeWhitespace();

            dtoDescription.SyntaxTree = compilationUnit.SyntaxTree;
        }

        private MemberDeclarationSyntax GeneratePropertyCode(PropertyDescription propertyDescription)
        {
            MemberDeclarationSyntax propertyCode = PropertyDeclaration(
                                                        IdentifierName(propertyDescription.Type.FullName),
                                                        Identifier(propertyDescription.Name))
                                                    .WithModifiers(
                                                        TokenList(
                                                            Token(SyntaxKind.PublicKeyword)))
                                                    .WithAccessorList(
                                                        AccessorList(
                                                            List<AccessorDeclarationSyntax>(
                                                                new AccessorDeclarationSyntax[]{
                                                                    AccessorDeclaration(
                                                                        SyntaxKind.GetAccessorDeclaration)
                                                                    .WithSemicolonToken(
                                                                        Token(SyntaxKind.SemicolonToken)),
                                                                    AccessorDeclaration(
                                                                        SyntaxKind.SetAccessorDeclaration)
                                                                    .WithModifiers(
                                                                        TokenList(
                                                                            Token(SyntaxKind.PrivateKeyword)))
                                                                    .WithSemicolonToken(
                                                                        Token(SyntaxKind.SemicolonToken))})));
            return propertyCode;
        }
    }
}
