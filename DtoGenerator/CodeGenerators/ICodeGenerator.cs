using DtoGenerator.CodeGenerators.GeneratedItems;

namespace DtoGenerator.CodeGenerators
{
    interface ICodeGenerator
    {
        void GenerateCode(object threadContext);
    }
}
