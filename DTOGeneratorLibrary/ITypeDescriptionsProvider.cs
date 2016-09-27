using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOGeneratorLibrary
{
    public interface ITypeDescriptionsProvider
    {
        TypeDescription[] TypeDescriptions { get; }
    }
}
