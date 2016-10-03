using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DtoGenerator
{
    public class DtoGenerator
    {
        public int MaxTaskCount { get; set; }
        public string NamespaceName { get; set; }

        private readonly object _lock = new object();
        private readonly Workspace _workspace = new AdhocWorkspace();

        public DtoGenerator(int maxTaskCount, string namespaceName)
        {
            NamespaceName = namespaceName;
            MaxTaskCount = maxTaskCount;
        }

        private DtoDeclaration GenerateDtoClassDeclaration(DtoInfo dtoClassInfo)
        {
            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(IdentifierName(NamespaceName));
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(dtoClassInfo.Name);
            classDeclaration =
                classDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.SealedKeyword)));
            classDeclaration =
                classDeclaration.WithMembers(List(dtoClassInfo.Fields.Select(GenerateDtoPropertyDeclaration)));

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
            return new DtoDeclaration(dtoClassInfo.Name,
                Formatter.Format(namespaceDeclaration, _workspace).ToFullString());
        }

        private MemberDeclarationSyntax GenerateDtoPropertyDeclaration(DtoFieldInfo dtoFieldInfo)
        {
            PropertyDeclarationSyntax propertyDeclaration =
                PropertyDeclaration(IdentifierName(dtoFieldInfo.DtoType.DotNetType.FullName), dtoFieldInfo.Name);
            propertyDeclaration = propertyDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));
            propertyDeclaration = propertyDeclaration.WithAccessorList(
                AccessorList(
                    List(
                        new[]
                        {
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                            AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                        }
                    )
                )
            );
            return propertyDeclaration;
        }
    }
}