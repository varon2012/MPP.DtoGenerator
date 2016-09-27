using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DtoGenerationLibrary
{
    public class DtoGenerator
    {
        private int _runningTasksCount;        
        private readonly Queue<TaskInfo> _tasksQueue = new Queue<TaskInfo>();
        private readonly object _syncRoot = new object();
        private CountdownEvent _countdownEvent;
        private DtoClassDeclaration[] _tasksResult;
        private readonly Workspace _workspace = new AdhocWorkspace();

        private struct TaskInfo
        {
            public readonly int TaskNumber;
            public readonly DtoClassInfo DtoClassInfo;

            public TaskInfo(int taskNumber, DtoClassInfo dtoClassInfo)
            {
                TaskNumber = taskNumber;
                DtoClassInfo = dtoClassInfo;
            }
        }

        // Public

        public int MaxRunningTasksCount { get; set; }
        public string NamespaceName { get; set; }

        public DtoGenerator(int maxRunningTasksCount, string namespaceName)
        {
            NamespaceName = namespaceName;
            MaxRunningTasksCount = maxRunningTasksCount;
        }

        public DtoClassDeclaration[] GenerateDtoClasses(DtoClassInfo[] dtoClassesInfo)
        {
            _countdownEvent = new CountdownEvent(dtoClassesInfo.Length);            
            _tasksResult = new DtoClassDeclaration[dtoClassesInfo.Length];

            for (int i = 0; i < dtoClassesInfo.Length; i++)            
            {
                QueueGenerationTask(new TaskInfo(i, dtoClassesInfo[i]));
            }

            WaitAllTasks();

            return _tasksResult;
        }

        // Internals

        private bool HasTasks => _tasksQueue.Count > 0;
        private bool CanAddToPool => _runningTasksCount < MaxRunningTasksCount;

        private void QueueGenerationTask(TaskInfo taskInfo)
        {
            lock (_syncRoot)
            {
                if (CanAddToPool)
                {
                    AddToPool(taskInfo);
                }
                else
                {
                    _tasksQueue.Enqueue(taskInfo);
                }
            }
        }

        private void WaitAllTasks()
        {
            _countdownEvent.Wait();
        }

        private void AddToPool(TaskInfo taskInfo)
        {
            ThreadPool.QueueUserWorkItem(delegate 
            {
                _tasksResult[taskInfo.TaskNumber] = GenerateDtoClassDeclaration(taskInfo.DtoClassInfo);
                OnTaskFinish();
            });
            _runningTasksCount++;
        }

        private void OnTaskFinish()
        {
            lock (_syncRoot)
            {
                _runningTasksCount--;

                if (HasTasks)
                {
                    AddToPool(_tasksQueue.Dequeue());
                }
            }
            _countdownEvent.Signal();
        }

        private DtoClassDeclaration GenerateDtoClassDeclaration(DtoClassInfo dtoClassInfo)
        {
            NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(IdentifierName(NamespaceName));
            ClassDeclarationSyntax classDeclaration = ClassDeclaration(dtoClassInfo.Name);
            classDeclaration = classDeclaration.WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword)));
            classDeclaration = classDeclaration.WithMembers(List(dtoClassInfo.Properties.Select(GenerateDtoPropertyDeclaration)));

            namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
            return new DtoClassDeclaration(dtoClassInfo.Name, Formatter.Format(namespaceDeclaration, _workspace).ToFullString());
        }

        private MemberDeclarationSyntax GenerateDtoPropertyDeclaration(DtoPropertyInfo dtoPropertyInfo)
        {
            PropertyDeclarationSyntax propertyDeclaration = PropertyDeclaration(IdentifierName(dtoPropertyInfo.PropertyType.FullName), dtoPropertyInfo.Name);
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
            return propertyDeclaration;
        }
    }
}
