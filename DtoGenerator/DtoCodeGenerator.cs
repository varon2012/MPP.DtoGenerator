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
        private int maxThreadNumber;
        private ManualResetEvent[] doneEvents;
        private TaskInfo[] tasksPool;
        private ICodeGenerator codeGenerator;
        private GeneratedClassList generatedClasses;


        public DtoCodeGenerator(string classesNamespace, int maxThreadNumber)
        {
            if (maxThreadNumber < 0) throw new ArgumentOutOfRangeException(nameof(maxThreadNumber));
            if (classesNamespace == null) throw new ArgumentNullException(nameof(classesNamespace));

            this.classesNamespace = classesNamespace;
            this.maxThreadNumber = maxThreadNumber;
            codeGenerator = new CSCodeGenerator();
            generatedClasses = new GeneratedClassList();
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
            ClassDescription[] classDescriptions = classList.classDescriptions;
            int allTaskCount = classDescriptions.Length;
            int taskPoolSize = Math.Min(allTaskCount, maxThreadNumber);

            generatedClasses.ClearClassList();

            InitializeTasks(taskPoolSize, classDescriptions);
            int taskAdded = 0;
            while(taskAdded < taskPoolSize)
            {
                AddTaskToQueue(tasksPool[taskAdded]);
                taskAdded++;
            }
            while(taskAdded < allTaskCount)
            {
                RefillQueueOnTaskFinishing(classDescriptions[taskAdded++]);
            }
            FinishAllTasks();

            return generatedClasses;
        }

        private void InitializeTasks(int taskPoolSize, ClassDescription[] classDescriptions)
        {
            tasksPool = new TaskInfo[taskPoolSize];
            doneEvents = new ManualResetEvent[taskPoolSize];

            for (int i = 0; i < taskPoolSize; i++)
            {
                tasksPool[i] = CreateNewTask(i, classDescriptions[i]);
            }
        }

        private TaskInfo CreateNewTask(int taskIndex, ClassDescription classDescription)
        {
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            doneEvents[taskIndex] = resetEvent;
            TaskInfo task = new TaskInfo(classDescription, classesNamespace, resetEvent);

            return task;
        }

        private void AddTaskToQueue(TaskInfo task)
        {
            ThreadPool.QueueUserWorkItem(codeGenerator.GenerateCode, task);
        }

        private void RefillQueueOnTaskFinishing(ClassDescription classDescription)
        {
            int finishedTaskIndex = WaitHandle.WaitAny(doneEvents);
            FinishTask(finishedTaskIndex);

            TaskInfo newTask = CreateNewTask(finishedTaskIndex, classDescription);
            tasksPool[finishedTaskIndex] = newTask;
            AddTaskToQueue(newTask);
        }

        private void FinishTask(int taskIndex)
        {
            TaskInfo finishedTask = tasksPool[taskIndex];
            generatedClasses.AddClass(finishedTask.Result);
            finishedTask.Dispose();
        }

        private void FinishAllTasks()
        {
            WaitHandle.WaitAll(doneEvents);
            for (int i = 0; i < tasksPool.Length; i++)
            {
                FinishTask(i);
            }
        }
    }
}
