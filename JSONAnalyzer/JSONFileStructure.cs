using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DTOGenerator
{
    public class JSONFileStructure
    {
        [JsonProperty("classDescriptions")]
        public ClassStructure[] ClassDescriptions { get; private set; }    
    }

    public class ClassStructure
    {
        [JsonProperty("className")]
        public string ClassName { get; private set; }

        [JsonProperty("properties")]
        public PropertyInfo[] Properties { get; private set; }
    }

    public class PropertyInfo
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("format")]
        public string Format { get; private set; }
    }
}
