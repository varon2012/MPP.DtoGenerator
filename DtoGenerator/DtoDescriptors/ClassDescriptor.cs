using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.DtoDescriptor
{
    internal class ClassDescriptor
    {
        internal string ClassName { get; set; }
        internal PropertyDescriptor[] properties { get; set; }
    }
}
