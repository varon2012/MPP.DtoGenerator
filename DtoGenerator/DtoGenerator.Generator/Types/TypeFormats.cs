using System;
using System.Collections.Generic;

namespace DtoGenerator.Generator.Types
{
    internal sealed class TypeFormats
    {
        public bool HasSingleFormat { get; }
        private readonly Type _singleFormat;

        private readonly Dictionary<string, Type> _formats;

        public TypeFormats()
        {
            HasSingleFormat = false;
            _formats = new Dictionary<string, Type>();
        }

        public TypeFormats(Type singleFormat)
        {
            if (singleFormat == null) throw new ArgumentNullException(nameof(singleFormat));

            HasSingleFormat = true;
            _singleFormat = singleFormat;
        }

        public Type ResolveFormat(string format)
        {
            Type resultType;

            if (HasSingleFormat)
            {
                resultType = _singleFormat;
            }
            else
            {
                if (format == null) throw new ArgumentNullException(nameof(format));

                try
                {
                    resultType = _formats[format];
                }
                catch (KeyNotFoundException)
                {
                    throw new TypeFormatNotFoundException($"Type format not found: {format}");
                }
            }

            return resultType;
        }

        public TypeFormats AddTypeFormat(string format, Type dotNetType)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (dotNetType == null) throw new ArgumentNullException(nameof(dotNetType));

            if (HasSingleFormat)
            {
                throw new InvalidOperationException("This instance has only single type format");
            }

            _formats[format] = dotNetType;

            return this;
        }
    }
}
