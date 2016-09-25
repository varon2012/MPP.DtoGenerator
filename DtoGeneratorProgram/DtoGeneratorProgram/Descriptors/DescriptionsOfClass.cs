using System.Collections.Generic;

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
