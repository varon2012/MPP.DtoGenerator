using System;
// ReSharper disable All

namespace FromJsonToCsFilesDtoGenerator
{
    class Runner
    {
        private const int ArgumentsCount = 4;

        public static void Main(string[] args)
        {
            if (args.Length == ArgumentsCount)
            {
                string jsonFilePath = args[0];
                string outputDirectoryPath = args[1];
                string namespaceName = args[2];
                int maxTaskCount = 1;
                if (!Int32.TryParse(args[3], out maxTaskCount))
                {
                    Console.Error.WriteLine("Error: max task count parse error");
                    return;
                }
                DtoGenerator.DtoGenerator generator = null;
                try
                {
                    JsonDtoInfoListReader reader = new JsonDtoInfoListReader(jsonFilePath);
                    CsFileDtoDeclarationWriter writer = new CsFileDtoDeclarationWriter(outputDirectoryPath);
                    generator = new DtoGenerator.DtoGenerator(maxTaskCount, namespaceName, reader,
                        writer);
                    generator.GenerateDtoDeclarations();
                    Console.WriteLine("DTO's successfully generated");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Error: {e.Message}");
                }
                finally
                {
                    generator?.Dispose();
                }

            }
            else
            {
                Console.Error.WriteLine("Error: invalid argument count");
            }
        }
    }
}