using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DTOGenerator
{
    public class JSONParser
    {
        public static JSONFileStructure ParseJSONFile(string JSONFilePath)
        {
            using (StreamReader sr = new StreamReader(JSONFilePath))
            {
                string JSONContent = sr.ReadToEnd();
                JSONFileStructure DTOClassesDesription = JsonConvert.DeserializeObject<JSONFileStructure>(JSONContent);
                return DTOClassesDesription;
            }
        }
    }

}
