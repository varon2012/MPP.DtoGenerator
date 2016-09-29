using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator;

namespace DtoGeneratorTest.Writers
{
    class FileWriter : IClassWriter
    {
        public void Write(List<GeneratingClassUnit> classes, string directory)
        {
            CreateDirectoryIfNotExist(directory);

            foreach (var generatedClass in classes)
            {
                string targetPath = Path.Combine(directory, generatedClass.ClassDescription.ClassName + ".cs");

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
