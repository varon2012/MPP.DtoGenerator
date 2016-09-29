using System.Collections.Generic;
using DtoGenerator.DeserializedData;

namespace DtoGenerator.CodeGenerators
{
    public class T4TemplateCodeGenerator : ICodeGenerator
    {
        public void GenerateCode(object obj)
        {
            RuntimeTextTemplateCodeGenerator template = new RuntimeTextTemplateCodeGenerator();
            GeneratingClassUnit generatingClassUnit = (GeneratingClassUnit)obj;
            template.Session = new Dictionary<string, object>()
            {
                {"ClassDescription", generatingClassUnit.ClassDescription},
                {"NamespaceName" , generatingClassUnit.NamespaceName},
                {"TypeTable", generatingClassUnit.TypeTable }
            };
            template.Initialize();
            string resultCode = template.TransformText();
            generatingClassUnit.Code = resultCode;
        }
    }
}
