using DtoGenerator.CodeGenerators;
using DtoGeneratorTest.FileIO;
using DtoGeneratorTest.FileReaders;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;

namespace DtoGeneratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IFileReader reader = new JsonFileReader();
            string jsonString = reader.ReadFile(@"C:\Users\Anastasia_Paramonova\Desktop\file.json");
            string classesNamespace = ConfigurationManager.AppSettings["generatedClassesNamespace"];
            GeneratedClasses classes = DtoGenerator.DtoGenerator.GenerateClasses(jsonString, classesNamespace);
            IFileWriter writer = new CSFileWriter( @"C:\Users\Anastasia_Paramonova\Desktop");
            IEnumerator<CodeCompileUnit> enumerator = classes.GetEnumerator();
            while(enumerator.MoveNext())
            {
                 writer.Write(enumerator.Current);
            }
            Console.WriteLine("done");
        }
    }
}
