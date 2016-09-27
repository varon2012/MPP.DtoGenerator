using TypeDescription;
namespace DoubleDescription
{
    public class DoubleDescriptor : TypeDescriptor
    {
        public DoubleDescriptor()
        {
            Type = "number";
            Format = "double";
            NETType = "System.Double";
        }
    }
}
