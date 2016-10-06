using System.Diagnostics.CodeAnalysis;

namespace DtoGenerationLibrary
{
    public struct DtoClassDeclaration
    {
        public readonly string ClassName;
        public readonly string ClassDeclaration;

        public DtoClassDeclaration(string className, string classDeclaration)
        {
            ClassName = className;
            ClassDeclaration = classDeclaration;
        }
    }
}
