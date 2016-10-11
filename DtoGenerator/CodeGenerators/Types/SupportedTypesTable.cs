using System;
using System.Collections.Generic;

namespace DtoGenerator.CodeGenerators.Types
{
    internal class SupportedTypesTable
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

        private class Key : IEquatable<Key>
        {
            private string type;
            private string format;
            private int hashCode;

            public Key(string type, string format)
            {
                this.type = type;
                this.format = format;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int result = hashCode;
                    if(result == 0)
                    {
                        result = 17;
                        result = 31 * result + type.GetHashCode();
                        result = 31 * result + format.GetHashCode();
                    hashCode = result;
                    }
                    return result;
                }
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(obj, null)) return false;
                if (ReferenceEquals(obj, this)) return true;
                if (obj.GetType() != GetType()) return false;

                return Equals(obj as Key);
            }

            public bool Equals(Key other)
            {
                if (other == null) return false;
                return string.Equals(type, other.type) && string.Equals(format, other.format);
            }
        }
    }

    
}
