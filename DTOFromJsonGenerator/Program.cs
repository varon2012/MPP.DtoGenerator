using System;
using System.Linq;
using DTOGeneratorLibrary;

namespace DTOFromJsonGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            DTOGenerator generator = new DTOGenerator(5);

            //DTOPropertyInfo[] properties =
            //{
            //    new DTOPropertyInfo {Name = "A", PropertyType = typeof(int)},
            //    new DTOPropertyInfo {Name = "B", PropertyType = typeof(string)}
            //};
            //DTOClassInfo classInfo = new DTOClassInfo {Name = "Foo", Properties = properties};

            JsonToDTOParser jsonToDtoParser = new JsonToDTOParser();
            foreach (var s in generator.GenerateDTOClasses(jsonToDtoParser.ParseJsonFileToDtoClassInfo("test.json")))
            {
                Console.WriteLine(s);
            }
            
        }
    }
}
