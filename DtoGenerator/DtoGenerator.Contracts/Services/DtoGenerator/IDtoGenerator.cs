using System;
using System.Collections.Generic;

namespace DtoGenerator.Contracts.Services.DtoGenerator
{
    public interface IDtoGenerator:IDisposable 
    {
        IDictionary<string,string> Generate(string json);
    }
}
