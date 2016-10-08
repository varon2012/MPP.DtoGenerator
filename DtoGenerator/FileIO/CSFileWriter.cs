using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.FileIO
{
    class CSFileWriter : IFileWriter
    {
        private const string CSFileExtension = ".cs";
        private const string CSBracingStyle = "C";


        public void write(CodeCompileUnit compileUnit, string className, string filePath)
        {
            if(compileUnit == null)
            {
                throw new ArgumentNullException(nameof(compileUnit));
            }
            if(className == null)
            {
                throw new ArgumentNullException(nameof(className));
            }
            if(filePath == null)
            {
                throw new ArgumentNullException(nameof(className));
            }

            string outputFilePath = buildOutputFileName(filePath, className);
            createCSFile(outputFilePath, compileUnit);

        }

        private string buildOutputFileName(string filePath, string classname)
        {
            string fileName = String.Concat(filePath, Path.DirectorySeparatorChar, classname, CSFileExtension);
            return fileName;
        }

        private void createCSFile(string filePath, CodeCompileUnit compileUnit)
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
