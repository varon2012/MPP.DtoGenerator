using System;
using System.Collections.Generic;

namespace DtoGenerator.Generator
{
    public struct DtoClassDescription
    {
        public string ClassName { get; set; }
        public IEnumerable<DtoClassProperty> Properties { get; set; }

        public DtoClassDescription(string className, IEnumerable<DtoClassProperty> properties)
        {
            if (className == null) throw new ArgumentNullException(nameof(className));
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            ClassName = className;
            Properties = properties;
        }
    }
}
