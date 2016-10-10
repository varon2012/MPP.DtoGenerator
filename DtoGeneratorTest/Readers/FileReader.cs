using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGeneratorTest.Readers
{
    internal sealed class FileReader : IReader
    {
        private readonly string filename;
        internal FileReader(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }
            
            this.filename = filename;
        }

        public string Read()
        {
            return File.ReadAllText(filename);
        }
    }
}
