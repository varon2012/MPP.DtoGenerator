using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.CodeGenerators.Types
{
    class SupportedTypesTable
    {
        private const string UndefinedType = "undefined";
        private Dictionary<Key, string> typeTable;

        public SupportedTypesTable()
        {
            typeTable = new Dictionary<Key, string>();

            addSupportedType("integer", "int32", "System.Int32");
            addSupportedType("integer", "int64", "System.Int64");
            addSupportedType("number", "float", "System.Single");
            addSupportedType("number", "double", "System.Double");
            addSupportedType("string", "byte", "System.Byte");
            addSupportedType("string", "date", "System.DateTime");
            addSupportedType("string", "string", "System.String");
            addSupportedType("boolean", "", "System.Boolean");
        }

        public void addSupportedType(string type, string format, string netType)
        {
            if(type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }
            if (netType== null)
            {
                throw new ArgumentNullException(nameof(netType));
            }

            Key key = new Key(type, format);
            if (!typeTable.ContainsKey(key))
            {
                typeTable.Add(key, netType);
            }
            
        }

        public string getNetType(string type, string format)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

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
                Key key = (Key)obj;
                return type.Equals(key.type) && format.Equals(key.format);
            }
        }
    }

    
}
