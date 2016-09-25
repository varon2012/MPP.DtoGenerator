using System.Collections.Generic;

namespace DtoGeneratorProgram.Descriptors
{
    public class DescriptionsOfClass
    {
        public List<ClassDescriptor> classDescriptions { get; set; }

        public DescriptionsOfClass()
        {
            classDescriptions = new List<ClassDescriptor>();
        }
    }
}
