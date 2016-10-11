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
            private set
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
            private set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(maxThreadNumber));
                maxThreadNumber = value;
            }
        }

        private ManualResetEvent[] doneEvents;
        private TaskInfo[] tasksInQueue;
        private ICodeGenerator codeGenerator;


        public DtoCodeGenerator(string classesNamespace, int maxThreadNumber)
        {
            ClassesNamespace = classesNamespace;
            MaxThreadNumber = maxThreadNumber;
            codeGenerator = new CSCodeGenerator();
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

        private GeneratedClassList GenerateClassesCode(ClassDescriptionList classList)
        {
            ClassDescription[] descriptions = classList.classDescriptions;
            int allTaskCount = descriptions.Length;
            int maxQueueSize = Math.Min(allTaskCount, maxThreadNumber);

            InitiateTaskQueue(maxQueueSize, descriptions);
            GeneratedClassList classes = RefillQueueOnThreadFinishing(maxQueueSize, allTaskCount, descriptions);
            WaitAllThreadsFinishing(classes);

            return classes;
        }

        private void InitiateTaskQueue(int queueSize, ClassDescription[] classDescriptions)
        {
            doneEvents = new ManualResetEvent[queueSize];
            tasksInQueue = new TaskInfo[queueSize];

            for (int threadIndex = 0; threadIndex < queueSize; threadIndex++)
            {
                TaskInfo task = CreateThreadTask(threadIndex, classDescriptions[threadIndex]);
                ThreadPool.QueueUserWorkItem(codeGenerator.GenerateCode, task);
            }
        }

        private GeneratedClassList RefillQueueOnThreadFinishing(int addedTaskCount, int allTaskCount, ClassDescription[] descriptions)
        {
            GeneratedClassList classes = new GeneratedClassList();
            for(int taskIndex = addedTaskCount; taskIndex < allTaskCount; taskIndex++)
            {
                int finishedThreadIndex = WaitHandle.WaitAny(doneEvents);
                classes.AddClass(tasksInQueue[finishedThreadIndex].result);

                AddTaskToQueue(finishedThreadIndex, descriptions[taskIndex]);
                addedTaskCount++;
            }
            return classes;
        }

        private void WaitAllThreadsFinishing(GeneratedClassList classes)
        {
            WaitHandle.WaitAll(doneEvents);
            for (int i = 0; i < tasksInQueue.Length; i++)
            {
                classes.AddClass(tasksInQueue[i].result);
            }
        }

        private void AddTaskToQueue(int threadIndex, ClassDescription description)
        {
            TaskInfo task = CreateThreadTask(threadIndex, description);
            ThreadPool.QueueUserWorkItem(codeGenerator.GenerateCode, tasksInQueue[threadIndex]);
        }

        private TaskInfo CreateThreadTask(int threadIndex, ClassDescription descriptions)
        {
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            doneEvents[threadIndex] = resetEvent;
            TaskInfo task = new TaskInfo(descriptions, ClassesNamespace, resetEvent);
            tasksInQueue[threadIndex] = task;

            return task;
        }
    }
}
