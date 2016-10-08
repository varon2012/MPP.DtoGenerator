using System.CodeDom;
using DtoGenerator.DtoDescriptor;
using System.Reflection;
using DtoGenerator.CodeGenerators.Types;

namespace DtoGenerator.CodeGenerators
{
    class CSCodeGenerator : ICodeGenerator
    {
        private SupportedTypesTable supportedTypes = new SupportedTypesTable();

        public CodeCompileUnit GenerateCode(ClassDescription classDescription, string classNamespace)
        {
            CodeCompileUnit targetUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace(classNamespace);
            CodeTypeDeclaration targetClass = GenerateClass(classDescription.ClassName, classDescription.Properties);

            targetUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(targetClass);

            return targetUnit;
        }

        private CodeTypeDeclaration GenerateClass(string name, Property[] properties)
        {
            CodeTypeDeclaration targetClass = new CodeTypeDeclaration(name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;

            foreach (Property property in properties)
            {
                string propertyType = supportedTypes.GetNetType(property.Type, property.Format);
                CodeMemberProperty codeProperty = GenerateProperty(property.Name, propertyType);
                targetClass.Members.Add(codeProperty);
            }

            return targetClass;
        }

        private CodeMemberProperty GenerateProperty(string name, string type)
        {
            CodeMemberProperty property = new CodeMemberProperty();
            property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            property.Name = name;
            property.Type = new CodeTypeReference(type);
            property.HasGet = true;
            property.HasSet = true;

            return property;
        }

    }
}
