using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DTOGeneratorLibrary
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DTOGenerator
    {
        public string GenerateDTOClasses(DTOClassInfo[] dtoClassesInfo)
        {
            CompilationUnitSyntax dtoClasses = CompilationUnit();            
            dtoClasses = dtoClasses.WithMembers(List(dtoClassesInfo.Select(GenerateDTOClassDeclaration)));

            SyntaxNode result = Formatter.Format(dtoClasses, new AdhocWorkspace());

            return result.ToFullString();
        }

        // Internals

        private MemberDeclarationSyntax GenerateDTOClassDeclaration(DTOClassInfo dtoClassInfo)
        {
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(dtoClassInfo.Name);
            classDeclaration = classDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword)));
            classDeclaration = classDeclaration.WithMembers(List(dtoClassInfo.Properties.Select(GenerateDTOPropertyDeclaration)));
            return classDeclaration;
        }

        private MemberDeclarationSyntax GenerateDTOPropertyDeclaration(DTOPropertyInfo dtoPropertyInfo)
        {
            PropertyDeclarationSyntax propertyDeclaration = PropertyDeclaration(IdentifierName(dtoPropertyInfo.PropertyType.FullName), dtoPropertyInfo.Name);
            propertyDeclaration = propertyDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));
            propertyDeclaration = propertyDeclaration.WithAccessorList(
                AccessorList(
                    List(
                        new[] {
                            AccessorDeclaration(
                                SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                            AccessorDeclaration(
                                SyntaxKind.SetAccessorDeclaration)
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))})));
            return propertyDeclaration;
        }
    }
}
