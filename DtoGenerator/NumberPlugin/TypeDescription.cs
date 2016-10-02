using System;
using DtoGenerator.Contracts.Plugins;

namespace NumberPlugin
{
    public class TypeDescription : ITypeDescription
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public Type DotNetType { get; set; }
    }
}
