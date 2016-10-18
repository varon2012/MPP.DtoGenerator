namespace DtoGenerator
{
    public sealed class DtoFieldInfo
    {
        public DtoFieldInfo(string name, DtoTypeInfo dtoType)
        {
            Name = name;
            DtoType = dtoType;
        }

        public string Name { get; }
        public DtoTypeInfo DtoType { get; }
    }
}