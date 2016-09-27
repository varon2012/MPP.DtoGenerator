using System.Diagnostics.CodeAnalysis;

namespace DTOGeneratorLibrary
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct DTOClassDeclaration
    {
        public readonly string ClassName;
        public readonly string ClassDeclaration;

        public DTOClassDeclaration(string className, string classDeclaration)
        {
            ClassName = className;
            ClassDeclaration = classDeclaration;
        }
    }
}
