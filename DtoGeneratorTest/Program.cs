using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DtoGenerator.CodeGenerators;
using DtoGenerator.DeserializedData;
using DtoGenerator.Parsers;


namespace DtoGeneratorTest
{
    static class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText("test.json");
            ClassList classList = new JsonParser().ParseClassList(json);
            List<DtoGenerator.GeneratingClassUnit> classes =  
                new DtoGenerator.DtoGenerator(classList, new CodeDomGenerator()).GenerateClasses();

            foreach (var generatedClass in classes)
            {
                using (FileStream fs = File.Open(generatedClass.ClassDescription.ClassName + ".cs", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(generatedClass.Code);
                    }
                }
                
            }
            Console.ReadLine();
        }

        private static string CSharpName(this Type type)
        {
            var sb = new StringBuilder();
            var name = type.FullName;
            if (!type.IsGenericType) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                                            .Select(t => t.CSharpName())));
            sb.Append(">");
            return sb.ToString();
        }

        
    }
}
