using System.ComponentModel.Composition;

using TypeDescription;

namespace StringDescription
{
    [Export(typeof(TypeDescriptor))]
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
