using DtoGenerator.CodeGenerators.GeneratedItems;
using DtoGenerator.DtoDescriptor;
using System.Threading;

namespace DtoGenerator.CodeGenerators
{
    internal class TaskInfo
    {
        public ClassDescription ClassDescription { get; }
        public string ClassesNamespace { get; }
        public ManualResetEvent ResetEvent { get; }
        public GeneratedClass result { get; set; }

        public TaskInfo(ClassDescription classDescription, string classesNamespace, ManualResetEvent resetEvent)
        {
            ClassDescription = classDescription;
            ClassesNamespace = classesNamespace;
            ResetEvent = resetEvent;
            result = null;
        }
    }
}
