using System;
using System.IO;
using Generator = DtoGenerator.Services.DtoGenerators.DtoGenerator;
using static JsonToDtoDeserializer.Properties.GeneratorSettings;

namespace JsonToDtoGenerator
{
    class JsonToDtoGenerator
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
                return;

            var pathToJsonFile = args[0];
            var pathToOutputFolder = args[1];

            Generator dtoGenerator = new Generator(Default.MaximumTaskNumber, Default.NamespaceName);
            try
            {             
                dtoGenerator.LoadAdditionalTypes(Default.PluginsFolderName);
                var classes = dtoGenerator.Generate(File.ReadAllText(pathToJsonFile));
                foreach (var @class in classes)
                {
                    File.WriteAllText(string.Format(("{0}{1}{2}.cs"), pathToOutputFolder, Path.DirectorySeparatorChar, @class.Key), 
                        @class.Value);
                }

                Console.WriteLine("Generation was successful");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                dtoGenerator.Dispose();
            }

        }
    }
}
