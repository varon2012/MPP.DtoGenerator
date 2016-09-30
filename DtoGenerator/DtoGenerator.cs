using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DtoGenerator.CodeGenerators;
using DtoGenerator.Config;
using DtoGenerator.DeserializedData;
using DtoGenerator.Plugins;

namespace DtoGenerator
{
    public sealed class DtoGenerator : IDisposable
    {
        private readonly ClassList sourceClassList;
        private readonly IConfig configData;
        private readonly ICodeGenerator codeGenerator;

        private readonly Task<TypeTable> asyncLoadingPlugins;
        private TypeTable TypeTable => asyncLoadingPlugins.Result;


        private readonly ThreadPool threadPool;
        private readonly Queue<GenerationClassUnit> pendingGenerationUnits;
        private readonly int maxWorkingTasksCount;
        private int currentWorkingTasksCount;
        private readonly CountdownEvent countdownEvent;

        public DtoGenerator(ClassList classList, ICodeGenerator codeGenerator, IConfig configData)
        {
            if (classList == null) throw new ArgumentNullException(nameof(classList));
            if (codeGenerator == null) throw new ArgumentNullException(nameof(codeGenerator));
            if (configData == null) throw new ArgumentNullException(nameof(configData));
            
            this.sourceClassList = classList;
            this.codeGenerator = codeGenerator;
            this.configData = configData;

            threadPool = new ThreadPool();
            countdownEvent = new CountdownEvent(sourceClassList.ClassDescriptions.Length);
            pendingGenerationUnits = new Queue<GenerationClassUnit>();
            asyncLoadingPlugins = new Task<TypeTable>(LoadPlugins);
            asyncLoadingPlugins.Start();
            maxWorkingTasksCount = configData.MaxTaskCount;
        }

        public List<GenerationResult> GenerateClasses()
        {
            List<GenerationClassUnit> generatedClassUnits = new List<GenerationClassUnit>();
            countdownEvent.Reset();
            currentWorkingTasksCount = 0;
            pendingGenerationUnits.Clear();

            foreach (ClassDescription classDescription in sourceClassList.ClassDescriptions)
            {
                GenerationClassUnit generationClassUnit =
                    new GenerationClassUnit(classDescription, configData.NamespaceName, TypeTable);
                generatedClassUnits.Add(generationClassUnit);

                AddToGenerationQueue(generationClassUnit);
            }
            
            countdownEvent.Wait();
            
            return GetGenerationResult(generatedClassUnits);
        }

        private void AddToGenerationQueue(GenerationClassUnit generationClassUnit)
        {
            lock (pendingGenerationUnits)
            {
                if (currentWorkingTasksCount < maxWorkingTasksCount)
                {
                    AddToWorkingPool(generationClassUnit);
                }
                else
                {
                    pendingGenerationUnits.Enqueue(generationClassUnit);
                }
            }
        }

        private List<GenerationResult> GetGenerationResult(List<GenerationClassUnit> generatedClassUnits)
        {
            List<GenerationResult> result = generatedClassUnits.Select(
                elem => new GenerationResult()
                    {
                        ClassName = elem.ClassDescription.ClassName,
                        Code = elem.Code
                }).ToList();
            return result;
        }

        private void AddToWorkingPool(GenerationClassUnit generationClassUnit)
        {
            currentWorkingTasksCount++;
            threadPool.QueueUserWorkItem(
                    codeGenerator.GenerateCode,
                    generationClassUnit,
                    () =>
                    {
                        lock (pendingGenerationUnits)
                        {
                            currentWorkingTasksCount--;
                            if (pendingGenerationUnits.Count > 0)
                            {
                                AddToWorkingPool(pendingGenerationUnits.Dequeue());
                            }
                            countdownEvent.Signal();
                        }
                    }
                );
        }

        private TypeTable LoadPlugins()
        {
            PluginLoader pluginLoader  = new PluginLoader();
            pluginLoader.LoadExternalTypes(configData.PluginsDirectory);
            return pluginLoader.TypeTable;
        }

        public void Dispose()
        {
            countdownEvent.Dispose();
            threadPool.Dispose();
        }
    }
}
