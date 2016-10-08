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
        ClassStructure[] classDescriptions;        
    }

    public class ClassStructure
    {
        [JsonProperty("className")]
        string className;

        [JsonProperty("properties")]
        PropertyInfo[] properties;
    }

    public class PropertyInfo
    {
        [JsonProperty("name")]
        string name;

        [JsonProperty("type")]
        string type;

        [JsonProperty("format")]
        string format;
    }
}
