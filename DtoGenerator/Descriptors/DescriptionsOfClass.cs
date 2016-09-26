using System.Collections.Generic;

namespace DtoGenerator.Descriptors
{
    public class DescriptionsOfClass
    {
        public List<ClassDescriptor> classDescriptions { get; set; }
        public string Namespace { get; set; }

        public DescriptionsOfClass()
        {
            classDescriptions = new List<ClassDescriptor>();
        }
    }
}
