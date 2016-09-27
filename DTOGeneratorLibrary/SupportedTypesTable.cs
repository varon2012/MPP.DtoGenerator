using System;
using System.Collections.Generic;

namespace DTOGeneratorLibrary
{
    public enum TypeKind
    {
        Integer,
        Number,
        Boolean,
        String,
        Object
    }

    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(TypeKind typeKind, string format) 
            : base($"Type '{typeKind}' with format '{format}' not registered")
        { }
    }

    public class SupportedTypesTable
    {
        private readonly List<TypeDescription> _registeredTypes = new List<TypeDescription>();

        private SupportedTypesTable()
        {
            RegisterBuiltinTypes();
        }

        // Public 

        public static SupportedTypesTable Instance { get; } = new SupportedTypesTable();

        public Type GetNetType(TypeKind typeKind, string format)
        {
            Type result = _registeredTypes.Find(x => (x.TypeKind == typeKind) && (x.FormatName == format)).NetType;
            if (result != null)
                return result;

            throw new TypeNotRegisteredException(typeKind, format);
        }

        // Internals

        private void RegisterBuiltinTypes()
        {
            TypeDescription[] builtinTypes =
            {
                new TypeDescription(TypeKind.Integer, "int32", typeof(int)),
                new TypeDescription(TypeKind.Integer, "int64", typeof(long)),
                new TypeDescription(TypeKind.Number, "float", typeof(float)),
                new TypeDescription(TypeKind.Number, "double", typeof(double)),  
                new TypeDescription(TypeKind.String, "byte", typeof(byte)), 
                new TypeDescription(TypeKind.Boolean, null, typeof(bool)),
                new TypeDescription(TypeKind.String, "date", typeof(DateTime)),  
                new TypeDescription(TypeKind.String, "string", typeof(string)), 
            };

            foreach (TypeDescription typeDescription in builtinTypes)
            {
                RegisterType(typeDescription);
            }
        }

        private void RegisterType(TypeDescription typeDescription)
        {
            _registeredTypes.Add(typeDescription);
        }
    }
}
