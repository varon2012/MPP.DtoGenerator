using System.Collections.Generic;

namespace DtoGenerator.Descriptors
{
    public class ClassDescriptor
    {
        public string ClassName { get; set; }
        public List<PropertyDescriptor> Properties { get; set; }    

        public ClassDescriptor()
        {
            Properties = new List<PropertyDescriptor>();
        }
    }
}
