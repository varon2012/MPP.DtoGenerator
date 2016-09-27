using System;
using System.Configuration;
using DTOGeneratorLibrary;

namespace DTOFromJsonGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            DTOGenerator generator = new DTOGenerator(Properties.Settings.Default.MaxRunningTasksCount, Properties.Settings.Default.NamespaceName);
            JsonToDTOParser jsonToDtoParser = new JsonToDTOParser();
            foreach (var s in generator.GenerateDTOClasses(jsonToDtoParser.ParseJsonFileToDtoClassInfo("test.json")))
            {
                Console.WriteLine(s);
            }
            
        }
    }
}
