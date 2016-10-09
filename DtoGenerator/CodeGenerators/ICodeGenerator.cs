using DtoGenerator.CodeGenerators.GeneratedItems;
using DtoGenerator.DtoDescriptor;

namespace DtoGenerator.CodeGenerators
{
    interface ICodeGenerator
    {
        GeneratedClass GenerateCode(ClassDescription classDescription, string classNamespace);
    }
}
