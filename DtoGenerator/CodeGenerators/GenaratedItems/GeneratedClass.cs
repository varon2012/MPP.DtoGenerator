using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.CodeGenerators.GeneratedItems
{
    public class GeneratedClass
    {
        public string ClassName { get; }
        public string ClassCode { get; }

        public GeneratedClass(string name, string code)
        {
            ClassName = name;
            ClassCode = code;
        }
    }
}
