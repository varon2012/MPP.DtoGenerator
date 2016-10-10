using System;
using DtoPlugin;

namespace BasePluginList
{
    public class FloatPlugin : IDtoPlugin
    {
        public string Type => "number";

        public string Format => "float";

        public Type TypeObj => typeof(float);
    }
}
