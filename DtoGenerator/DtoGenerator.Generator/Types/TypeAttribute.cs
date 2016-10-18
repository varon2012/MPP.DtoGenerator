using System;

namespace DtoGenerator.Generator.Types
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TypeAttribute : Attribute
    {
        public string Name { get; set; }
        public string Format { get; set; }

        public TypeAttribute() : this(null)
        {
        }

        public TypeAttribute(string format) : this(null, format)
        {
        }

        public TypeAttribute(string name, string format)
        {
            Name = name;
            Format = format;
        }
    }
}
