using System;
using DtoPlugin;

namespace BasePluginList
{
    public class BoolPlugin : IDtoPlugin
    {
        public string Type => "boolean";

        public string Format => string.Empty;

        public Type TypeObj => typeof(bool);
    }
}
