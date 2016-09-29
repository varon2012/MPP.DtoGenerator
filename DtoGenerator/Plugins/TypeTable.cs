using System;
using System.Collections.Generic;
using DtoPlugin;

namespace DtoGenerator.Plugins
{
    public class TypeTable
    {
        private readonly Dictionary<string, Dictionary<string, Type>> typeTable;

        public TypeTable()
        {
            typeTable = new Dictionary<string, Dictionary<string, Type>>();
        }

        public void AddType(IDtoPlugin plugin)
        {
            string type = plugin.Type;
            string format = plugin.Format;
            Type typeObj = plugin.TypeObj;

            Dictionary<string, Type> formatDictionary = GetOrCreateDictElement(typeTable, type);

            if (formatDictionary.ContainsKey(format))
            {
                throw new InvalidOperationException("Plugin with the same type and format already exists");
            }

            formatDictionary.Add(format, typeObj);
        }

        public Type GetCSharpTypeByFormatAndType(string type, string format)
        {
            Dictionary<string, Type> formatDictionary;
            if (!typeTable.TryGetValue(type, out formatDictionary))
            {
                throw new InvalidOperationException($"There are no plugin for type = {type} and format = {format}");
            }

            Type targetType;
            if (!formatDictionary.TryGetValue(format, out targetType))
            {
                throw new InvalidOperationException($"There are no plugin for type = {type} and format = {format}");
            }

            return targetType;
        }

        private TValue GetOrCreateDictElement<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            TValue val;

            if (!dict.TryGetValue(key, out val))
            {
                val = new TValue();
                dict.Add(key, val);
            }

            return val;
        }
    }
}
