using System;
using System.Collections.Generic;
using System.IO;
using DtoGenerator;

namespace DtoGeneratorTest.Writers
{
    internal sealed class FileWriter : IClassWriter
    {
        public void Write(List<GenerationResult> classes, string directory)
        {
            if (classes == null)
            {
                throw new ArgumentNullException(nameof(classes));
            }

            CreateDirectoryIfNotExist(directory);

            foreach (var generatedClass in classes)
            {
                string targetPath = Path.Combine(directory, generatedClass.ClassName + ".cs");

                using (FileStream fs = File.Open(targetPath, FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(generatedClass.Code);
                    }
                }
            }
        }

        private void CreateDirectoryIfNotExist(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
