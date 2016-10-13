using DtoGenerator.Contracts.InputModels;
using Newtonsoft.Json;

namespace DtoGenerator.InputModels
{
    public class PropertyDescription: IPropertyDescription
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }
}
