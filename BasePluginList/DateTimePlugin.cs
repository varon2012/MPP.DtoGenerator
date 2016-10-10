using System;
using DtoPlugin;

namespace BasePluginList
{
    public class DateTimePlugin : IDtoPlugin
    {
        public string Type => "string";

        public string Format => "date";

        public Type TypeObj => typeof(System.DateTime);
    }
}
