using DtoGenerator.DtoDescriptors;
using Newtonsoft.Json;
using System;

namespace DtoGenerator.Parser
{
    internal class ClassParser : IParser<ClassList>
    {
        public ClassList Parse(string jsonString)
        {
            if(jsonString == null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }

            ClassList classDescriptors = JsonConvert.DeserializeObject<ClassList>(jsonString);
            return classDescriptors;
        }
    }
}
