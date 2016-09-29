using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DtoGenerator.CodeGenerators;
using DtoGenerator.Config;
using DtoGenerator.DeserializedData;
using DtoGenerator.Plugins;

namespace DtoGenerator
{
    public class DtoGenerator
    {
        private readonly ClassList sourceClassList;
        private readonly ConfigData configData;

        private readonly Task<TypeTable> asyncLoadingPlugins;
        private TypeTable TypeTable => asyncLoadingPlugins.Result;
        

        private readonly ICodeGenerator codeGenerator;

        public DtoGenerator(ClassList classList, ICodeGenerator codeGenerator)
        {
            this.sourceClassList = classList;
            this.codeGenerator = codeGenerator;
            configData = new ConfigData();
            asyncLoadingPlugins = new Task<TypeTable>(LoadPlugins);
            asyncLoadingPlugins.Start();
        }

        public List<GeneratingClassUnit> GenerateClasses()
        {
            List<GeneratingClassUnit> generatedClassUnits = new List<GeneratingClassUnit>();

            using(ThreadPool threadPool = new ThreadPool(configData.ThreadCount, true))
            using (CountdownEvent countdownEvent = new CountdownEvent(sourceClassList.ClassDescriptions.Length))
            {

                foreach (ClassDescription classDescription in sourceClassList.ClassDescriptions)
                {
                    GeneratingClassUnit generatingClassUnit =
                        new GeneratingClassUnit(classDescription, configData.NamespaceName, TypeTable);
                    generatedClassUnits.Add(generatingClassUnit);
                    threadPool.QueueUserWorkItem(
                            codeGenerator.GenerateCode,
                            generatingClassUnit,
                            () => countdownEvent.Signal()
                        );

                }
                countdownEvent.Wait();

            }


            return generatedClassUnits;
        }

        private TypeTable LoadPlugins()
        {
            PluginLoader pluginLoader  = new PluginLoader();
            pluginLoader.LoadExternalTypes(configData.PluginsDirectory);
            return pluginLoader.TypeTable;
        }
    }
}
