using DtoGenerator.CodeGenerators.GeneratedItems;
using System;
using System.IO;

namespace DtoGeneratorTest.FileIO
{
    class CSFileWriter : IFileWriter
    {
        private const string CSFileExtension = ".cs";
        private const string CSBracingStyle = "C";
        private string directoryPath;

        public CSFileWriter(string directoryPath)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }
            this.directoryPath = directoryPath;
        }

        public string CreateFile(GeneratedClass generatedClass)
        {
            if(generatedClass == null)
            {
                throw new ArgumentNullException(nameof(generatedClass));
            }
            if(!Directory.Exists(directoryPath))
            {
                return null;
            }

            string className = generatedClass.ClassName;
            string outputFilePath = BuildOutputFileName(className);
            CreateCSFile(outputFilePath, generatedClass);
            return outputFilePath;
        }

        private string BuildOutputFileName(string classname)
        {
            string fileName = String.Concat(directoryPath, Path.DirectorySeparatorChar, classname, CSFileExtension);
            return fileName;
        }

        private void CreateCSFile(string filePath, GeneratedClass generatedClass)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(generatedClass.ClassCode);
                writer.Close();
            }
        }
    }

    
}
