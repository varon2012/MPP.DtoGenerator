using System;
using System.IO;

namespace DtoGeneratorTest.FileReaders
{
    internal class JsonFileReader : IFileReader
    {
        private const string JsonFileExtension = ".json";

        public string ReadFile(string filePath)
        {
            if(filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if(!File.Exists(filePath) || !Path.GetExtension(filePath).Equals(JsonFileExtension))
            {
                return null;
            }

            return System.IO.File.ReadAllText(filePath);
        }
    }
}
