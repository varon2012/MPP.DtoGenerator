using System.ComponentModel.Composition;

using TypeDescription;

namespace BooleanDescription
{
    [Export(typeof(TypeDescriptor))]
    public class BooleanDescriptor : TypeDescriptor
    {
        public BooleanDescriptor()
        {
            Type = "boolean";
            Format = "";
            NETType = "System.Boolean";
        }
    }
}
