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

            Tester tester = new Tester();
            tester.ReadApplicationSettings();
            tester.Test(jsonFilePath, directoryPath);
            Console.ReadLine();
        }

        private void ReadApplicationSettings()
        {
            classesNamespace = ConfigurationManager.AppSettings["generatedClassesNamespace"];
            maxThreadNumber = Int32.Parse(ConfigurationManager.AppSettings["maxThreadNumber"]);
        }

        private void Test(string jsonFilePath, string directoryPath)
        {
            string jsonString = ReadJsonFile(jsonFilePath);
            if(jsonString == null)
            {
                Console.WriteLine("Incorrect json file path.");
                return;
            }

            DtoCodeGenerator generator = new DtoCodeGenerator(classesNamespace, maxThreadNumber);
            GeneratedClassList classes = generator.GenerateDtoClasses(jsonString);
            WriteCodeToFiles(classes, directoryPath);
        }

        private string ReadJsonFile(string filePath)
        {
            IFileReader reader = new JsonFileReader();
            return reader.ReadFile(filePath);
        }

        private void WriteCodeToFiles(GeneratedClassList classes, string directoryPath)
        {
            ICodeWriter writer = new CSCodeWriter(directoryPath);
            IEnumerator<GeneratedClass> enumerator = classes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GeneratedClass generatedClass = enumerator.Current;
                String filePath = writer.CreateSourceFile(generatedClass);
                if (filePath != null)
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
    }
}
