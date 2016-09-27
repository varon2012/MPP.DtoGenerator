using TypeDescription;

namespace FloatDescription
{
    public class FloatDescriptor : TypeDescriptor
    {
        public FloatDescriptor()
        {
            Type = "number";
            Format = "float";
            NETType = "System.Single";
        }
    }
}
