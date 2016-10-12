using System;
using System.Runtime.Serialization;

namespace DtoGenerator.Generator.Types
{
    [Serializable]
    internal class TypeFormatNotFoundException : Exception
    {
        public TypeFormatNotFoundException()
        {
        }

        public TypeFormatNotFoundException(string message) : base(message)
        {
        }

        public TypeFormatNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }   

        protected TypeFormatNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}