using System;
using System.IO;
using System.Text;

namespace DtoGenerator
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

            using (var stream = File.OpenWrite($"{_directory}/{className}.cs"))
            {
                using (var fileWritter = new StreamWriter(stream, Encoding.UTF8))
                {
                    fileWritter.WriteLine(classCode);
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
