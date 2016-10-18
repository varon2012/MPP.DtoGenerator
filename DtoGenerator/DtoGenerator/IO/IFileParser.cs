using System.Collections.Generic;

namespace DtoGenerator.IO
{
    internal interface IFileParser<out T>
    {
        IEnumerable<T> Parse(string filename);
    }
}
