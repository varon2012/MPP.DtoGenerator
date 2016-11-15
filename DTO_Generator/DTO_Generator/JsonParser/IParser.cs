using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generator.Descriptions;

namespace DTO_Generator.JsonParser
{
    interface IParser
    {
        ClassesList ParseClassDescriptions(string jsonInput);
    }
}
