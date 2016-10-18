using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

// ReSharper disable All

namespace DtoGenerator
{
    public class DtoGenerator
    {
        private static readonly Logger Logger = Logger.GetLogger(typeof(DtoGenerator).Name);

        private readonly Workspace _workspace = new AdhocWorkspace();
        private IDtoInfoListReader _dtoInfoListReader;
        private BlockingCollection<DtoInfo> _readerQueue;
        private BlockingCollection<DtoDeclaration> _writerQueue;
        private Semaphore _semaphore;

        public DtoGenerator(int maxTaskCount, string namespaceName,
            IDtoInfoListReader dtoInfoListReader, IDtoDeclarationWriter dtoDeclarationWriter)
        {
            MaxTaskCount = maxTaskCount;
            NamespaceName = namespaceName;
            DtoInfoListReader = dtoInfoListReader;
            DtoDeclarationWriter = dtoDeclarationWriter;
        }

        public int MaxTaskCount { get; set; }
        public string NamespaceName { get; set; }
        public IDtoDeclarationWriter DtoDeclarationWriter { get; set; }

        public IDtoInfoListReader DtoInfoListReader
        {
            get { return _dtoInfoListReader; }
            set
            {
                _dtoInfoListReader = value;
                _dtoInfoListReader.OnDtoInfoRead -= OnDtoInfoRead;
                _dtoInfoListReader.OnDtoInfoRead += OnDtoInfoRead;
                _dtoInfoListReader.OnReadCompleted -= OnReadCompleted;
                _dtoInfoListReader.OnReadCompleted += OnReadCompleted;
            }
        }

        private void OnReadCompleted()
        {
            _readerQueue.CompleteAdding();
            Logger.Log("Reading completed");
        }

        private int _counter = 0;

        private int _activeTaskCount = 0;

        private void OnDtoInfoRead(DtoInfo dtoInfo)
        {
            Interlocked.Increment(ref _activeTaskCount);
            _readerQueue.Add(dtoInfo);
            _semaphore.WaitOne();
            ThreadPool.QueueUserWorkItem(delegate
            {
                int index = _counter++;
                Logger.Log($"Thread_{index} starts");

                DtoDeclaration dtoDeclaration = GenerateDtoDeclaration(_readerQueue.Take());
                Logger.Log($"Declaration generated for {dtoDeclaration.ClassName}");

                _writerQueue.Add(dtoDeclaration);
                Interlocked.Decrement(ref _activeTaskCount);
                if ((_activeTaskCount == 0) && _readerQueue.IsCompleted)
                {
                    _writerQueue.CompleteAdding();
                }
                Logger.Log($"Thread_{index} ends");
                _semaphore.Release();
            });
        }

        public void GenerateDtoDeclarations()
        {
            if (MaxTaskCount <= 0 || string.IsNullOrEmpty(NamespaceName)
                || DtoDeclarationWriter == null || DtoInfoListReader == null)
            {
                throw new ArgumentException("Illegal argument values");
            }

            using (_readerQueue = new BlockingCollection<DtoInfo>(MaxTaskCount))
            using (_writerQueue = new BlockingCollection<DtoDeclaration>())
            using (_semaphore = new Semaphore(MaxTaskCount, MaxTaskCount))
            using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
            using (DtoDeclarationWriter)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    while ((!_writerQueue.IsCompleted))
                    {
                        DtoDeclaration declaration = _writerQueue.Take();
                        DtoDeclarationWriter.Write(declaration);
                        Logger.Log($"{declaration.ClassName}.cs has been written");
                    }

                    Logger.Log("Writing completed");
                    manualResetEvent.Set();
                });
                DtoInfoListReader.ReadList();
                manualResetEvent.WaitOne();
            }
        }


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