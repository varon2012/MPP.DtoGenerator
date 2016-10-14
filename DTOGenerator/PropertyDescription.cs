using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOGenerator
{
    public class PropertyDescription
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }

        public PropertyDescription(string name, Type type)
        {
            Name = name;
            Type = type;    
        }
    }
}
