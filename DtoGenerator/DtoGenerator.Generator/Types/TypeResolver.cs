using System;
using System.Collections.Generic;
using System.Reflection;

namespace DtoGenerator.Generator.Types
{
    public sealed class TypeResolver
    {
        private readonly Dictionary<string, TypeFormats> _types;

        public TypeResolver()
        {
            _types = new Dictionary<string, TypeFormats>
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
        }

        public TypeResolver(string pluginsDirectory)
            : this()
        {
            if (pluginsDirectory == null) throw new ArgumentNullException(nameof(pluginsDirectory));

            var types = PluginsLoader.GetTypesWithAttribute<TypeAttribute>(pluginsDirectory);

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

        public Type ResolveType(string type, string format)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            // type format can be null

            try
            {
                return _types[type].ResolveFormat(format);
            }
            catch (TypeFormatNotFoundException e)
            {
                throw new TypeException($"Error occurred with type '{type}':", e);
            }
            catch (KeyNotFoundException)
            {
                throw new TypeException($"Type missing: {type}");
            }
        }
    }
}
