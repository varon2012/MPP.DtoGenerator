using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DTOGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            string JSONFilePath = GetJSONFilePathFromConsole();
            try
            {
                JSONFileStructure DTOClassesDecription = JSONParser.ParseJSONFile(JSONFilePath);
            }
            catch(Exception)
            {
                Console.WriteLine("Error reading JSON file");
            }

            Console.ReadLine();
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

        

    }
}
