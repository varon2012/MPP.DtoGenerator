using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator.Descriptions;

namespace Generator.CodeGenerators
{
    public interface ICodeGenerator
    {
        void GenerateCode(ClassTemplate template);
    }
}
