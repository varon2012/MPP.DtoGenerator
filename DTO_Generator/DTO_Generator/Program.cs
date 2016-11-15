using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator;
using Generator.CodeGenerators;
using Generator.Descriptions;

namespace DTO_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
      /*      if (args.Length < 2)
            {
                Console.WriteLine("Enter path to input file and path to output directory");
                Console.ReadKey();
                return;
            }
            */
            string input = @"e:\json.json";
            string output = @"e:\result";
            Generate(input, output);
            // Generate(args[0], args[1]);

            if (Logger.GetInstance().ExceptionList == null)
                Console.WriteLine("Genereration finished without problems");
            else
            {
                PrintLog();
                Console.WriteLine("end");
            }
            Console.ReadKey();
        }

        private static void Generate(string inputPath, string outputPath)
        {
            var fileWorker = new FileWorker.FileWorker();

            try
            {
                var text = fileWorker.ReadFile(inputPath);
                ClassesList classes = new JsonParser.JsonParser().ParseClassDescriptions(text);
                var generator = new DtoGenerator(classes, new CodeDomGenerator(), new Configuration());
                List<GeneratedClass> generatedClasses = generator.GenerateClasses();
                fileWorker.WriteFile(outputPath, generatedClasses);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Log(ex.Message);
            }
        }

        private static void PrintLog()
        {
            var exceptionList = Logger.GetInstance().ExceptionList;
            foreach (var exception in exceptionList)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
