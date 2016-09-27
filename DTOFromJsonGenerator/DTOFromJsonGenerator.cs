using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DTOGeneratorLibrary;
using Newtonsoft.Json;
using static DTOFromJsonGenerator.Properties.Settings;

namespace DTOFromJsonGenerator
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    class DTOFromJsonGenerator
    {
        private const int ArgumentsCount = 2;
        private const string PluginsDirectoryPath = "plugins";

        public static void Main(string[] args)
        {
            if (args.Length == ArgumentsCount)
            {
                string jsonFilePath = args[0];
                string outputDirectoryPath = args[1];

                try
                {
                    SupportedTypesTable.Instance.LoadExternalTypes(PluginsDirectoryPath);

                    var jsonToDtoParser = new JsonToDTOInfoConverter();
                    DTOClassInfo[] dtoClassesInfo = jsonToDtoParser.ParseJsonFileToDtoClassInfo(jsonFilePath);

                    var generator = new DTOGenerator(Default.MaxRunningTasksCount, Default.NamespaceName);
                    DTOClassDeclaration[] dtoClasses = generator.GenerateDTOClasses(dtoClassesInfo);

                    WriteResults(dtoClasses, outputDirectoryPath);
                }
                catch (PluginLoadingException)
                {
                    PrintErrorMessage("Error while plugins loading");
                }
                catch (JsonException)
                {
                    PrintErrorMessage("Invalid JSON file");
                }
                catch (TypeNotRegisteredException exception)
                {
                    PrintErrorMessage($"Invalid type: {exception.Message}");
                }
            }
            else
            {
                PrintErrorMessage("Invalid number of arguments");
            }
        }

        // Static internals

        private static void WriteResults(IEnumerable<DTOClassDeclaration> dtoClassesDeclaration, string outputDirectoryPath)
        {
            Directory.CreateDirectory(outputDirectoryPath);

            foreach (DTOClassDeclaration dtoClassDeclaration in dtoClassesDeclaration)
            {
                string outputFileName = $"{dtoClassDeclaration.ClassName}.cs";
                string outputFilePath = Path.Combine(outputDirectoryPath, outputFileName);
                File.WriteAllText(outputFilePath, dtoClassDeclaration.ClassDeclaration);
            }
        }

        private static void PrintErrorMessage(string errorMessage)
        {
            Console.Error.WriteLine($"Error: {errorMessage}");
        }
    }
}
