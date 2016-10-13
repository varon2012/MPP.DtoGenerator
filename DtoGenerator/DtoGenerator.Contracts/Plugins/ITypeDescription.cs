using System;

namespace DtoGenerator.Contracts.Plugins
{
    public interface ITypeDescription
    {
        string Type { get; set; }
        string Format { get; set; }
        Type DotNetType { get; set; }
    }
}
