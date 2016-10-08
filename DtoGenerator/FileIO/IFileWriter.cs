using System.CodeDom;

namespace DtoGenerator.FileIO
{
    interface IFileWriter
    {
        void write(CodeCompileUnit compileUnit, string className, string filePath);
    }
}
