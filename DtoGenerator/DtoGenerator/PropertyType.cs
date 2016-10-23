using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    internal class PropertyType
    {
        private Dictionary<TypeKey, string> types;

        public PropertyType()
        {
            types = new Dictionary<TypeKey, string>();
            AddTypes();
        }
        
        private void AddTypes()
        {
            types.Add(new TypeKey("integer", "int32"), "Int32");
            types.Add(new TypeKey("integer", "int64"), "Int64");
            types.Add(new TypeKey("number", "float"), "Single");
            types.Add(new TypeKey("number", "double"), "Double");
            types.Add(new TypeKey("string", "byte"), "Byte");
            types.Add(new TypeKey("boolean", ""), "Boolean");
            types.Add(new TypeKey("string", "date"), "DateTime");
            types.Add(new TypeKey("string", "string"), "String");
        }

        public string GetType(string type, string format)
        {
            if (type == null || format == null)
                throw new ArgumentNullException();

            TypeKey key = new TypeKey(type, format);

            if (types.ContainsKey(key))
                return types[key];
            else
                throw new KeyNotFoundException();
        }
    }
}
