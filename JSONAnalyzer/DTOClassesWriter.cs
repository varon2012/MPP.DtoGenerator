using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using System.IO;
using DTOGenerator;

namespace JSONAnalyzer
{
    class DTOClassesWriter
    {
        public static void WriteToFile(DTODescription dtoDescription, string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
            string fileName = dtoDescription.ClassName + ".cs";
            string filePath = Path.Combine(directoryPath, fileName);
            File.WriteAllText(filePath, dtoDescription.SyntaxTree.ToString());              
        }

    }
}
