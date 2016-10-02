using System.Collections;
using System.Collections.Generic;

namespace DtoGenerator.Contracts.InputModels
{
    public interface IClassDescription
    {
        string ClassName { get; set; }
        IEnumerable<IPropertyDescription> Properties { get; set; }
    }
}
