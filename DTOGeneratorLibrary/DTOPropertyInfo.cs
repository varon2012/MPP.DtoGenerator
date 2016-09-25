using System;
using System.Diagnostics.CodeAnalysis;

namespace DTOGeneratorLibrary
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DTOPropertyInfo
    {
        public string Name { get; set; }
        public Type PropertyType { get; set; }
    }
}