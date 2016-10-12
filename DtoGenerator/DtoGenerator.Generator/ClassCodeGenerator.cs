using System.Collections.Generic;
using DtoGenerator.Generator.Types;

namespace DtoGenerator.Generator
{
    public class ClassCodeGenerator
    {
        private readonly string _specifiedNamespace;
        private readonly ICodeGenerator _generator;

        public ClassCodeGenerator(string specifiedNamespace, ICodeGenerator generator)
        {
            _specifiedNamespace = specifiedNamespace;
            _generator = generator;
        }

        public IDictionary<string, string> Generate(IEnumerable<DtoClassDescription> classDescriptions)
        {
            var result = new Dictionary<string, string>();

            // TODO: use thread pool
            foreach (var classDescription in classDescriptions)
            {
                result[classDescription.ClassName] = 
                    _generator.Generate(_specifiedNamespace, classDescription, new TypeResolver());
            }

            return result;
        }
    }
}
