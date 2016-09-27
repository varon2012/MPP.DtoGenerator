using System.Diagnostics.CodeAnalysis;

namespace DTOGeneratorLibrary
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DTOClassInfo
    {
        public string Name { get; set; }
        public DTOPropertyInfo[] Properties { get; set; }
    }
}
