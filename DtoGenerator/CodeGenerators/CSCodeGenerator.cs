using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator.DtoDescriptors;
using DtoGenerator.DtoDescriptor;
using System.Reflection;
using DtoGenerator.CodeGenerators.Types;

namespace DtoGenerator.CodeGenerators
{
    class CSCodeGenerator : ICodeGenerator
    {
        private SupportedTypesTable supportedTypes = new SupportedTypesTable();

        public CodeCompileUnit generateCode(ClassDescription classDescription, string classNamespace)
        {
            CodeCompileUnit targetUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace(classNamespace);
            CodeTypeDeclaration targetClass = generateClass(classDescription.ClassName, classDescription.Properties);

            targetUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(targetClass);

            return targetUnit;
        }

        private CodeTypeDeclaration generateClass(string name, Property[] properties)
        {
            CodeTypeDeclaration targetClass = new CodeTypeDeclaration(name);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;

            foreach (Property property in properties)
            {
                string propertyType = supportedTypes.getNetType(property.Type, property.Format);
                CodeMemberProperty codeProperty = generateProperty(property.Name, propertyType);
                targetClass.Members.Add(codeProperty);
            }

            return targetClass;
        }

        private CodeMemberProperty generateProperty(string name, string type)
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
