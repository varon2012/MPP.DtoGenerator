using DtoGenerator.DtoDescriptor;
using DtoGenerator.DtoDescriptors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Parser
{
    internal class ClassParser : IParser<ClassList>
    {
        public ClassList parse(string jsonString)
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
