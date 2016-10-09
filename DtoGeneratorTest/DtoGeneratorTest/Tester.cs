using DtoGenerator;
using DtoGenerator.CodeGenerators;
using DtoGenerator.CodeGenerators.GeneratedItems;
using DtoGeneratorTest.FileIO;
using DtoGeneratorTest.FileReaders;
using System;
using System.CodeDom;
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

            Generator generator = new Generator(classesNamespace);
            GeneratedClasses classes = generator.GenerateDtoClasses(jsonString);
            IFileWriter writer = new CSFileWriter(directoryPath);
            IEnumerator<GeneratedClass> enumerator = classes.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                GeneratedClass generatedClass = enumerator.Current;
                writer.Write(generatedClass);
                i++;
            }
            Console.WriteLine("Generated " + i + " classes in directory " + directoryPath);
        }

        private string readJsonFile(string filePath)
        {
            IFileReader reader = new JsonFileReader();
            return reader.ReadFile(filePath);
        }
    }
}
