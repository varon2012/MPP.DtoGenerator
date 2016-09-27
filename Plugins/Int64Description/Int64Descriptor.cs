using TypeDescription;

namespace Int64Description
{
    public class Int64Descriptor : TypeDescriptor
    {
        public Int64Descriptor()
        {
            Type = "integer";
            Format = "int64";
            NETType = "System.Int64";
        }
    }
}
