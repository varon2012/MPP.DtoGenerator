using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using DtoGenerator.DeserializedData;
using DtoGenerator.Plugins;

namespace DtoGenerator.CodeGenerators
{
    public class CodeDomGenerator : ICodeGenerator
    {
        public void GenerateCode(object obj)
        {
            GeneratingClassUnit generatingClassUnit = (GeneratingClassUnit)obj;
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace(generatingClassUnit.NamespaceName);
            CodeTypeDeclaration generatingClass = new CodeTypeDeclaration(generatingClassUnit.ClassDescription.ClassName);
            
            compileUnit.Namespaces.Add(codeNamespace);
            codeNamespace.Types.Add(generatingClass);

            foreach (PropertyDescription property in generatingClassUnit.ClassDescription.Properties)
            {
                string privateFieldName = "_" + property.Name;

                Type type = GetCSharpType(generatingClassUnit.TypeTable, property.Type, property.Format);

                generatingClass.Members.Add(CreatePrivateFieldForProperty(property, privateFieldName, type));
                generatingClass.Members.Add(CreatePublicPropertyByField(property, privateFieldName, type));
            }
            generatingClassUnit.Code = GetGeneratedClass(compileUnit);
        }

        private string GetGeneratedClass(CodeCompileUnit compileUnit)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter sw = new StringWriter(stringBuilder))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, sw, options);
            }

            return stringBuilder.ToString();
        }

        private CodeMemberField CreatePrivateFieldForProperty(PropertyDescription property, string fieldName, Type type)
        {
            CodeMemberField field = new CodeMemberField()
            {
                Name = fieldName,
                Type = new CodeTypeReference(type),
                Attributes = MemberAttributes.Private
            };

            return field;
        }

        private CodeMemberProperty CreatePublicPropertyByField(PropertyDescription property, string fieldName, Type type)
        {
            CodeMemberProperty codeMemberProperty = new CodeMemberProperty()
            {
                Name = property.Name,
                Type = new CodeTypeReference(type),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            codeMemberProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), fieldName)));
            codeMemberProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), fieldName),
                    new CodePropertySetValueReferenceExpression()));

            return codeMemberProperty;
        }

        private Type GetCSharpType(TypeTable typeTable, string type, string format)
        {
            return typeTable.GetCSharpTypeByFormatAndType(type, format);
        }
    }
}
