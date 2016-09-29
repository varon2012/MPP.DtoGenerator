using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGeneratorTest.Writers
{
    public interface IClassWriter
    {
        void Write(List<DtoGenerator.GeneratingClassUnit> classes, string directory);
    }
}
