using System.Collections.Generic;
using DtoGenerator.Contracts.InputModels;
using Newtonsoft.Json;

namespace DtoGenerator.InputModels
{
    public class ClassDescriptions : IClassDescriptions
    {
        [JsonProperty("classDescriptions")]
        public IEnumerable<IClassDescription> Classes { get; set; }

        public ClassDescriptions(ClassDescription[] classDescriptions)
        {
            Classes = classDescriptions;
        }
    }
}
