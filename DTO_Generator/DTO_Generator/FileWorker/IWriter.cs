using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator;

namespace DTO_Generator.FileWorker
{
    interface IWriter
    {
        void WriteFile(string outputPath, List<GeneratedClass> text);
    }
}
