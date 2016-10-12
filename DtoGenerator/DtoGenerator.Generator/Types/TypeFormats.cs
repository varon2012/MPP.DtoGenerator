using System;
using System.Collections.Generic;

namespace DtoGenerator.Generator.Types
{
    internal sealed class TypeFormats
    {
        private readonly bool _hasSingleFormat;
        private readonly Type _singleFormat;

        private readonly Dictionary<string, Type> _formats;

        internal TypeFormats()
        {
            _hasSingleFormat = false;
            _formats = new Dictionary<string, Type>();
        }

        internal TypeFormats(Type singleFormat)
        {
            if (singleFormat == null) throw new ArgumentNullException(nameof(singleFormat));

            _hasSingleFormat = true;
            _singleFormat = singleFormat;
        }

        internal Type ResolveFormat(string format)
        {
            Type resultType;

            if (_hasSingleFormat)
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

        internal TypeFormats AddTypeFormat(string format, Type dotNetType)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            if (dotNetType == null) throw new ArgumentNullException(nameof(dotNetType));

            if (_hasSingleFormat)
            {
                throw new InvalidOperationException("This instance has only single type format");
            }

            _formats[format] = dotNetType;

            return this;
        }
    }
}
