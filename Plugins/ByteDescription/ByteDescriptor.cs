using TypeDescription;

namespace ByteDescription
{
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
