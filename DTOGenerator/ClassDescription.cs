using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOGenerator
{
    public class ClassDescription
    {
        public string ClassName { get; private set; }
        public List<PropertyDescription> PropertyDescriptions { get; private set; }

        public ClassDescription(string className)
        {
            ClassName = className;
            PropertyDescriptions = new List<PropertyDescription>();
        }
    }
}
