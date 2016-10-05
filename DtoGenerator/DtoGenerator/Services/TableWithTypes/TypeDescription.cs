using DtoGenerator.Contracts.Plugins;
using System;

namespace DtoGenerator.Services.TableWithTypes
{
    public class TypeDescription : ITypeDescription
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public Type DotNetType { get; set; }

        public TypeDescription(string type, string format, Type dotNetType)
        {
            Type = type;
            Format = format;
            DotNetType = dotNetType;
        }
    }
}
