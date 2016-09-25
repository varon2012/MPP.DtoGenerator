using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGeneratorProgram.Descriptors
{
    internal class DescriptionsOfClass
    {
        public List<ClassDescriptor> classDescriptions { get; set; }

        public DescriptionsOfClass()
        {
            classDescriptions = new List<ClassDescriptor>();
        }
    }
}
