using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.DtoDescriptor
{
    public class PropertyDescriptor
    {
        public string Name { get; }
        public string Type { get; }
        public string Format { get; }

        public PropertyDescriptor(string name, string type, string format)
        {
            if (name == null || type == null || format == null)
            {
                throw new ArgumentNullException();
            }

            Name = name;
            Type = type;
            Format = format;
        }
    }
}
