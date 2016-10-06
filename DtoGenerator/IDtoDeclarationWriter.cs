using System;

namespace DtoGenerator
{
    public interface IDtoDeclarationWriter : IDisposable
    {
        void Write(DtoDeclaration dtoDeclaration);
    }
}