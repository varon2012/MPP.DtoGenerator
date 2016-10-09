using DtoGenerator.CodeGenerators.GeneratedItems;

namespace DtoGeneratorTest.FileIO
{
    interface IFileWriter
    {
        string CreateFile(GeneratedClass generatedClass);
    }
}
