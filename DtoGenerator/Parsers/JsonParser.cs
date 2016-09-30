using System;
using DtoGenerator.DeserializedData;
using Newtonsoft.Json;

namespace DtoGenerator.Parsers
{
    public class JsonParser : IParser
    {
        public ClassList ParseClassList(string classDescriptions)
        {
            if (classDescriptions == null)
            {
                throw new ArgumentNullException(nameof(classDescriptions));
            }
            return JsonConvert.DeserializeObject<ClassList>(classDescriptions);
        }
    }
}
