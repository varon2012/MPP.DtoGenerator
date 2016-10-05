using System.Collections.Generic;
using DtoGenerator.Contracts.InputModels;
using Newtonsoft.Json;

namespace DtoGenerator.InputModels
{
    public class ClassDescription: IClassDescription
    {
        [JsonProperty("className")]
        public string ClassName { get; set; }

        [JsonProperty("properties")]
        public IEnumerable<IPropertyDescription> Properties{ get; set; }

        public ClassDescription(PropertyDescription[] properties)
        {
            Properties = properties;
        }
    }
}
