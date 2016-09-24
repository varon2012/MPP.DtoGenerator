using System;
using System.Collections.Generic;

namespace DtoGenerator.Generator
{
    public class DtoClassDeclaration
    {
        public string ClassName { get; }
        public IEnumerable<DtoClassProperty> Properties { get; }

        public DtoClassDeclaration(string className, IEnumerable<DtoClassProperty> properties)
        {
            if (className == null) throw new ArgumentNullException(nameof(className));
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            ClassName = className;
            Properties = properties;
        }
    }
}
