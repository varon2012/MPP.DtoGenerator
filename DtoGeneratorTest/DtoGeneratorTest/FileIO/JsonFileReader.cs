using System;
using System.IO;

namespace DtoGeneratorTest.FileReaders
{
    internal class JsonFileReader : IFileReader
    {
        public string ReadFile(string filePath)
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
