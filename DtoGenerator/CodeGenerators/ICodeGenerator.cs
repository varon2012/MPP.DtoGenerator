using DtoGenerator.DtoDescriptor;
using System.CodeDom;

namespace DtoGenerator.CodeGenerators
{
    interface ICodeGenerator
    {
        CodeCompileUnit generateCode(ClassDescription classDescription, string classNamespace);
    }
}
