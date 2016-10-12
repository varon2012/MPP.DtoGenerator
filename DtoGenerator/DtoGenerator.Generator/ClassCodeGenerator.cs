using System.Collections.Generic;
using DtoGenerator.Generator.Types;

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

        public IDictionary<string, string> Generate(IEnumerable<DtoClassDescription> classDescriptions)
        {
            var result = new Dictionary<string, string>();

            // TODO: use thread pool
            foreach (var classDescription in classDescriptions)
            {
                result[classDescription.ClassName] = 
                    _generator.Generate(_config.ClassesNamespace, classDescription, new TypeResolver(_config.PluginsDirectoryPath));
            }

            return result;
        }
    }
}
