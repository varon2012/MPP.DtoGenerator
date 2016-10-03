using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public sealed class DtoInfo
    {
        public DtoInfo(string name, DtoFieldInfo[] fields)
        {
            Name = name;
            Fields = fields;
        }

        public DtoInfo()
        {

        }

        public string Name { get; set; }
        public DtoFieldInfo[] Fields { get; set; }
    }
}
