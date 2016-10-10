using System;
using DtoPlugin;

namespace BasePluginList
{
    public class DoublePlugin : IDtoPlugin
    {
        public string Type => "number";

        public string Format => "double";

        public Type TypeObj => typeof(double);
    }
}
