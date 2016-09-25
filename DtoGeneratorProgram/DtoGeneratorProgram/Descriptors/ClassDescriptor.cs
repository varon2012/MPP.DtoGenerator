using System.Collections.Generic;

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
