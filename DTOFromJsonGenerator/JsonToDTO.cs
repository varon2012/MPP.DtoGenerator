using System.Linq;
using Newtonsoft.Json;
using System.IO;
using DTOGeneratorLibrary;

namespace DTOFromJsonGenerator
{
    internal class JsonClassDescription
    {
        public string ClassName { get; set; }
        public JsonPropertyDescription[] Properties { get; set; }

        internal DTOClassInfo ToDTOClassInfo()
        {
            return new DTOClassInfo
            {
                Name = ClassName,
                Properties = Properties.Select(x => x.ToDTOPropertyInfo()).ToArray()
            };
        }
    }

    internal class JsonPropertyDescription
    {
        public string Name { get; set; }
        public TypeKind Type { get; set; }
        public string Format { get; set; }

        internal DTOPropertyInfo ToDTOPropertyInfo()
        {
            return new DTOPropertyInfo
            {
                Name = Name,
                PropertyType = SupportedTypesTable.Instance.GetNetType(Type, Format)
            };
        }
    }

    internal class JsonClassDescriptions
    {
        public JsonClassDescription[] ClassDescriptions { get; set; }
    }

    internal class JsonToDTOParser
    {
        public DTOClassInfo[] ParseJsonFileToDtoClassInfo(string path)
        {
            return JsonStringToDtoClassInfo(File.ReadAllText(path));
        }

        // Internals

        private DTOClassInfo[] JsonStringToDtoClassInfo(string jsonString)
        {
            JsonClassDescriptions jsonClassDescriptions = JsonConvert.DeserializeObject<JsonClassDescriptions>(jsonString);
            DTOClassInfo[] result = new DTOClassInfo[jsonClassDescriptions.ClassDescriptions.Length];

            for (int i = 0; i < result.Length; i++)
            {
                JsonClassDescription classDescription = jsonClassDescriptions.ClassDescriptions[i];
                result[i] = classDescription.ToDTOClassInfo();
            }

            return result;
        }
    }
}
