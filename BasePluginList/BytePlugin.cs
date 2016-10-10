using System;
using DtoPlugin;

namespace BasePluginList
{
    public class BytePlugin : IDtoPlugin
    {
        public string Type => "string";

        public string Format => "byte";

        public Type TypeObj => typeof(byte);
    }
}
