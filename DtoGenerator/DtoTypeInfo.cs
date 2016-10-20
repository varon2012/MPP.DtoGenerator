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

    public sealed class DtoTypeInfoKey : IEquatable<DtoTypeInfoKey>
    {
        public TypeForm Form { get; }
        public string Format { get; }

        public DtoTypeInfoKey(TypeForm form, string format)
        {
            Form = form;
            Format = format;
        }

        public bool Equals(DtoTypeInfoKey other)
        {
            if (other == null) return false;
            return other.Format != null && other.Form.Equals(Form) && other.Format.Equals(Format);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            DtoTypeInfoKey other = (DtoTypeInfoKey) obj;
            return other.Format != null && other.Form.Equals(Form) && other.Format.Equals(Format);
        }

        public override int GetHashCode()
        {
            int result = 17 * Form.GetHashCode();
            if (Format != null)
            {
                result += 31 * Format.GetHashCode();
            }
            else
            {
                result++;
            }
//            result += 31 * ((Format != null)? Format.GetHashCode() : 1);
            return result;
        }
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