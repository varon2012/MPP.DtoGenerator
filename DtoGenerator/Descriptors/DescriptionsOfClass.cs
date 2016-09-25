using System.Collections.Generic;

namespace DtoGenerator.Descriptors
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
