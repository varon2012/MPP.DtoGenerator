using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DtoGenerator.Generator.Types;
using TextFormatters;
using ThreadPool;

namespace DtoGenerator.Generator
{
    public sealed class ClassCodeGenerator
    {
        private readonly Config _config;
        private readonly ICodeGenerator _generator;

        public ClassCodeGenerator(Config config, ICodeGenerator generator)
        {
            _config = config;
            _generator = generator;
        }

        public IDictionary<string, string> Generate(IEnumerable<DtoClassDescription> classDescriptions, ILogger logger)
        {
            var result = new ConcurrentDictionary<string, string>();
            var typeResolver = _config.PluginsDirectoryPath != null
                ? new TypeResolver(_config.PluginsDirectoryPath, logger)
                : new TypeResolver();

            using (var threadPool = new CustomThreadPool(_config.MaxTaskCount))
            {
                foreach (var classDescription in classDescriptions)
                {
                    threadPool.AddToProcessingQueue(new TaskInfo
                    {
                        WaitCallback = data =>
                        {
                            result[classDescription.ClassName] =
                                _generator.Generate(_config.ClassesNamespace, classDescription,
                                    typeResolver);
                        }
                    });
                }

                threadPool.Countdown.Wait();

                return result;
            }
        }
    }
}
