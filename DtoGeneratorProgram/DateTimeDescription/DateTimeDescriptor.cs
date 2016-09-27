using TypeDescription;

namespace DateTimeDescription
{
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
