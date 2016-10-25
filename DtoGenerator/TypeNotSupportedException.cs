using System;

namespace DtoGenerator
{
    [Serializable]
    public class TypeNotSupportedException : Exception
    {

        public TypeNotSupportedException() { }

        public TypeNotSupportedException(string message) : base(message) { }

        public TypeNotSupportedException(string message, Exception innerException) : base(message, innerException) { }

    }
}
