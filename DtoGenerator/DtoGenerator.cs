using DtoGenerator.CodeGenerators;
using DtoGenerator.DtoDescriptor;
using DtoGenerator.DtoDescriptors;
using DtoGenerator.Parser;;
using System.CodeDom;

namespace DtoGenerator
{
    public class DtoGenerator
    {
        public static GeneratedClasses GenerateClasses(string jsonString, string classesNamespace)
        {
            
            IParser<ClassList> parser = new ClassParser();
            ClassList list = parser.Parse(jsonString);
            ICodeGenerator codeGenerator = new CSCodeGenerator();
            GeneratedClasses classes = new GeneratedClasses();
            foreach(ClassDescription classDescription in list.classDescriptions)
            {
                CodeCompileUnit compileUnit = codeGenerator.GenerateCode(classDescription, classesNamespace);
                classes.AddClass(compileUnit);
            }

            return classes;
        }
    }
}
