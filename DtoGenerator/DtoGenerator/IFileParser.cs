using System.Collections.Generic;

namespace DtoGenerator
{
    internal interface IFileParser<out T>
    {
        IEnumerable<T> Parse(string filename);
    }
}
