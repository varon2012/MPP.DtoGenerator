using System.Collections.Generic;

namespace DtoGenerator.CodeGenerators
{
    public sealed class T4TemplateCodeGenerator : ICodeGenerator
    {
        public void GenerateCode(object obj)
        {
            RuntimeTextTemplateCodeGenerator template = new RuntimeTextTemplateCodeGenerator();
            GenerationClassUnit generationClassUnit = (GenerationClassUnit)obj;
            template.Session = new Dictionary<string, object>()
            {
                {"ClassDescription", generationClassUnit.ClassDescription},
                {"NamespaceName" , generationClassUnit.NamespaceName},
                {"TypeTable", generationClassUnit.TypeTable }
            };
            template.Initialize();
            string resultCode = template.TransformText();
            generationClassUnit.Code = resultCode;
        }
    }
}
