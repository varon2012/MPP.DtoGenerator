using DtoGenerator.Contracts.Plugins;
using System;

namespace DtoGenerator.Services.TablesWithTypes
{
    internal class TypeDescription : ITypeDescription
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public Type DotNetType { get; set; }

        internal TypeDescription(string type, string format, Type dotNetType)
        {
            Type = type;
            Format = format;
            DotNetType = dotNetType;
        }
    }
}
