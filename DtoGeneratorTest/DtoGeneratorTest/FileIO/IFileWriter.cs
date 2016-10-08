using System.CodeDom;

namespace DtoGeneratorTest.FileIO
{
    interface IFileWriter
    {
        void Write(CodeCompileUnit compileUnit);
    }
}
