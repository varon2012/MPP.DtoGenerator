using DtoGenerator.Services.ThreadPool;
using System;
using System.IO;

namespace JsonToDtoDeserializer
{
    class JsonToDtoGenerator
    {
        static void Main(string[] args)
        {
            var time = DateTime.Now;

            DtoGenerator.Services.DtoGeneratror.DtoGenerator dtoGenerator = new DtoGenerator.Services.DtoGeneratror.DtoGenerator(1, "lol");
            const string outputPath = "E:\\1";
            var classes = dtoGenerator.Generate(File.ReadAllText("E:\\Json.txt"));
            dtoGenerator.Dispose();
            foreach (var @class in classes)
            {
                File.WriteAllText(outputPath + "\\" + @class.Key + ".cs", @class.Value);
            }

            Console.WriteLine(DateTime.Now - time);
            Console.ReadKey();
        }
    }
}
