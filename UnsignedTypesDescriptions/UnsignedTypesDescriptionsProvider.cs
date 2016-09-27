using DTOGeneratorLibrary;

namespace UnsignedTypesDescriptions
{
    public class UnsignedTypesDescriptionsProvider : ITypeDescriptionsProvider
    {
        public TypeDescription[] TypeDescriptions => new[]
        {
            new TypeDescription(TypeKind.Integer, "uint32", typeof(uint)), 
            new TypeDescription(TypeKind.Integer, "uint64", typeof(ulong)), 
        };
    }
}
