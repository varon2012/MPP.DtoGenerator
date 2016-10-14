using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using JSONAnalyzer;

namespace DTOGenerator
{
    class Program
    {
            
        static void Main(string[] args)
        {
            string JSONFilePath = GetJSONFilePathFromConsole();

            try
            {
                JSONFileStructure jsonFileStructure = JSONParser.ParseJSONFile(JSONFilePath);
                try
                {
                    List<ClassDescription> dtoClassDescriptions = JSONFileStructureAdapter.AdaptToClassDecriptionList(jsonFileStructure);
                    string directoryPath = GetOutputDirectoryFromConsole();
                    List<DTODescription> dtoDecriptions = (new DTOGenerator(SettingsManager.GetThreadPoolLimit(),SettingsManager.GetDtoNamespace())).GenerateCode(dtoClassDescriptions);

                    foreach (DTODescription dtoDescription in dtoDecriptions)
                    {
                        DTOClassesWriter.WriteToFile(dtoDescription, directoryPath);
                    }
                    Console.WriteLine("Done!");
                    Console.ReadLine();
                }
                catch(InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }

                
            }
            catch(JsonReaderException)
            {
                Console.WriteLine("Error reading JSON file");
                Console.ReadLine();
            }

            
            
        }

        private static string GetJSONFilePathFromConsole()
        {
            string JSONFilePath = string.Empty;
            bool JSONFilePathIsCorrect = false;

            while (!JSONFilePathIsCorrect)
            {
                Console.WriteLine("Enter JSON file path:");
                JSONFilePath = Console.ReadLine();

                if (!File.Exists(JSONFilePath))
                {
                    Console.WriteLine("File doesn't exists. Try again.");
                }
                else
                {
                    if (Path.GetExtension(JSONFilePath) != ".json")
                    {
                        Console.WriteLine("File must have .json extension. Try again.");
                    }
                    else
                    {
                        JSONFilePathIsCorrect = true;
                    }
                }
            }

            return JSONFilePath;
        }

        private static string GetOutputDirectoryFromConsole()
        {
            string outputDirectory = string.Empty;
            Console.WriteLine("Enter output directory path:");
            outputDirectory = Console.ReadLine();
            return outputDirectory;
        }


    }
}
