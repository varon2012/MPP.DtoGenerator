using System;

namespace DtoGenerator.Generator
{
    public class DtoClassProperty
    {
        public string Name { get; }
        public string Type { get; }
        public string Format { get; }

        public DtoClassProperty(string name, string type, string format)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));
            // type format can be null

            Name = name;
            Type = type;
            Format = format;
        }
    }
}
