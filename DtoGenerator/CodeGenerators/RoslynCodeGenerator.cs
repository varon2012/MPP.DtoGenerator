using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator.DeserializedData;
using DtoGenerator.Plugins;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DtoGenerator.CodeGenerators
{
    public class RoslynCodeGenerator : ICodeGenerator
    {
        public void GenerateCode(object obj)
        {
            GeneratingClassUnit generatingClassUnit = (GeneratingClassUnit)obj;
            GenerateDtoClassDeclaration(generatingClassUnit);
        }

        private void GenerateDtoClassDeclaration(GeneratingClassUnit generatingClassUnit)
        {
            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(IdentifierName(generatingClassUnit.NamespaceName));
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(generatingClassUnit.ClassDescription.ClassName);


            classDeclaration = classDeclaration.WithModifiers(
                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword))
                );
            classDeclaration = classDeclaration.WithMembers(
                List(
                    GenerateDtoPropertiesDeclaration(generatingClassUnit)
                    )
                );

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
            SyntaxNode rootNode = (generatingClassUnit.NamespaceName == string.Empty)
                ? (SyntaxNode)classDeclaration
                : (SyntaxNode)namespaceDeclaration;

            generatingClassUnit.Code = Formatter.Format(rootNode, new AdhocWorkspace()).ToFullString();
        }

        private List<MemberDeclarationSyntax> GenerateDtoPropertiesDeclaration(GeneratingClassUnit generatingClassUnit)
        {
            List<MemberDeclarationSyntax> propertiesList = new List<MemberDeclarationSyntax>();
            foreach (PropertyDescription property in generatingClassUnit.ClassDescription.Properties)
            {
                Type propertyType = GetCSharpType(generatingClassUnit.TypeTable, property.Type, property.Format);
                PropertyDeclarationSyntax propertyDeclaration = PropertyDeclaration(IdentifierName(propertyType.FullName), property.Name);
                propertyDeclaration = propertyDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));
                propertyDeclaration = propertyDeclaration.WithAccessorList(
                    AccessorList(
                        List(
                            new[]
                            {
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                            AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                            }
                        )
                    )
                );
                propertiesList.Add(propertyDeclaration);
            }
            
            
            return propertiesList;
        }

        private Type GetCSharpType(TypeTable typeTable, string type, string format)
        {
            return typeTable.GetCSharpTypeByFormatAndType(type, format);
        }
    }
}
