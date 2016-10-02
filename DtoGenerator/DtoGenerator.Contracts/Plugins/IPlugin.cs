using System.Collections.Generic;

namespace DtoGenerator.Contracts.Plugins
{
    public interface IPlugin
    {
        IEnumerable<ITypeDescription> GetTypes( ); 
    }
}
