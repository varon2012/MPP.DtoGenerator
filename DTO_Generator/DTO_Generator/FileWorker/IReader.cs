using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Generator.FileWorker
{
    interface IReader
    {
        string ReadFile(string fileName);
    }
}
