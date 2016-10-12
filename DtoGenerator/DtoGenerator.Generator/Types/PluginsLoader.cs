using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DtoGenerator.Generator.Types
{
    internal static class PluginsLoader
    {
        public static IEnumerable<Type> GetTypesWithAttribute<T>(string path)
            where T: Attribute
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var result = new List<Type>();

            foreach (var file in Directory.EnumerateFiles(path))
            {
                var assemble = Assembly.LoadFile(Path.GetFullPath(file));
                var types = assemble.GetExportedTypes();
                result.AddRange(types.Where(type => type.GetCustomAttribute(typeof(T)) != null));
            }

            return result;
        }
    }
}
