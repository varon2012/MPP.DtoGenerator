using DtoGenerator.CodeGenerators;
using DtoGenerator.DtoDescriptor;
using DtoGenerator.DtoDescriptors;
using DtoGenerator.FileIO;
using DtoGenerator.FileReaders;
using DtoGenerator.Parser;
using System;
using System.CodeDom;
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
            ICodeGenerator codeGenerator = new CSCodeGenerator();
            IFileWriter writer = new CSSourceFileWriter();
            foreach(ClassDescription classDescription in list.classDescriptions)
            {
                CodeCompileUnit compileUnit = codeGenerator.generateCode(classDescription, "myNamespace");
                writer.write(compileUnit, classDescription.ClassName, @"C:\Users\Anastasia_Paramonova\Desktop");
            }
            
            Console.WriteLine("done");
        }
    }
}
