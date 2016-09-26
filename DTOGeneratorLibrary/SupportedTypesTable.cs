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

    internal class SupportedTypesTable
    {
        private readonly List<TypeDescription> _registeredTypes = new List<TypeDescription>();

        internal SupportedTypesTable()
        {
            RegisterBuiltinTypes();
        }

        internal Type GetNetType(TypeKind typeKind, string format)
        {
            return _registeredTypes.Find(x => (x.TypeKind == typeKind) && (x.FormatName == format)).NetType;
        }

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
