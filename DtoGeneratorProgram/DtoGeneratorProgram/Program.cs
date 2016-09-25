using System;
using System.IO;
using System.Web.Script.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using DtoGenerator;
using DtoGenerator.Descriptors;

namespace DtoGeneratorProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "samples.json";
            string taskCountPath = "taskCount.txt";
        
         
            if ( (File.Exists(path)) && (File.Exists(taskCountPath)) )
            {
                string json = File.ReadAllText(path);
                int countOfThreads = Int32.Parse(File.ReadAllText(taskCountPath));

                JavaScriptSerializer ser = new JavaScriptSerializer();
                DescriptionsOfClass classes = ser.Deserialize<DescriptionsOfClass>(json);
                Console.WriteLine(classes.classDescriptions.Count);

                DTOGenerator tempDto = new DTOGenerator(classes, new List<TypeDescription.TypeDescriptor>(), countOfThreads);
                Dictionary<string, CodeCompileUnit> temp = tempDto.GetUnitsOfDtoClasses();

                foreach(var unit in temp)
                {
                    SaveCode(unit.Key, unit.Value);
                }
            }

            
            Console.ReadLine();
        }

        public static void SaveCode(string className, CodeCompileUnit compileunit)
        {
            // Build the source file name with the appropriate
            // language extension.
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            String sourceFile = className + ".cs";

            // Create an IndentedTextWriter, constructed with
            // a StreamWriter to the source file.
            IndentedTextWriter codeWriter = new IndentedTextWriter(new StreamWriter(sourceFile, false), "    ");
            // Generate source code using the code generator.
            provider.GenerateCodeFromCompileUnit(compileunit, codeWriter, new CodeGeneratorOptions());
            // Close the output file.
            codeWriter.Close();
        }
    }
}
