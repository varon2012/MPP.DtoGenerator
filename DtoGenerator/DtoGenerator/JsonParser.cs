using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DtoGenerator
{
    public class JsonParser
    {
        public ClassDescriptionList Parse (string jsonFile)
        {
            if (jsonFile == null)
                throw new ArgumentNullException(nameof(jsonFile));
            
            return JsonConvert.DeserializeObject<ClassDescriptionList>(jsonFile);
        }
    }
}
