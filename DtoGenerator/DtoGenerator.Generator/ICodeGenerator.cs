using DtoGenerator.Generator.Types;

namespace DtoGenerator.Generator
{
    public interface ICodeGenerator
    {
        string Generate(string specifiedNamespace, DtoClassDescription classDescription, TypeResolver typeResolver);
    }
}
