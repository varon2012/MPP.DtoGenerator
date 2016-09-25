using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOGeneratorLibrary
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DTOClassInfo
    {
        public string Name { get; set; }
        public DTOPropertyInfo[] Properties { get; set; }
    }
}
