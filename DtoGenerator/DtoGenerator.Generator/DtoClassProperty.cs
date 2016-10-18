using System;

namespace DtoGenerator.Generator
{
    public struct DtoClassProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }

        public DtoClassProperty(string name, string type) 
            : this(name, type, null)
        {
        }

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
