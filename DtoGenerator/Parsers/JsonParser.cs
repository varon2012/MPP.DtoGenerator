using DtoGenerator.DeserializedData;
using Newtonsoft.Json;

namespace DtoGenerator.Parsers
{
    public class JsonParser : IParser
    {
        public ClassList ParseClassList(string classDescriptions)
        {
            return JsonConvert.DeserializeObject<ClassList>(classDescriptions);
        }
    }
}
