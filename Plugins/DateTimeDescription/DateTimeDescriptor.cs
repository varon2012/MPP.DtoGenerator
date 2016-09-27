using System.ComponentModel.Composition;

using TypeDescription;

namespace DateTimeDescription
{
    [Export(typeof(TypeDescriptor))]

    public class DateTimeDescriptor : TypeDescriptor
    {
        public DateTimeDescriptor()
        {
            Type = "string";
            Format = "date";
            NETType = "System.DateTime";
        }
    }
}
