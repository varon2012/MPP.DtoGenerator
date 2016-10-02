using System.Collections.Generic;

namespace DtoGenerator.Contracts.InputModels
{
    public interface IClassDescriptions
    {
        IEnumerable<IClassDescription> Classes { get; set; }
    }
}
