using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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

    public class PluginLoadingException : Exception { }

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

        public void LoadExternalTypes(string lookupDirectoryPath)
        {
            foreach (string file in Directory.EnumerateFiles(lookupDirectoryPath))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] exportedTypes = assembly.GetExportedTypes();

                    Type[] typeDescriptionsProvidersTypes = Array.FindAll(exportedTypes,
                        type => typeof(ITypeDescriptionsProvider).IsAssignableFrom(type) && !type.IsAbstract);
                    foreach (Type typeDescriptionsProviderType in typeDescriptionsProvidersTypes)
                    {
                        ITypeDescriptionsProvider typeDescriptionsProvider =
                            (ITypeDescriptionsProvider) Activator.CreateInstance(typeDescriptionsProviderType);
                        RegisterTypes(typeDescriptionsProvider.TypeDescriptions);
                    }
                }
                catch (Exception)
                {
                    throw new PluginLoadingException();
                }
            }
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

            RegisterTypes(builtinTypes);
        }

        private void RegisterTypes(IEnumerable<TypeDescription> typeDescriptions)
        {
            foreach (TypeDescription typeDescription in typeDescriptions)
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
