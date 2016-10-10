using System;
using System.Collections.Generic;

namespace DtoGenerator.CodeGenerators.Types
{
    class SupportedTypesTable
    {
        private const string UndefinedType = "undefined";
        private Dictionary<Key, string> typeTable;

        public SupportedTypesTable()
        {
            typeTable = new Dictionary<Key, string>();

            AddSupportedType("integer", "int32", "System.Int32");
            AddSupportedType("integer", "int64", "System.Int64");
            AddSupportedType("number", "float", "System.Single");
            AddSupportedType("number", "double", "System.Double");
            AddSupportedType("string", "byte", "System.Byte");
            AddSupportedType("string", "date", "System.DateTime");
            AddSupportedType("string", "string", "System.String");
            AddSupportedType("boolean", "", "System.Boolean");
        }

        public void AddSupportedType(string type, string format, string netType)
        {
            if(type == null) throw new ArgumentNullException(nameof(type));
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (netType== null) throw new ArgumentNullException(nameof(netType));

            Key key = new Key(type, format);
            if (!typeTable.ContainsKey(key))
            {
                typeTable.Add(key, netType);
            }
            
        }

        public string GetNetType(string type, string format)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (format == null) throw new ArgumentNullException(nameof(format));

            Key key = new Key(type, format);
            if (typeTable.ContainsKey(key))
            {
                return typeTable[key];
            }
            else
            {
                return UndefinedType;
            }
        }

        private class Key
        {
            private string type;
            private string format;

            public Key(string type, string format)
            {
                this.type = type;
                this.format = format;
            }

            public override int GetHashCode()
            {
                return 15485863 | type.GetHashCode() & format.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Key key = obj as Key;
                return type.Equals(key.type) && format.Equals(key.format);
            }
        }
    }

    
}
