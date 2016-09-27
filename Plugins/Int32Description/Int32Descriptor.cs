using TypeDescription;

namespace Int32Description
{
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
