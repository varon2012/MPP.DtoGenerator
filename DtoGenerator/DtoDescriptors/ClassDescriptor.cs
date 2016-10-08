using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.DtoDescriptor
{
    class ClassDescriptor
    {
        public string ClassName { get; set; }
        public IList<PropertyDescriptor> properties { get; set; }
    }
}
