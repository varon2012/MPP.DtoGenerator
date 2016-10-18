using System;
using System.Runtime.Serialization;

namespace DtoGenerator.Generator.Types
{
    [Serializable]
    internal class TypeException : Exception
    {
        public TypeException()
        {
        }

        public TypeException(string message) : base(message)
        {
        }

        public TypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}