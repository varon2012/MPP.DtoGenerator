using System;

namespace DtoGenerationLibrary
{
    public class TypeDescription
    {
        public TypeKind TypeKind { get; }
        public string FormatName { get; }
        public Type NetType { get; }

        public TypeDescription(TypeKind typeKind, string formatName, Type netType)
        {
            FormatName = formatName;
            NetType = netType;
            TypeKind = typeKind;
        }
    }
}
