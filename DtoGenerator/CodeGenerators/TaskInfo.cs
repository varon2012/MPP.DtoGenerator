using DtoGenerator.CodeGenerators.GeneratedItems;
using DtoGenerator.DtoDescriptor;
using System;
using System.Threading;

namespace DtoGenerator.CodeGenerators
{
    internal class TaskInfo : IDisposable
    {
        public ClassDescription ClassDescription { get; }
        public string ClassesNamespace { get; }
        public ManualResetEvent ResetEvent { get; }
        public GeneratedClass Result { get; set; }

        public TaskInfo(ClassDescription classDescription, string classesNamespace, ManualResetEvent resetEvent)
        {
            ClassDescription = classDescription;
            ClassesNamespace = classesNamespace;
            ResetEvent = resetEvent;
            Result = null;
        }

        public void Dispose()
        {
            ResetEvent.Dispose();
        }
    }
}
