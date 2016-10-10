namespace TypeDescription
{
    public abstract class TypeDescriptor
    {
        public string Type { get; protected set; }
        public string Format { get; protected set; }
        public string NETType { get; protected set; }
    }
}
