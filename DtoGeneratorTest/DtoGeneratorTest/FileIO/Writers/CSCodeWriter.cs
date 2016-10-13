using DtoGenerator.CodeGenerators.GeneratedItems;
using System;
using System.IO;

namespace DtoGeneratorTest.FileIO
{
    internal class CSCodeWriter : ICodeWriter
    {
        private const string CSFileExtension = ".cs";
        private const string CSBracingStyle = "C";
        private string directoryPath;
        public string DirectoryPath
        {
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(directoryPath));
                directoryPath = value;
            }
            get
            {
                return directoryPath;
            }
        }

        public CSCodeWriter(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }

        public string CreateSourceFile(GeneratedClass generatedClass)
        {
            if(generatedClass == null) throw new ArgumentNullException(nameof(generatedClass));

            if(!Directory.Exists(directoryPath))
            {
                return null;
            }

            string className = generatedClass.ClassName;
            string outputFilePath = BuildOutputFileName(className);
            WriteClassToFile(outputFilePath, generatedClass);
            return outputFilePath;
        }

        private string BuildOutputFileName(string classname)
        {
            string fileName = String.Concat(classname, CSFileExtension);
            return Path.Combine(directoryPath, fileName);
        }

        private void WriteClassToFile(string outputFilePath, GeneratedClass generatedClass)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath, false))
            {
                writer.WriteLine(generatedClass.ClassCode);
                writer.Close();
            }
        }
    }

    
}
