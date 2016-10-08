using DtoGenerator.DtoDescriptor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Parser
{
    class ClassDescroptorParser : IParser<ClassDescriptor>
    {
        public IList<ClassDescriptor> parse(string jsonString)
        {
            if(jsonString == null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }

            IList<ClassDescriptor> classDescriptors = JsonConvert.DeserializeObject<List<ClassDescriptor>>(jsonString);
            return classDescriptors;
        }
    }
}
