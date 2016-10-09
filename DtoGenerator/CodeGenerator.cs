using DtoGenerator.CodeGenerators;
using DtoGenerator.CodeGenerators.GeneratedItems;
using DtoGenerator.DtoDescriptor;
using DtoGenerator.DtoDescriptors;
using DtoGenerator.Parser;
using System;
using System.Threading;

namespace DtoGenerator
{
    public class CodeGenerator
    {
        public string ClassesNamespace { get; set; }
        private int maxThreadNumber;
        public int MaxThreadNumber
        {
            get
            {
                return maxThreadNumber;
            }
            set
            {
                if(value > 0)
                {
                    maxThreadNumber = value;
                }
            }
        }
        

        public CodeGenerator(string classesNamespace, int maxThreadNumber)
        {
            ClassesNamespace = classesNamespace;
            MaxThreadNumber = maxThreadNumber;
        }

        public GeneratedClasses GenerateDtoClasses(string jsonString)
        {
            
            if(jsonString == null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }
            if (ClassesNamespace == null)
            {
                throw new ArgumentNullException(nameof(ClassesNamespace));
            }

            ClassList list = ParseJsonString(jsonString);
            GeneratedClasses classes = GenerateClassesCode(list);

            return classes;
        }

        private ClassList ParseJsonString(string jsonString)
        {
            IParser<ClassList> parser = new ClassParser();
            return parser.Parse(jsonString);
        }

        private GeneratedClasses GenerateClassesCode(ClassList list)
        {
            ThreadPool.SetMaxThreads(maxThreadNumber, maxThreadNumber);
            ManualResetEvent[] doneEvents = new ManualResetEvent[list.classDescriptions.Length];
            TaskInfo[] parameters = new TaskInfo[list.classDescriptions.Length];
            InitializeThreadsParameters(parameters, doneEvents, list.classDescriptions);

            ICodeGenerator codeGenerator = new CSCodeGenerator();
            GeneratedClasses classes = new GeneratedClasses();
            for(int i = 0; i < parameters.Length; i++)
            {
                ThreadPool.QueueUserWorkItem(codeGenerator.GenerateCode, parameters[i]);
            }

            WaitHandle.WaitAll(doneEvents);

            for (int i = 0; i < parameters.Length; i++)
            {
                classes.AddClass(parameters[i].result);
            }

            return classes;
        }

        private void InitializeThreadsParameters(TaskInfo[] parameters, ManualResetEvent[] doneEvents, ClassDescription[] descriptions)
        {
            for(int i = 0; i < parameters.Length; i++)
            {
                ManualResetEvent resetEvent = new ManualResetEvent(false);
                doneEvents[i] = resetEvent;
                parameters[i] = new TaskInfo(descriptions[i], ClassesNamespace, resetEvent);
            }

        }

    }
}
