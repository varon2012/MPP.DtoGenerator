using System;
using System.Collections.Generic;
using DtoGenerator.DeserializedData;
using DtoGenerator.Plugins;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DtoGenerator.CodeGenerators
{
    public sealed class RoslynCodeGenerator : ICodeGenerator
    {
        public void GenerateCode(GenerationClassUnit generationClassUnit)
        {
            GenerateDtoClassDeclaration(generationClassUnit);
        }

        private void GenerateDtoClassDeclaration(GenerationClassUnit generationClassUnit)
        {
            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(IdentifierName(generationClassUnit.NamespaceName));
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(generationClassUnit.ClassDescription.ClassName);


            classDeclaration = classDeclaration.WithModifiers(
                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword))
                );
            classDeclaration = classDeclaration.WithMembers(
                List(
                    GenerateDtoPropertiesDeclaration(generationClassUnit)
                    )
                );

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
            SyntaxNode rootNode = (generationClassUnit.NamespaceName == string.Empty)
                ? (SyntaxNode)classDeclaration
                : (SyntaxNode)namespaceDeclaration;

            generationClassUnit.Code = Formatter.Format(rootNode, new AdhocWorkspace()).ToFullString();
        }

        private List<MemberDeclarationSyntax> GenerateDtoPropertiesDeclaration(GenerationClassUnit generationClassUnit)
        {
            List<MemberDeclarationSyntax> propertiesList = new List<MemberDeclarationSyntax>();
            foreach (PropertyDescription property in generationClassUnit.ClassDescription.Properties)
            {
                Type propertyType = GetCSharpType(generationClassUnit.TypeTable, property.Type, property.Format);
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
