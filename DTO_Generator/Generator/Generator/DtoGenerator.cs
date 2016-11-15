using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Generator.CodeGenerators;
using Generator.Descriptions;
using Generator.Generator;

namespace Generator
{
    public class DtoGenerator : IDtoGenerator
    {
        private readonly ClassesList descriptions;
        private readonly ICodeGenerator codeGenerator;
        private readonly IConfiguration config;

        private readonly int maxTaskCount;
        private readonly string nameSpace;

        private readonly CountdownEvent complete;
        private readonly Queue<ClassTemplate> waitingClasses;
        private int currentTaskCount;

        public DtoGenerator(ClassesList descriptions, ICodeGenerator codeGenerator, IConfiguration config)
        {
            if (descriptions == null) throw new ArgumentNullException(nameof(descriptions));
            if (descriptions.Classes == null) throw new ArgumentNullException(nameof(descriptions.Classes));
            if (codeGenerator == null) throw new ArgumentNullException(nameof(codeGenerator));
            if (config == null) throw new ArgumentNullException(nameof(config));

            this.descriptions = descriptions;
            this.codeGenerator = codeGenerator;
            this.config = config;
            maxTaskCount = config.MaxTaskCount;
            nameSpace = config.Namespace;

            complete = new CountdownEvent(descriptions.Classes.Length);
            waitingClasses = new Queue<ClassTemplate>();
            currentTaskCount = 0;
        }

        public List<GeneratedClass> GenerateClasses()
        {
            complete.Reset();
            currentTaskCount = 0;
            waitingClasses.Clear();

            List<ClassTemplate> generatedClassesList = new List<ClassTemplate>();
            foreach (var classDescription in descriptions.Classes)
            {
                var templateClass = new ClassTemplate
                {
                    Description = classDescription,
                    Namespace = nameSpace
                };
                generatedClassesList.Add(templateClass);
                AddToQueue(templateClass);
            }

            complete.Wait();

            return GetResult(generatedClassesList);
        }

        private void AddToQueue(ClassTemplate template)
        {
            lock (waitingClasses)
            {
                if (currentTaskCount < maxTaskCount)
                    AddTaskToThreadPool(template);
                else
                    waitingClasses.Enqueue(template);
            }
        }

        private class Helper
        {
            public delegate void ThreadFinishCallback();

            public ClassTemplate Template { get; set; }
            public ThreadFinishCallback FinallyCallback { get; set; }
        }

        private void AddTaskToThreadPool(ClassTemplate template)
        {
            currentTaskCount++;
            Helper helper = new Helper()
            {
                Template = template,
                FinallyCallback = () =>
                {
                    lock (waitingClasses)
                    {
                        currentTaskCount--;
                        if (waitingClasses.Count > 0)
                            AddTaskToThreadPool(waitingClasses.Dequeue());
                        complete.Signal();
                    }
                }
            };

            ThreadPool.QueueUserWorkItem(
                (help) => { Generate(helper); }
            );
        }

        private void Generate(Helper helper)
        {
            codeGenerator.GenerateCode(helper.Template);
            helper.FinallyCallback();
        }

        private List<GeneratedClass> GetResult(List<ClassTemplate> list)
        {
            var generatedClasses = new List<GeneratedClass>();
            foreach (var @class in list)
            {
                generatedClasses.Add(new GeneratedClass()
                {
                    ClassName = @class.Description.ClassName,
                    Code = @class.Code
                });
            }
            return generatedClasses;
        }
    }
}
