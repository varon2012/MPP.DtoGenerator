using DtoGenerator.CodeGenerators.GeneratedItems;
using System.CodeDom;

namespace DtoGeneratorTest.FileIO
{
    interface IFileWriter
    {
        void Write(GeneratedClass generatedClass);
    }
}
