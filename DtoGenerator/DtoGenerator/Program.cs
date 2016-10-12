using System;
using System.IO;
using DtoGenerator.Generator;
using DtoGenerator.IO;

namespace DtoGenerator
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <path-to-json-file> <path-to-output-directory>");
                return;
            }

            var filename = args[0];
            var outputPath = args[1];

            try
            {
                Process(new JsonParser<DtoClassDescription>(), filename, outputPath);
            }
            catch (BadInputException e)
            {
                Console.Error.WriteLine($"Error occurred during working with supplied data: {e.Message}");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unknown error occurred: {e.Message}");
            }
        }

        private static void Process(IFileParser<DtoClassDescription> parser, string filename, string outputPath)
        {
            var classes = parser.Parse(filename);

            // TODO: fetch namespace from config file
            var generator = new ClassCodeGenerator("Test", new RoslynCodeGenerator());
            var generatedClasses = generator.Generate(classes);

            IClassWriter writter = new FileWriter(outputPath);

            foreach (var className in generatedClasses.Keys)
            {
                try
                {
                    var classCode = generatedClasses[className];
                    writter.Write(className, classCode);
                }
                catch (IOException e)
                {
                    Console.Error.WriteLine($"Error occurred during writing to file: {e.Message}");
                }
            }
        }
    }
}
