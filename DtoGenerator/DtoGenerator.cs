using DtoGenerator.DtoDescriptor;
using DtoGenerator.DtoDescriptors;
using DtoGenerator.FileReaders;
using DtoGenerator.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    public class DtoGenerator
    {
        public static void test()
        {
            IFileReader reader = new JsonFileReader();
            string jsonString = reader.readFile(@"C:\Users\Anastasia_Paramonova\Desktop\file.json");
            IParser<ClassList> parser = new ClassParser();
            ClassList list = parser.parse(jsonString);
            Console.WriteLine("done");
        }
    }
}
