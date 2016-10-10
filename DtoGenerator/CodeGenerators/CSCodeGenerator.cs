using DtoGenerator.DtoDescriptor;
using DtoGenerator.CodeGenerators.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using DtoGenerator.CodeGenerators.GeneratedItems;
using System;

namespace DtoGenerator.CodeGenerators
{
    class CSCodeGenerator : ICodeGenerator
    {
        private SupportedTypesTable supportedTypes = new SupportedTypesTable();
        private string classNamespace;
        private ClassDescription classDescription;

        public void GenerateCode(object threadContext)
        {
            //I really don't know what happens here, but Roslyn doesn't work without it
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);

            if(threadContext == null) throw new ArgumentNullException(nameof(threadContext));

            TaskInfo parameters = threadContext as TaskInfo;
            classNamespace = parameters.ClassesNamespace;
            classDescription = parameters.ClassDescription;

            NamespaceDeclarationSyntax namespaceDeclaration = GenerateNamespace();
            GeneratedClass generatedClass = new GeneratedClass(classDescription.ClassName, Formatter.Format(namespaceDeclaration, new AdhocWorkspace()).ToFullString());

            parameters.result = generatedClass;
            parameters.ResetEvent.Set();
        }


        private NamespaceDeclarationSyntax GenerateNamespace()
        {
            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(IdentifierName(classNamespace));
            ClassDeclarationSyntax classDeclaration = GenerateClass(classDescription.ClassName, classDescription.Properties);
            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);

            return namespaceDeclaration;
        }

        private ClassDeclarationSyntax GenerateClass(string className, Property[] properties)
        {
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(className);
            classDeclaration = classDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));

            MemberDeclarationSyntax[] propertiesDeclarations = new MemberDeclarationSyntax[properties.Length];
            for(int i = 0; i < properties.Length; i++)
            {
                Property property = properties[i];
                string propertyType = supportedTypes.GetNetType(property.Type, property.Format);
                propertiesDeclarations[i] = GenerateProperty(property.Name, propertyType);
                
            }

            classDeclaration = classDeclaration.AddMembers(propertiesDeclarations);
            return classDeclaration;
        }

        private PropertyDeclarationSyntax GenerateProperty(string propertyName, string propertyType)
        {
            PropertyDeclarationSyntax propertyDeclaration = PropertyDeclaration(IdentifierName(propertyType), propertyName);
            propertyDeclaration = propertyDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));
            propertyDeclaration = propertyDeclaration.WithAccessorList(
                AccessorList(
                    List(
                        new[] {
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                            AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                        }
                        )
                    )
                );

            return propertyDeclaration;
        }
    }
}
