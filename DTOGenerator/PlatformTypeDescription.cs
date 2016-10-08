using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOGenerator
{
    public class PlatformTypeDescription
    {
        public NonPlatformType Type { get; private set; }
        public string Format { get; private set; }
        public Type PlatformType { get; private set; }

        internal PlatformTypeDescription(NonPlatformType type, string format, Type platformType)
        {
            this.Type = type;
            this.Format = format;
            this.PlatformType = platformType;
        } 
    }
}
