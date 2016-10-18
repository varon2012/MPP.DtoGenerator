using System;
using System.Collections.Generic;
using TextFormatters;

namespace DtoGenerator.Generator.Types
{
    internal sealed class TypeBuilder
    {
        private IEnumerable<Type> _loadedTypes;

        public TypeBuilder(string pluginsDirectory, ILogger logger)
        {
            if (pluginsDirectory != null)
            {
                _loadedTypes = new PluginsLoader(logger).GetTypesWithAttribute<TypeAttribute>(pluginsDirectory);
            }
        }

        public IDictionary<string, TypeFormats> GetDefaultTypes() => 
            new Dictionary<string, TypeFormats>
        {
            {
                "integer", new TypeFormats()
                    .AddTypeFormat("int32", typeof(int))
                    .AddTypeFormat("int64", typeof(long))
            },
            {
                "number", new TypeFormats()
                    .AddTypeFormat("float", typeof(float))
                    .AddTypeFormat("double", typeof(double))
            },
            {
                "string", new TypeFormats()
                    .AddTypeFormat("byte", typeof(byte))
                    .AddTypeFormat("date", typeof(DateTime))
                    .AddTypeFormat("string", typeof(string))
            },
            {
                "boolean", new TypeFormats(typeof(bool))
            }
        };

        public IDictionary<string, TypeFormats> GetLoadedTypes(TypeResolver typeResolver)
        {
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute(typeof(TypeAttribute)) as TypeAttribute;
                if (attribute == null) continue;

                var typeName = attribute.Name ?? type.Name;
                var typeFormat = attribute.Format;
                TypeFormats typeFormats;

                if (!_types.TryGetValue(typeName, out typeFormats))
                {
                    typeFormats = typeFormat == null ? new TypeFormats(type) : new TypeFormats();
                    _types[typeName] = typeFormats;
                }

                if (!typeFormats.HasSingleFormat)
                {
                    typeFormats.AddTypeFormat(typeFormat, type);
                }
            }
        }
    }
}
