using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Types
{
    public class ExactType
    {
        public string Type { get; set; }
        public string Format { get; set; }
        public Type DotNetType { get; set; }
    }
}
