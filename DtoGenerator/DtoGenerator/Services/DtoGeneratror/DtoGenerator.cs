using DtoGenerator.Contracts.Services.DtoGenerator;
using DtoGenerator.InputModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DtoGenerator.Services.DtoGeneratror
{
    public class DtoGenerator : IDtoGenerator
    {
        public void Generate(string filePath, string outputDirectoryPath)
        {
            ClassDescriptions classDescriptions = JsonConvert.DeserializeObject<ClassDescriptions>(
                File.ReadAllText(filePath));
        }
    }
}
