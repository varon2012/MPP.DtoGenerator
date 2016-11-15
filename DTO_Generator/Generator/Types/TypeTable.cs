using System;
using System.Collections.Generic;

namespace Generator.Types
{
    public class TypeTable
    {
        private static readonly Lazy<TypeTable> lazy =
            new Lazy<TypeTable>(() => new TypeTable(), true);

        public static TypeTable GetInstance()
        {
            return lazy.Value;
        }

        private TypeTable()
        {
            typeDictionary = new Dictionary<string, Dictionary<string, Type>>();
            InitializeTable();
        }

        private ExactType[] types = new ExactType[]
        {
            new ExactType() {Type = "integer", Format = "int32",  DotNetType = typeof(int)},
            new ExactType() {Type = "integer", Format = "int64",  DotNetType = typeof(long)},
            new ExactType() {Type = "number",  Format = "float",  DotNetType = typeof(float)},
            new ExactType() {Type = "number",  Format = "double", DotNetType = typeof(double)},
            new ExactType() {Type = "string",  Format = "byte",   DotNetType = typeof(byte)},
            new ExactType() {Type = "boolean", Format = "",       DotNetType = typeof(bool)},
            new ExactType() {Type = "string",  Format = "date",   DotNetType = typeof(DateTime)},
            new ExactType() {Type = "string",  Format = "string", DotNetType = typeof(string)},
        };

        private readonly Dictionary<string, Dictionary<string, Type>> typeDictionary;

        private void InitializeTable()
        {
            foreach (var type in types)
            {
                AddToDictionary(type);
            }
        }

        private void AddToDictionary(ExactType type)
        {
            if (typeDictionary.ContainsKey(type.Type))
                typeDictionary[type.Type].Add(type.Format, type.DotNetType);
            else
            {
                var tempDictionary = new Dictionary<string, Type>();
                tempDictionary.Add(type.Format, type.DotNetType);
                typeDictionary.Add(type.Type, tempDictionary);
            }
        }

        public Type GetCSharpType(string format, string type)
        {
            if (typeDictionary.ContainsKey(type))
            {
                Dictionary<string, Type> dictionary = typeDictionary[type];
                if (dictionary.ContainsKey(format))
                {
                    return dictionary[format];
                }
                throw new KeyNotFoundException($"DotNetType with type = {type} and format = {format}  doesn't exist");
            }
            throw new KeyNotFoundException($"DotNetType with type = {type} and format = {format}  doesn't exist");
        }
    }
}
