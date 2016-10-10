using System;

namespace DtoPlugin
{
    public interface IDtoPlugin
    {
        string Type { get; }
        string Format { get; }
        Type TypeObj { get; }
    }
}
