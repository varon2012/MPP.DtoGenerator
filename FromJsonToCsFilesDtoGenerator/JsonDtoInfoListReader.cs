using System;
using System.Collections.Generic;
using System.IO;
using DtoGenerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FromJsonToCsFilesDtoGenerator
{
    public class JsonDtoInfoListReader : IDtoInfoListReader
    {
        public string FilePath { get; set; }
        
        public JsonDtoInfoListReader(string path)
        {
            FilePath = path;
        }
        
        public event Action<DtoInfo> OnDtoInfoRead;
        public event Action OnReadCompleted;

        public void ReadList()
        {
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                while (reader.TokenType != JsonToken.StartArray)
                    reader.Read();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        
                        dynamic obj = JObject.Load(reader);

                   /*     string className = obj["className"];
                        List<DtoFieldInfo> fields = new List<DtoFieldInfo>();
                        foreach (var property in obj.properties)
                        {
                            DtoFieldInfo fieldInfo = new DtoFieldInfo(property.name.ToString(), 
                                DtoGeneratorTypesTable.Instance.GetDotNetType());    
                        }

                    }*/
                }

            }


        }
    }
}
