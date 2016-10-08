using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator.Parser
{
    interface IParser<T>
    {
        T parse(string jsonString);
    }
}
