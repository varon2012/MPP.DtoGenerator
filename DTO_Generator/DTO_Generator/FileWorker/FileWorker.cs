using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator;

namespace DTO_Generator.FileWorker
{
    class FileWorker : IReader, IWriter
    {
        public string ReadFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            return File.ReadAllText(fileName);
        }

        public void WriteFile(string outputPath, List<GeneratedClass> classes)
        {
            if (outputPath == null)
                throw new ArgumentNullException(nameof(outputPath));

            CreateDirectory(outputPath);

            foreach (var generatedClass in classes)
            {
                if ( !((generatedClass.ClassName == null) || (generatedClass.Code == null)) )
                {
                    string target = Path.Combine(outputPath, generatedClass.ClassName + ".cs");

                    using (var fileStream = File.Create(target))
                    {
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.Write(generatedClass.Code);
                        }
                    }
                }
            }
        }

        private void CreateDirectory(string outputPath)
        {
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
        }
    }
}
