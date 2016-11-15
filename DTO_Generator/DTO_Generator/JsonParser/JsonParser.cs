using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator.Descriptions;
using Newtonsoft.Json;

namespace DTO_Generator.JsonParser
{
    class JsonParser : IParser
    {
        public ClassesList ParseClassDescriptions(string jsonInput)
        {
            if (jsonInput == null)
                return null;

            return JsonConvert.DeserializeObject<ClassesList>(jsonInput);
        }
    }
}
