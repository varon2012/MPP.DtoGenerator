using System;

namespace DtoGenerator
{
    public enum TypeForm
    {
        Integer,
        Number,
        Boolean,
        String
    }

    public sealed class DtoTypeInfo
    {
        public DtoTypeInfo(TypeForm form, string format, Type dotNetType)
        {
            Form = form;
            Format = format;
            DotNetType = dotNetType;
        }

        public TypeForm Form { get; }
        public string Format { get; }
        public Type DotNetType { get; }
    }
}