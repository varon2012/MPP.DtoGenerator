using System;
using DtoPlugin;

namespace BasePluginList
{
    public class StringPlugin : IDtoPlugin
    {
        public string Type => "string";

        public string Format => "string";

        public Type TypeObj => typeof(string);
    }
}
