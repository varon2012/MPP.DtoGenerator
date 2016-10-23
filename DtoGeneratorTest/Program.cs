using DtoGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGeneratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] configInformation = GetConfigInformation();
                DtoGenerator.DtoGenerator dtoGenerator = new DtoGenerator.DtoGenerator(Int32.Parse(configInformation[0]), configInformation[1]);
                JsonParser jsonParser = new JsonParser();
                string jsonFile = GetJSONFile();
                Dictionary<string, List<StringBuilder>> resultClasses = dtoGenerator.GenerateClasses(jsonParser.Parse(jsonFile));
                SaveCSFiles(resultClasses);
                Console.ReadKey();
            }
            catch (FormatException e)
            {
                Console.WriteLine("Check your configuration file, please.");
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("There are no such type, which has described in JSON file.");
            }
        }

        private static string[] GetConfigInformation()
        {
            return File.ReadAllLines("project.config");
        }

        private static string GetJSONFile()
        {
            string jsonFilePath = GetPath("JSON file", true);
            return File.ReadAllText(jsonFilePath);
        }

        private static void SaveCSFiles(Dictionary<string,List<StringBuilder>> resultClasses)
        {
            string saveDirectory = GetPath("directory to save generated .cs files", false);
            foreach (var className in resultClasses.Keys)
                foreach (var unit in resultClasses[className])
                    File.WriteAllText(Path.Combine(saveDirectory, string.Concat(className,".cs")), unit.ToString());
            Console.WriteLine("Classes have been saved.");
        }

        private static string GetPath(string destination, bool isFile)
        {
            Console.WriteLine(string.Format("Enter the path to {0}:", destination));
            string path = Console.ReadLine();
            if (isFile)
            {
                while (!File.Exists(path))
                    path = RepeatPathInput();
            }
            else
            {
                while (!Directory.Exists(path))
                    path = RepeatPathInput();
            }

            return path;
        }

        private static string RepeatPathInput()
        {
            Console.WriteLine("Wrong path :( Try to enter the path again.");
            return Console.ReadLine();
        }

    }
}
