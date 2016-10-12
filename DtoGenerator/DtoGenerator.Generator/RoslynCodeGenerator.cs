using System;
using System.Collections.Generic;
using DtoGenerator.Generator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TextFormatters;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DtoGenerator.Generator
{
    public sealed class RoslynCodeGenerator : ICodeGenerator
    {
        private const string ClassIndent = "    ";
        private const string PropertyIndent = "        ";

        private readonly ILogger _logger;

        public RoslynCodeGenerator()
        {
        }

        public RoslynCodeGenerator(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public string Generate(string specifiedNamespace, DtoClassDescription classDescription, 
            TypeResolver typeResolver)
        {
            if (specifiedNamespace == null) throw new ArgumentNullException(nameof(specifiedNamespace));
            if (typeResolver == null) throw new ArgumentNullException(nameof(typeResolver));

            return CompilationUnit()
                    .WithMembers(SingletonList(GenerateNamespace(specifiedNamespace, SingletonList(
                        GenerateClass(classDescription, GenerateProperties(classDescription.Properties, typeResolver)))
                    ))).ToString();
        }

        private MemberDeclarationSyntax GenerateNamespace(string specifiedNamespace, 
            SyntaxList<MemberDeclarationSyntax> members)
        {
            return NamespaceDeclaration(IdentifierName(Identifier
                (
                    TriviaList(),
                    specifiedNamespace,
                    TriviaList(LineFeed)
                )))
                .WithNamespaceKeyword(Token(TriviaList(), SyntaxKind.NamespaceKeyword, TriviaList(Space)))
                .WithOpenBraceToken(Token(TriviaList(), SyntaxKind.OpenBraceToken, TriviaList(LineFeed)))
                .WithMembers(members)
                .WithCloseBraceToken(Token(TriviaList(), SyntaxKind.CloseBraceToken, TriviaList(LineFeed)));
        }

        private MemberDeclarationSyntax GenerateClass(DtoClassDescription classDescription,
            SyntaxList<MemberDeclarationSyntax> members)
        {
            return ClassDeclaration(Identifier(TriviaList(), classDescription.ClassName, TriviaList(Space, LineFeed)))
                .WithModifiers(TokenList
                (
                    Token(TriviaList(Whitespace(ClassIndent)), SyntaxKind.PublicKeyword, TriviaList(Space)),
                    Token(TriviaList(), SyntaxKind.SealedKeyword, TriviaList(Space)))
                )
                .WithKeyword(Token(TriviaList(), SyntaxKind.ClassKeyword, TriviaList(Space)))
                .WithOpenBraceToken(Token(TriviaList(Whitespace(ClassIndent)), SyntaxKind.OpenBraceToken,
                    TriviaList(LineFeed)))
                .WithMembers(members)
                .WithCloseBraceToken(Token
                (
                    TriviaList(Whitespace(ClassIndent)),
                    SyntaxKind.CloseBraceToken,
                    TriviaList(LineFeed))
                );
        }

        private SyntaxList<MemberDeclarationSyntax> GenerateProperties(IEnumerable<DtoClassProperty> properties,
            TypeResolver typeResolver)
        {
            var propertyList = new List<MemberDeclarationSyntax>();

            foreach (var property in properties)
            {
                try
                {
                    propertyList.Add(GenerateProperty(property, GenerateAccessorList(), typeResolver));
                }
                // TODO: separate exceptions handling
                catch (Exception e)
                {
                    if (_logger == null) continue;

                    _logger.Log(e.Message);
                    if (e.InnerException != null)
                    {
                        _logger.Log(e.InnerException.Message);
                    }
                }
            }

            return List(propertyList);
        }

        private MemberDeclarationSyntax GenerateProperty(DtoClassProperty property, AccessorListSyntax accessorList,
            TypeResolver typeResolver)
        {
            var type = typeResolver.ResolveType(property.Type, property.Format);

            return PropertyDeclaration
                (
                    QualifiedName
                    (
                        IdentifierName(type.Namespace),
                        IdentifierName(Identifier(TriviaList(), type.Name, TriviaList(Space)))
                    ),
                    Identifier(TriviaList(), property.Name, TriviaList(Space))
                )
                .WithModifiers(TokenList(Token
                (
                    TriviaList(Whitespace(PropertyIndent)),
                    SyntaxKind.PublicKeyword,
                    TriviaList(Space)
                )))
                .WithAccessorList(accessorList);
        }

        private AccessorListSyntax GenerateAccessorList()
        {
            return AccessorList(
                    List(
                        new[]
                        {
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(Token(TriviaList(), SyntaxKind.SemicolonToken, TriviaList(Space))),
                            AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                .WithSemicolonToken(Token(TriviaList(), SyntaxKind.SemicolonToken, TriviaList(Space)))
                        }))
                .WithOpenBraceToken(Token(TriviaList(), SyntaxKind.OpenBraceToken, TriviaList(Space)))
                .WithCloseBraceToken(Token(TriviaList(), SyntaxKind.CloseBraceToken, TriviaList(LineFeed)));
        }
    }
}
