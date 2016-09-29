using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGeneratorTest.Readers
{
    class FileReader : IReader
    {
        private readonly string filename;
        public FileReader(string filename)
        {
            this.filename = filename;
        }

        public string Read()
        {
            return File.ReadAllText(filename);
        }
    }
}
