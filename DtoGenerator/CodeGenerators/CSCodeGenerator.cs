using DtoGenerator.DtoDescriptor;
using DtoGenerator.CodeGenerators.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using DtoGenerator.CodeGenerators.GeneratedItems;

namespace DtoGenerator.CodeGenerators
{
    class CSCodeGenerator : ICodeGenerator
    {
        private SupportedTypesTable supportedTypes = new SupportedTypesTable();
        private Workspace _workspace = new AdhocWorkspace();


        public GeneratedClass GenerateCode(ClassDescription classDescription, string classNamespace)
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);

            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(IdentifierName(classNamespace));
            ClassDeclarationSyntax classDeclaration = GenerateClass(classDescription.ClassName, classDescription.Properties);
            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);

            GeneratedClass generatedClass = new GeneratedClass(classDescription.ClassName, Formatter.Format(namespaceDeclaration, new AdhocWorkspace()).ToFullString());

            return generatedClass;
        }

        private ClassDeclarationSyntax GenerateClass(string name, Property[] properties)
        {
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(name);
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

        private PropertyDeclarationSyntax GenerateProperty(string name, string type)
        {
            PropertyDeclarationSyntax propertyDeclaration = PropertyDeclaration(IdentifierName(type), name);
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
