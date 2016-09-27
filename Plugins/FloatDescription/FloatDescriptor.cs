using System.ComponentModel.Composition;

using TypeDescription;

namespace FloatDescription
{
    [Export(typeof(TypeDescriptor))]
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
