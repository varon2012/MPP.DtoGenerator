using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
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

        public void Write(CodeCompileUnit compileUnit)
        {
            if(compileUnit == null)
            {
                throw new ArgumentNullException(nameof(compileUnit));
            }

            string className = compileUnit.Namespaces[0].Types[0].Name;
            string outputFilePath = BuildOutputFileName(className);
            CreateCSFile(outputFilePath, compileUnit);

        }

        private string BuildOutputFileName(string classname)
        {
            string fileName = String.Concat(directoryPath, Path.DirectorySeparatorChar, classname, CSFileExtension);
            return fileName;
        }

        private void CreateCSFile(string filePath, CodeCompileUnit compileUnit)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BracingStyle = CSBracingStyle;
                options.BlankLinesBetweenMembers = false;

                IndentedTextWriter textWriter = new IndentedTextWriter(writer);
                provider.GenerateCodeFromCompileUnit(compileUnit, textWriter, options);
                textWriter.Close();
            }
        }
    }

    
}
