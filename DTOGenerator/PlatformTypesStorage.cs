using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOGenerator
{
    class PlatformTypesStorage
    {
        public static volatile PlatformTypesStorage instance;
        public static readonly object syncRoot = new object();

        public List<PlatformTypeDescription> PlatformTypesList { get; private set; }

        private PlatformTypesStorage()
        {
            PlatformTypesList = new List<PlatformTypeDescription>();
        }

        public PlatformTypesStorage Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new PlatformTypesStorage();
                    }
                }
            }
            return instance;
        }

        private void AddBasicTypes()
        {
            PlatformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Integer, "int32", typeof(Int32)));
            PlatformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Integer, "int64", typeof(Int64)));
            PlatformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Number, "float", typeof(Single)));
            PlatformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Number, "double", typeof(Double)));
            PlatformTypesList.Add(new PlatformTypeDescription(NonPlatformType.String, "byte", typeof(Byte)));
            PlatformTypesList.Add(new PlatformTypeDescription(NonPlatformType.Boolean, null, typeof(Boolean)));
            PlatformTypesList.Add(new PlatformTypeDescription(NonPlatformType.String, "date", typeof(DateTime)));
            PlatformTypesList.Add(new PlatformTypeDescription(NonPlatformType.String, "string", typeof(String)));
        }
    }
}
