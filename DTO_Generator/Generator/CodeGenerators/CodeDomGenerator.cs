using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Generator.Descriptions;
using Generator.Types;
using Microsoft.CSharp;

namespace Generator.CodeGenerators
{
    public class CodeDomGenerator : ICodeGenerator
    {
        public void GenerateCode(ClassTemplate template)
        {
            try
            {
                CodeCompileUnit compileUnit = CreateCompileUnit(template);
                template.Code = GenerateCSharpCode(compileUnit);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Log(ex.Message);
            }
        }

        private CodeCompileUnit CreateCompileUnit(ClassTemplate template)
        {
            if (template.Description.Properties == null)
                throw new ArgumentNullException(nameof(template.Description.Properties));
            if (template.Description.ClassName == null)
                throw new ArgumentNullException(nameof(template.Description.ClassName));

            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace codeNamespace = new CodeNamespace(template.Namespace);
            CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration(template.Description.ClassName);

            codeTypeDeclaration.IsClass = true;
            codeTypeDeclaration.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            codeNamespace.Types.Add(codeTypeDeclaration);
            compileUnit.Namespaces.Add(codeNamespace);

            foreach (var property in template.Description.Properties)
            {
                try
                {
                    var type = GetCSharpType(property.Format, property.Type);
                    codeTypeDeclaration.Members.Add(CreatePublicProperty(property, type));
                }
                catch (Exception ex)
                {
                    Logger.GetInstance().Log(ex.Message);
                }
            }
            return compileUnit;
        }

        private CodeMemberProperty CreatePublicProperty(ClassProperty propertyDescription, Type type)
        {
            CodeMemberProperty property = new CodeMemberProperty
            {
                Type = new CodeTypeReference(type),
                HasGet = true,
                HasSet = true,
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = propertyDescription.Name
            };
            return property;
        }

        private Type GetCSharpType(string format, string type)
        {
            return TypeTable.GetInstance().GetCSharpType(format, type);
        }

        private string GenerateCSharpCode(CodeCompileUnit compileUnit)
        {
            CodeDomProvider provider = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(stringBuilder))
            {
                provider.GenerateCodeFromCompileUnit(compileUnit, stringWriter, options);
            }

            return stringBuilder.ToString();
        }
    }
}
