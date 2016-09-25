using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGeneratorProgram.Descriptors
{
    public class ClassDescriptor
    {
        public string ClassName { get; set; }
        public List<PropertyDescriptor> properties;

        public ClassDescriptor()
        {
            properties = new List<PropertyDescriptor>();
        }
    }
}
