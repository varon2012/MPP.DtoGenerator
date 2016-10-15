using DtoGenerator.CodeGenerators.GeneratedItems;

namespace DtoGenerator.CodeGenerators
{
    internal interface ICodeGenerator
    {
        void GenerateCode(object threadContext);
    }
}
