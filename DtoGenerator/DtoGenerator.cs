using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public static DtoGenerator Instance = new DtoGenerator();

        public IDtoInfoListReader DtoReader { get; set; } = null;
        public IDtoDeclarationWriter DtoWriter { get; set; } = null;
        public int MaxTaskCount { get; set; } = -1;
        public string NamespaceName { get; set; } = null;

        private readonly object _lock = new object();
        private readonly Workspace _workspace = new AdhocWorkspace();
       
        
        private DtoGenerator() { }

        public void Reset()
        {
            
        }

 /*       public DtoDeclaration[] GenerateDtoDeclarations(DtoInfo[] dtoInfos)
        {
            lock (_lock)
            {
                using (CountdownEvent countdownEvent = new CountdownEvent(dtoInfos.Length))
                using (Semaphore semaphore = new Semaphore(MaxTaskCount, MaxTaskCount))
                {
                    _dtoDeclarations = new DtoDeclaration[dtoInfos.Length];
                    for (int i = 0; i < dtoInfos.Length; i++)
                    {
                        semaphore.WaitOne();
                        int index = i;
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            _dtoDeclarations[index] = (GenerateDtoDeclaration(dtoInfos[index]));
                            semaphore.Release();
                            countdownEvent.Signal();
                        });
                    }
                    countdownEvent.Wait();
                    return _dtoDeclarations;
                }
            }
        }
*/
        
        private DtoDeclaration GenerateDtoDeclaration(DtoInfo dtoInfo)
        {
            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(IdentifierName(NamespaceName));
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(dtoInfo.Name);
            classDeclaration =
                classDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.SealedKeyword)));
            classDeclaration =
                classDeclaration.WithMembers(List(dtoInfo.Fields.Select(GenerateDtoPropertyDeclaration)));

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
            return new DtoDeclaration(dtoInfo.Name,
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