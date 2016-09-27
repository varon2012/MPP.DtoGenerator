using TypeDescription;

namespace StringDescription
{
    public class StringDescriptor : TypeDescriptor
    {
        public StringDescriptor()
        {
            Type = "string";
            Format = "string";
            NETType = "System.String";
        }
    }
}
