using System.ComponentModel.Composition;

using TypeDescription;

namespace ByteDescription
{
    [Export(typeof(TypeDescriptor))]
    public class ByteDescriptor : TypeDescriptor
    {
        public ByteDescriptor()
        {
            Type = "string";
            Format = "byte";
            NETType = "System.Byte";
        }
    }
}
