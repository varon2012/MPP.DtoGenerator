using System.Collections.Generic;
using System.IO;
using DtoGenerationLibrary;

namespace DTOFromJsonGenerator
{
    internal class DtoClassesWriter
    {
        internal void WriteDtoClasses(IEnumerable<DtoClassDeclaration> dtoClassesDeclaration, string outputDirectoryPath)
        {
            Directory.CreateDirectory(outputDirectoryPath);

            foreach (DtoClassDeclaration dtoClassDeclaration in dtoClassesDeclaration)
            {
                string outputFileName = $"{dtoClassDeclaration.ClassName}.cs";
                string outputFilePath = Path.Combine(outputDirectoryPath, outputFileName);
                File.WriteAllText(outputFilePath, dtoClassDeclaration.ClassDeclaration);
            }
        }
    }
}
