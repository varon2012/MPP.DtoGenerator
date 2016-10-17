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
            string jsonFile = GetFilePath();
            DtoGenerator.DtoGenerator dtoGenerator = new DtoGenerator.DtoGenerator();
            dtoGenerator.GenerateClasses(jsonFile);
            Console.ReadKey();
        }

        private static string GetFilePath()
        {
            Console.WriteLine("Enter the path to JSON file: ");
            string jsonFilePath = Console.ReadLine();
            while (!File.Exists(jsonFilePath))
            {
                Console.WriteLine("Wrong path :( Try to enter the path again.");
                jsonFilePath = Console.ReadLine();
            }

            return File.ReadAllText(jsonFilePath);
        }
    }
}
