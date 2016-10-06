using System;
using System.Runtime.Serialization;

namespace DtoGenerationLibrary
{
    [Serializable]
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException()
        {
        }

        public TypeNotRegisteredException(string message) : base(message)
        {
        }

        public TypeNotRegisteredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TypeNotRegisteredException(TypeKind typeKind, string format)
            : base($"Type '{typeKind}' with format '{format}' not registered")
        {
        }

        protected TypeNotRegisteredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}