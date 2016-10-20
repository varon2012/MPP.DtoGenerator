using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    internal class PropertyType
    {
        private class TypeKey
        {
            public string Type { get; }
            public string Format { get; }

            public TypeKey(string type,string format)
            {
                Type = type;
                Format = format;
            }
        }

        private Dictionary<TypeKey, Type> types = new Dictionary<TypeKey, Type>();

        private void AddTypes()
        {
            types.Add(new TypeKey("integer", "int32"), typeof(Int32));
            types.Add(new TypeKey("integer", "int64"), typeof(Int64));
            types.Add(new TypeKey("number", "float"), typeof(Single));
            types.Add(new TypeKey("number", "double"), typeof(Double));
            types.Add(new TypeKey("string", "byte"), typeof(Byte));
            types.Add(new TypeKey("boolean", ""), typeof(Boolean));
            types.Add(new TypeKey("string", "date"), typeof(DateTime));
            types.Add(new TypeKey("string", "string"), typeof(String));
        }

        public Type GetType(string type, string format)
        {
            foreach (var key in types.Keys)
            {
                if (key.Type == type && key.Format == format)
                    return types[key];
            }

            return null;
        }
    }
}
