using DtoGenerator.DtoDescriptor;
using System.CodeDom;

namespace DtoGenerator.CodeGenerators
{
    interface ICodeGenerator
    {
        CodeCompileUnit GenerateCode(ClassDescription classDescription, string classNamespace);
    }
}
