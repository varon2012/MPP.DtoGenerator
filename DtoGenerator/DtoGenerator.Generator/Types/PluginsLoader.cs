using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TextFormatters;

namespace DtoGenerator.Generator.Types
{
    internal sealed class PluginsLoader
    {
        private readonly ILogger _logger;

        public PluginsLoader(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<Type> GetTypesWithAttribute<T>(string path)
            where T : Attribute
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var result = new List<Type>();

            foreach (var file in Directory.EnumerateFiles(path))
            {
                try
                {
                    var assemble = Assembly.LoadFile(Path.GetFullPath(file));
                    var types = assemble.GetExportedTypes();
                    result.AddRange(types.Where(type => type.GetCustomAttribute(typeof(T)) != null));
                }
                catch (BadImageFormatException e)
                {
                    _logger?.Log(e.Message);
                }
            }

            return result;
        }
    }
}
