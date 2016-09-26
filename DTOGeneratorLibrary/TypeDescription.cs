using System;

namespace DTOGeneratorLibrary
{
    public struct TypeDescription
    {
        public readonly TypeKind TypeKind;
        public readonly string FormatName;
        public readonly Type NetType;

        public TypeDescription(TypeKind typeKind, string formatName, Type netType)
        {
            FormatName = formatName;
            NetType = netType;
            TypeKind = typeKind;
        }
    }
}
