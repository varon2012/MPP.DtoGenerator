using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public sealed class DtoFieldInfo
    {
        public DtoFieldInfo(string name, DtoTypeInfo dtoType)
        {
            Name = name;
            DtoType = dtoType;
        }

        public string Name { get; }
        public DtoTypeInfo DtoType { get; }
    }
}
