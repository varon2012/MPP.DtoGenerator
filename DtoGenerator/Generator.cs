using DtoGenerator.CodeGenerators;
using DtoGenerator.DtoDescriptor;
using DtoGenerator.DtoDescriptors;
using DtoGenerator.Parser;
using System;
using System.CodeDom;

namespace DtoGenerator
{
    public class Generator
    {
        private string classesNamespace;

        public Generator(string classesNamespace)
        {
            this.classesNamespace = classesNamespace;
        }


        public GeneratedClasses GenerateDtoClasses(string jsonString)
        {
            
            if(jsonString == null)
            {
                throw new ArgumentNullException(nameof(jsonString));
            }
            if (classesNamespace == null)
            {
                throw new ArgumentNullException(nameof(classesNamespace));
            }

            ClassList list = ParseJsonString(jsonString);
            GeneratedClasses classes = GenerateClassesCode(list);

            return classes;
        }

        private ClassList ParseJsonString(string jsonString)
        {
            IParser<ClassList> parser = new ClassParser();
            return parser.Parse(jsonString);
        }

        private GeneratedClasses GenerateClassesCode(ClassList list)
        {
            ICodeGenerator codeGenerator = new CSCodeGenerator();
            GeneratedClasses classes = new GeneratedClasses();
            foreach (ClassDescription classDescription in list.classDescriptions)
            {
                CodeCompileUnit compileUnit = codeGenerator.GenerateCode(classDescription, classesNamespace);
                classes.AddClass(compileUnit);
            }

            return classes;
        }
    }
}
