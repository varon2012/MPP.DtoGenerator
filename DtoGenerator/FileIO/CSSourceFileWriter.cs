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
    class CSSourceFileWriter : IFileWriter
    {
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

            CSharpCodeProvider provider = new CSharpCodeProvider();
            string outputFile = buildOutputFileName(filePath, className, provider.FileExtension);

            using (StreamWriter writer = new StreamWriter(outputFile, false))
            {
                IndentedTextWriter textWriter = new IndentedTextWriter(writer, "    ");
                provider.GenerateCodeFromCompileUnit(compileUnit, textWriter,
                    new CodeGeneratorOptions());
                textWriter.Close();
            }

        }

        private string buildOutputFileName(string filePath, string classname, string fileExtension)
        {
            string fileName = String.Concat(filePath, '\\', classname);
            if (fileExtension[0] == '.')
            {
                fileName = String.Concat(fileName, fileExtension);
            }
            else
            {
                fileName = String.Concat(fileName, '.', fileExtension);
            }
            return fileName;
        }
    }

    
}
