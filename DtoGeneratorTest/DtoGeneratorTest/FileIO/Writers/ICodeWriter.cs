using DtoGenerator.CodeGenerators.GeneratedItems;

namespace DtoGeneratorTest.FileIO
{
    internal interface ICodeWriter
    {
        string CreateSourceFile(GeneratedClass generatedClass);
    }
}
