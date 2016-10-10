using DtoGenerator.DtoDescriptors;
using Newtonsoft.Json;
using System;

namespace DtoGenerator.Parser
{
    internal class JsonStringParser : IParser<ClassDescriptionList>
    {
        public ClassDescriptionList Parse(string jsonString)
        {
            if(jsonString == null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }

            ClassDescriptionList classDescriptors = JsonConvert.DeserializeObject<ClassDescriptionList>(jsonString);
            return classDescriptors;
        }
    }
}
