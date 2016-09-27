using System.ComponentModel.Composition;

using TypeDescription;

namespace DoubleDescription
{
    [Export(typeof(TypeDescriptor))]
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
