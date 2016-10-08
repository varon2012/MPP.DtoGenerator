using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoGenerator.DtoDescriptors;
using DtoGenerator.DtoDescriptor;
using System.Reflection;

namespace DtoGenerator.CodeGenerators
{
    class CSCodeGenerator : ICodeGenerator
    {
        public CodeCompileUnit generateCode(ClassDescription classDescription, string classNamespace)
        {
            CodeCompileUnit targetUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace(classNamespace);
            CodeTypeDeclaration targetClass = new CodeTypeDeclaration(classDescription.ClassName);
            targetClass.IsClass = true;
            targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;

            targetUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(targetClass);

            foreach(Property property in classDescription.Properties)
            {
                CodeMemberProperty codeProperty = new CodeMemberProperty();
                codeProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                codeProperty.Name = property.Name;
                codeProperty.Type = new CodeTypeReference(property.Type);
                codeProperty.HasGet = true;
                codeProperty.HasSet = true;

                targetClass.Members.Add(codeProperty);
            }

            return targetUnit;
        }
    }
}
