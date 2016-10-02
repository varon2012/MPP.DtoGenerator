using System;
using System.Collections.Generic;
using DtoGenerator.Contracts.Plugins;

namespace NumberPlugin
{
    public class NumberPlugin : IPlugin
    {
        private IEnumerable<ITypeDescription> types;

        public NumberPlugin()
        {
            types = CreateTypes();
        }

        public IEnumerable<ITypeDescription> GetTypes()
        {
            return types;
        }

        private IEnumerable<ITypeDescription> CreateTypes()
        {
            return new TypeDescription[]
            {
                new TypeDescription
                {
                    Type = "number",
                    Format = "float",
                    DotNetType = typeof(float)
                },
                new TypeDescription
                {
                    Type = "number",
                    Format = "double",
                    DotNetType = typeof(double)
                }
            };
        }
    }
}
