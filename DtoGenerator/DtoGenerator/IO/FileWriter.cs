using System;
using System.IO;
using System.Text;

namespace DtoGenerator.IO
{
    internal sealed class FileWriter : IClassWriter
    {
        private readonly string _directory;

        public FileWriter(string directory)
        {
            if (directory == null) throw new ArgumentNullException(nameof(directory));

            _directory = directory;
            CreateDirectoryIfNotExists(directory);
        }

        public void Write(string className, string classCode)
        {
            if (className == null) throw new ArgumentNullException(nameof(className));
            if (classCode == null) throw new ArgumentNullException(nameof(classCode));

            using (var stream = File.Open(Path.Combine(_directory, $"{className}.cs"), FileMode.Create))
            {
                using (var fileWriter = new StreamWriter(stream, Encoding.UTF8))
                {
                    fileWriter.WriteLine(classCode);
                }
            }
        }

        private static void CreateDirectoryIfNotExists(string path)
        {
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
        }
    }
}
