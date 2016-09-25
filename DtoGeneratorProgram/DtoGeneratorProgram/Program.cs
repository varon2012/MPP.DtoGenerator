using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoGeneratorProgram.Descriptors;

namespace DtoGeneratorProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "samples.json";
         
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                DescriptionsOfClass classes = ser.Deserialize<DescriptionsOfClass>(json);
                Console.WriteLine(classes.classDescriptions.Count);
            }


            Console.ReadLine();
        }
    }
}
