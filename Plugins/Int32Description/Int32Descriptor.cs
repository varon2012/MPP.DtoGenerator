using System.ComponentModel.Composition;

using TypeDescription;

namespace Int32Description
{
    [Export(typeof(TypeDescriptor))]
    public class Int32Descriptor : TypeDescriptor
    {
        public Int32Descriptor()
        {
            Type = "integer";
            Format = "int32";
            NETType = "System.Int32";
        }
    }
}
