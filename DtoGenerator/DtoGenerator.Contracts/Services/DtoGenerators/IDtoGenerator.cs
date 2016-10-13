using System;
using System.Collections.Generic;

namespace DtoGenerator.Contracts.Services.DtoGenerators
{
    public interface IDtoGenerator:IDisposable 
    {
        IDictionary<string,string> Generate(string json);
    }
}
