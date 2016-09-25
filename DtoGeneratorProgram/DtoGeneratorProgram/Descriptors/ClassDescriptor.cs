using System.Collections.Generic;

namespace DtoGeneratorProgram.Descriptors
{
    public class ClassDescriptor
    {
        public string className { get; set; }
        public List<PropertyDescriptor> properties { get; set; }    

        public ClassDescriptor()
        {
            properties = new List<PropertyDescriptor>();
        }
    }
}
