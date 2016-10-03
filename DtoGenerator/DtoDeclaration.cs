using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public sealed class DtoDeclaration
    {
        public DtoDeclaration(string className, string classFullText)
        {
            ClassName = className;
            ClassFullText = classFullText;
        }

        public string ClassName { get; }
        public string ClassFullText { get; }
    }
}