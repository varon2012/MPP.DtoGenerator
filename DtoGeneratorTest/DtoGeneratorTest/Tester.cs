using DtoGenerator;
using DtoGenerator.CodeGenerators.GeneratedItems;
using DtoGeneratorTest.FileIO;
using DtoGeneratorTest.FileReaders;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DtoGeneratorTest
{
    class Tester
    {
        private static string classesNamespace;
        private static int maxThreadNumber;

        static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Wrong number of arguments. You should specify 2 arguments.");
                Console.WriteLine("1st argument - json file path. 2nd argument - destination directory path.");
                Console.ReadLine();
                return;
            }

            string jsonFilePath = args[0];
            string directoryPath = args[1];

            classesNamespace = ConfigurationManager.AppSettings["generatedClassesNamespace"];
            maxThreadNumber = Int32.Parse(ConfigurationManager.AppSettings["maxThreadNumber"]);

            Tester tester = new Tester();
            tester.test(jsonFilePath, directoryPath);
            Console.ReadLine();
        }

        private void test(string jsonFilePath, string directoryPath)
        {
            string jsonString = readJsonFile(jsonFilePath);
            if(jsonString == null)
            {
                Console.WriteLine("Incorrect json file path.");
                return;
            }

            CodeGenerator generator = new CodeGenerator(classesNamespace, maxThreadNumber);
            GeneratedClasses classes = generator.GenerateDtoClasses(jsonString);
            IFileWriter writer = new CSFileWriter(directoryPath);
            IEnumerator<GeneratedClass> enumerator = classes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GeneratedClass generatedClass = enumerator.Current;
                String filePath = writer.CreateFile(generatedClass);
                if(filePath != null)
                {
                    Console.WriteLine("Created file " + filePath);
                }
                else
                {
                    Console.WriteLine("Invalid directory " + directoryPath);
                    return;
                }
            }
        }

        private string readJsonFile(string filePath)
        {
            IFileReader reader = new JsonFileReader();
            return reader.ReadFile(filePath);
        }
    }
}
