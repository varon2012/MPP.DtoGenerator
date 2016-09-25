using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGeneratorProgram.Descriptors
{
    internal class ClassDescriptor
    {
        public string className { get; set; }
        public List<PropertyDescriptor> properties { get; set; }    

        public ClassDescriptor()
        {
            properties = new List<PropertyDescriptor>();
        }
    }
}
