using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator;

namespace FromJsonToCsFilesDtoGenerator
{
    class Runner
    {
        private const int ArgumentsCount = 2;

        public static void Main(string[] args)
        {
            if (args.Length == ArgumentsCount)
            {
                string jsonFilePath = args[0];
                string outputDirectoryPath = args[1];

                try
                {
                    JsonDtoInfoListReader reader = new JsonDtoInfoListReader(jsonFilePath);
                    CsFileDtoDeclarationWriter writer = new CsFileDtoDeclarationWriter(outputDirectoryPath);

                    DtoGenerator.DtoGenerator generator = new DtoGenerator.DtoGenerator(3, "dtos", reader, writer);
                    generator.GenerateDtoDeclarations();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Error: {e.Message}");
                }

            }
            else
            {
                Console.Error.WriteLine("Error: invalid argument count");
            }
            Console.Read();
        }
    }
}