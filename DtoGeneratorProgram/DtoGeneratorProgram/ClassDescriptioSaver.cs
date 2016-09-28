using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace DtoGeneratorProgram
{
    internal static class ClassDescriptioSaver
    {
        public static void SaveCode(string path, CodeCompileUnit compileunit)
        {

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

            IndentedTextWriter codeWriter = new IndentedTextWriter(new StreamWriter(path, false), "    ");
            provider.GenerateCodeFromCompileUnit(compileunit, codeWriter, new CodeGeneratorOptions());

            codeWriter.Close();
        }
    }
}
