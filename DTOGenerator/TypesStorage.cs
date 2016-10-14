using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOGenerator
{
    public class TypesStorage
    {
        public static TypesStorage instance;
        public static readonly object syncRoot = new object();

        public Dictionary<string, NonPlatformType> nonPlatformTypesMappingDictionary { get; private set;}

        private List<PlatformTypeDescription> platformTypesList;

        private TypesStorage()
        {
            platformTypesList = new List<PlatformTypeDescription>();
            nonPlatformTypesMappingDictionary = new Dictionary<string, NonPlatformType>();

            AddBasicTypes();
            InitializeNonPlatformTypesMappingDictionary();
        }

        public static TypesStorage Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new TypesStorage();
                    }
                }
            }
            return instance;
        }

        private void AddBasicTypes()
        {
            platformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Integer, "int32", typeof(Int32)));
            platformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Integer, "int64", typeof(Int64)));
            platformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Number, "float", typeof(Single)));
            platformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Number, "double", typeof(Double)));
            platformTypesList.Add(new PlatformTypeDescription(NonPlatformType.String, "byte", typeof(Byte)));
            platformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Boolean, null, typeof(Boolean)));
            platformTypesList.Add(new PlatformTypeDescription(NonPlatformType.String, "date", typeof(DateTime)));
            platformTypesList.Add(new PlatformTypeDescription(NonPlatformType.String, "string", typeof(String)));
        }

        private void InitializeNonPlatformTypesMappingDictionary()
        {
            nonPlatformTypesMappingDictionary.Add("integer", NonPlatformType.Integer);
            nonPlatformTypesMappingDictionary.Add("number", NonPlatformType.Number);
            nonPlatformTypesMappingDictionary.Add("string", NonPlatformType.String);
            nonPlatformTypesMappingDictionary.Add("boolean", NonPlatformType.Boolean);
        }

        public NonPlatformType GetNonPlatformType(string type)
        {
            if (nonPlatformTypesMappingDictionary.ContainsKey(type))
            {
                return nonPlatformTypesMappingDictionary[type];
            }
            else
            {
                throw new InvalidOperationException("NonPlatformType is not found.");
            }
        }

        public Type GetPlatformType(NonPlatformType type, string format)
        {
            foreach (PlatformTypeDescription platformTypeDescription in platformTypesList)
            {
                if ((platformTypeDescription.Type == type) && (platformTypeDescription.Format == format))
                    return platformTypeDescription.PlatformType;
            }
            throw new InvalidOperationException("Platfrom type is not found.");
        }
    }
}
