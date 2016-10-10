using System;
using DtoPlugin;

namespace BasePluginList
{
    public class Int32Plugin : IDtoPlugin
    {
        public string Type => "integer";

        public string Format => "int32";

        public Type TypeObj => typeof(System.Int32);
    }
}
