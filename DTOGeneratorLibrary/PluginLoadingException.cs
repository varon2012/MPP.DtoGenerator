using System;
using System.Runtime.Serialization;

namespace DtoGenerationLibrary
{
    [Serializable]
    public class PluginLoadingException : Exception
    {
        public PluginLoadingException()
        {
        }

        public PluginLoadingException(string message) : base(message)
        {
        }

        public PluginLoadingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PluginLoadingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}