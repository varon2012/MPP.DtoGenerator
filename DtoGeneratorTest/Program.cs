using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DtoGenerator.CodeGenerators;
using DtoGenerator.DeserializedData;
using DtoGenerator.Parsers;
using DtoGeneratorTest.Writers;


namespace DtoGeneratorTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText("test.json");
            ClassList classList = new JsonParser().ParseClassList(json);


            List<DtoGenerator.GeneratingClassUnit> classes =  
                new DtoGenerator.DtoGenerator(classList, new RoslynCodeGenerator()).GenerateClasses();


            new FileWriter().Write(classes, "./GeneratedClasses");

            Console.ReadLine();
        }
    }
}
