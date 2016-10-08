using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.FileReaders
{
    internal class JsonFileReader : IFileReader
    {
        public string readFile(string filePath)
        {
            if(filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if(!File.Exists(filePath) || !Path.GetExtension(filePath).Equals(".json"))
            {
                return null;
            }

            return System.IO.File.ReadAllText(filePath);
        }
    }
}
