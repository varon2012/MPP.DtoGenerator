using System;
using DtoPlugin;

namespace BasePluginList
{
    public class Int64Plugin : IDtoPlugin
    {
        public string Type => "integer";

        public string Format => "int64";

        public Type TypeObj => typeof(System.Int64);
    }
}
