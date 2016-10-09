using System;
using System.Collections.Generic;

using DtoGenerator.CodeGenerators;
using DtoGenerator.DeserializedData;
using DtoGenerator.Parsers;
using DtoGeneratorTest.Readers;
using DtoGeneratorTest.Writers;


namespace DtoGeneratorTest
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please, enter name of output directory and the file with input data");
                return;
            }

            GenerateClasses(args[0], args[1]);
            Console.WriteLine("The good has happened.");
            Console.ReadLine();
        }

        private static void GenerateClasses(string filename, string outputDir)
        {
            try
            {
                string json = new FileReader(filename).Read();

                ClassList classList = new JsonParser().ParseClassList(json);

                using (var generator = new DtoGenerator.DtoGenerator(classList, new T4TemplateCodeGenerator(), new ConfigData()))
                {
                    List<DtoGenerator.GenerationResult> classes = generator.GenerateClasses();
                    new FileWriter().Write(classes, outputDir);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}
