using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DtoGenerator
{
    internal class ClassDescriptionList
    {
        public List<ClassDescription> classDescriptions { get; set; } 
    }

    public class DtoGenerator
    {
        public void GenerateClasses(string jsonFile)
        {
            ClassDescriptionList classDescription = JsonConvert.DeserializeObject<ClassDescriptionList>(jsonFile);
            
        }
    }
}
