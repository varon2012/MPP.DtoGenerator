using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Generator
{
    public interface IDtoGenerator
    {
        List<GeneratedClass> GenerateClasses();
    }
}
