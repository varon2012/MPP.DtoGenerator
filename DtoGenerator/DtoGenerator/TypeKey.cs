using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoGenerator
{
    internal class TypeKey : IEquatable<TypeKey>
    {
        public string Type { get; }
        public string Format { get; }

        public TypeKey(string type, string format)
        {
            if (type == null || format == null)
                throw new ArgumentNullException();
            Type = type;
            Format = format;
        }

        public bool Equals(TypeKey other)
        {
            if (other == null) return false;
            return string.Equals(Type, other.Type) && string.Equals(Format, other.Format);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as TypeKey);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) + Type.GetHashCode();
                hashCode = (hashCode * 397) + Format.GetHashCode();

                return hashCode;
            }
        }
    }
}