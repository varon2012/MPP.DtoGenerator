using System;
using System.Threading;
using DtoGenerator.Contracts.InputModels;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DtoGenerator.InputModels;
using Newtonsoft.Json;
using DtoGenerator.Contracts.Services.DtoGenerators;
using DtoGenerator.Contracts.Services.ThreadPools;
using DtoGenerator.Services.TablesWithTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DtoGenerator.Services.DtoGenerators
{
    public class DtoGenerator : IDtoGenerator
    {
        #region Private Members

        private readonly IThreadPool _threadPool = new ThreadPools.ThreadPool();
        private ConcurrentDictionary<string,string> _classes = new ConcurrentDictionary<string,string>();
        private readonly Workspace _workspace = new AdhocWorkspace();
        private readonly int _maximumTaskNumber;
        private readonly string _namespaceName;

        #endregion 

        #region Ctor

        public DtoGenerator(int maximumTaskNumber, string namespaceName)
        {
            if (maximumTaskNumber <= 0)
                throw new ArgumentOutOfRangeException("Task number Level must be greater than zero.");
            _maximumTaskNumber = maximumTaskNumber;
            _namespaceName = namespaceName;
        }

        #endregion

        #region Public Methods

        public IDictionary<string,string> Generate(string json)
        {
            ClassDescriptions classDescriptions = JsonConvert.DeserializeObject<ClassDescriptions>(json);

            using (var semaphore = new SemaphoreSlim(_maximumTaskNumber,_maximumTaskNumber))
            using (var coutdownEvent = new CountdownEvent(classDescriptions.Classes.Count()))
            {
                foreach (var classDescription in classDescriptions.Classes)
                {
                    semaphore.Wait();
                    _threadPool.QueueUserWorkItem((object state) => { CreateDto(state); semaphore.Release(); coutdownEvent.Signal(); },
                        classDescription);
                }
                coutdownEvent.Wait();
            }
            return _classes;
        }

        public void Dispose()
        {
            _threadPool.Dispose();
        }

        public void LoadAdditionalTypes(string folderName)
        {
            TableWithTypes.LoadAdditionalTypes(folderName);
        }

        #endregion

        #region Private Methods

        private void CreateDto(object state)
        {
            var classDesciption = state as IClassDescription;

            var classDeclaration = ClassDeclaration(classDesciption.ClassName)
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword)));

            classDeclaration = classDeclaration.WithMembers(List(classDesciption.Properties.Select(CreatePropertysInClass)));

            var namespaceDeclaration = NamespaceDeclaration(IdentifierName(_namespaceName))
                .AddMembers(classDeclaration);

            var unitDeclaration = CompilationUnit()
                .AddUsings(UsingDirective(IdentifierName("System")))
                .AddUsings(UsingDirective(IdentifierName("System.Generic")))
                .AddMembers(namespaceDeclaration);

            _classes.AddOrUpdate(classDesciption.ClassName,
                    Formatter.Format(unitDeclaration, _workspace).ToFullString(),
                    (string key, string oldValue) => oldValue);               
        }

        private MemberDeclarationSyntax CreatePropertysInClass(IPropertyDescription property)
        {
            return PropertyDeclaration(IdentifierName(TableWithTypes.TranslateToDotNetType(property.Format)), 
                property.Name)
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(AccessorList(List
                    (new[]
                        {
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                            AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                        }
                    )));
        }

        #endregion
    }
}
