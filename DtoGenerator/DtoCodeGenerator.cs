using DtoGenerator.CodeGenerators;
using DtoGenerator.CodeGenerators.GeneratedItems;
using DtoGenerator.DtoDescriptor;
using DtoGenerator.DtoDescriptors;
using DtoGenerator.Parser;
using System;
using System.Threading;

namespace DtoGenerator
{
    public class DtoCodeGenerator
    {
        private string classesNamespace;
        public string ClassesNamespace
        {
            get
            {
                return classesNamespace;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(classesNamespace));
                classesNamespace = value;
            }
        }
        private int maxThreadNumber;
        public int MaxThreadNumber
        {
            get
            {
                return maxThreadNumber;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(maxThreadNumber));
                maxThreadNumber = value;
            }
        }

        private ManualResetEvent[] doneEvents;
        private TaskInfo[] tasks;


        public DtoCodeGenerator(string classesNamespace, int maxThreadNumber)
        {
            ClassesNamespace = classesNamespace;
            MaxThreadNumber = maxThreadNumber;
        }

        public GeneratedClassList GenerateDtoClasses(string jsonString)
        {
            
            if(jsonString == null) throw new ArgumentNullException(nameof(jsonString));

            ClassDescriptionList list = ParseJsonString(jsonString);
            GeneratedClassList classes = GenerateClassesCode(list);

            return classes;
        }

        private ClassDescriptionList ParseJsonString(string jsonString)
        {
            IParser<ClassDescriptionList> parser = new JsonStringParser();
            return parser.Parse(jsonString);
        }

        private GeneratedClassList GenerateClassesCode(ClassDescriptionList list)
        {
            InitializeThreadsTasks(list.classDescriptions);
            AddTasksToQueue();
            WaitHandle.WaitAll(doneEvents);

            GeneratedClassList classes = new GeneratedClassList();
            for (int i = 0; i < tasks.Length; i++)
            {
                classes.AddClass(tasks[i].result);
            }

            return classes;
        }

        private void InitializeThreadsTasks(ClassDescription[] descriptions)
        {
            doneEvents = new ManualResetEvent[descriptions.Length];
            tasks = new TaskInfo[descriptions.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                ManualResetEvent resetEvent = new ManualResetEvent(false);
                doneEvents[i] = resetEvent;
                tasks[i] = new TaskInfo(descriptions[i], ClassesNamespace, resetEvent);
            }

        }

        private void AddTasksToQueue()
        {
            ThreadPool.SetMaxThreads(maxThreadNumber, maxThreadNumber);
            ICodeGenerator codeGenerator = new CSCodeGenerator();

            for (int i = 0; i < tasks.Length; i++)
            {
                ThreadPool.QueueUserWorkItem(codeGenerator.GenerateCode, tasks[i]);
            }
        }

    }
}
