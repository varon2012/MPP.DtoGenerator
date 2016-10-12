using System;
using System.Collections.Generic;

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
