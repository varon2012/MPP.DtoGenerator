using System.Linq;
using Newtonsoft.Json;
using System.IO;
using DtoGenerationLibrary;

namespace DTOFromJsonGenerator
{
    internal class JsonClassDescription
    {
        public string ClassName { get; set; }
        public JsonPropertyDescription[] Properties { get; set; }

        internal DtoClassInfo ToDtoClassInfo()
        {
            return new DtoClassInfo
            {
                Name = ClassName,
                Properties = Properties.Select(x => x.ToDtoPropertyInfo()).ToArray()
            };
        }
    }

    internal class JsonPropertyDescription
    {
        public string Name { get; set; }
        public TypeKind Type { get; set; }
        public string Format { get; set; }

        internal DtoPropertyInfo ToDtoPropertyInfo()
        {
            return new DtoPropertyInfo
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

    internal class JsonToDtoInfoConverter
    {
        internal DtoClassInfo[] ParseJsonFileToDtoClassInfo(string path)
        {
            return JsonStringToDtoClassInfo(File.ReadAllText(path));
        }

        // Internals

        private DtoClassInfo[] JsonStringToDtoClassInfo(string jsonString)
        {
            JsonClassDescriptions jsonClassDescriptions = JsonConvert.DeserializeObject<JsonClassDescriptions>(jsonString);
            var result = new DtoClassInfo[jsonClassDescriptions.ClassDescriptions.Length];

            for (int i = 0; i < result.Length; i++)
            {
                JsonClassDescription classDescription = jsonClassDescriptions.ClassDescriptions[i];
                result[i] = classDescription.ToDtoClassInfo();
            }

            return result;
        }
    }
}
