using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace DTOGenerator
{
    public class DTODescription
    {
        public string ClassName { get; private set; }
        public SyntaxTree SyntaxTree { get; set; }

        public DTODescription(string className)
        {
            this.ClassName = className;
        }
    }
}
