using System;
using DtoGenerator.Generator;
using DtoGenerator.IO;
using static System.Console;

namespace DtoGenerator
{
    internal sealed class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Error.WriteLine("Usage: <path-to-json-file> <path-to-output-directory>");
                return;
            }

            var filename = args[0];
            var outputPath = args[1];

            try
            {
                var app = new App(new JsonParser<DtoClassDescription>(), filename, outputPath);
                app.Process();
            }
            catch (BadInputException e)
            {
                Error.WriteLine($"Error occurred during working with supplied data: {e.Message}");
            }
            catch (Exception e)
            {
                Error.WriteLine($"Unknown error occurred: {e.Message}");
            }
        }
    }
}
