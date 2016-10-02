namespace DtoGenerator.CodeGenerators
{
    public interface ICodeGenerator
    {
        void GenerateCode(GenerationClassUnit generatingClass);
    }
}
