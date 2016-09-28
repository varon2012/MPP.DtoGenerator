﻿using System;
using System.IO;
using System.Web.Script.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Configuration;

using DtoGenerator.Descriptors;
using TypeDescription;

namespace DtoGeneratorProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "samples.json";
            string directory = "./MadeClasses/";
            string plugins = "./TypePlugins/";

            int threadCount;
            string Namespace;
            List<Exception> Errors = null;

            try
            {
                if (File.Exists(path))
                {
                    FinderTypes dog = new FinderTypes(plugins);
                    List<TypeDescriptor> pluginTypes = dog.FindPlugins();

                    threadCount = Int32.Parse(ConfigurationManager.AppSettings["thread_count"]);
                    Namespace = ConfigurationManager.AppSettings["namespace"];

                    string json = File.ReadAllText(path);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    DescriptionsOfClass classes = ser.Deserialize<DescriptionsOfClass>(json);
                    classes.Namespace = Namespace;

                    Console.WriteLine("Find classes = " + classes.classDescriptions.Count);

                    DtoGenerator.DtoGenerator tempDto = new DtoGenerator.DtoGenerator(classes, pluginTypes, threadCount);
                    Dictionary<string, CodeCompileUnit> temp = tempDto.GetUnitsOfDtoClasses(out Errors);

                    if (Errors != null)
                    {
                        throw new InvalidDataException();
                    }
                    foreach (var unit in temp)
                    {
                        ClassDescriptioSaver.SaveCode(directory + unit.Key + ".cs", unit.Value);
                    }
                }
                Console.WriteLine("Work done");
                Console.ReadLine();
            }
            catch(InvalidDataException error)
            {
                foreach(var exception in Errors)
                {
                    Console.WriteLine(exception.Message);
                }
                Console.ReadLine();
            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
                Console.ReadLine();
            }

            
        }
    }
}
