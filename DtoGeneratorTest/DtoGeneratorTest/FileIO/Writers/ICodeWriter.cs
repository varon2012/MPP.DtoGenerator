using DtoGenerator.CodeGenerators.GeneratedItems;

namespace DtoGeneratorTest.FileIO
{
    interface ICodeWriter
    {
        string CreateSourceFile(GeneratedClass generatedClass);
    }
}
