using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class ClassDescription
    {
        public string ClassName { get; set; }
        public List<Property> Properties { get; set; } = new List<Property>();
    }
}
