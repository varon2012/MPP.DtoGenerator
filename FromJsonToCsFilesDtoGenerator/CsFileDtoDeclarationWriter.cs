using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator;

namespace FromJsonToCsFilesDtoGenerator
{
    public class CsFileDtoDeclarationWriter : IDtoDeclarationWriter
    {
        public string OutputDirectoryPath { get; }
        private bool Disposed { get; set; }

        public CsFileDtoDeclarationWriter(string outputDirectoryPath)
        {
            OutputDirectoryPath = outputDirectoryPath;
            Directory.CreateDirectory(outputDirectoryPath);
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
            }
            else
            {
                throw new ObjectDisposedException(ToString());
            }

        }

        public void Write(DtoDeclaration dtoDeclaration)
        {
            string outputFileName = $"{dtoDeclaration.ClassName}.cs";
            string outputFilePath = Path.Combine(OutputDirectoryPath, outputFileName);
            File.WriteAllText(outputFilePath, dtoDeclaration.ClassFullText);
        }
    }
}
