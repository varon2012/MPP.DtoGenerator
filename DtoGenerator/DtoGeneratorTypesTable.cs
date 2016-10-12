using System;
using System.Collections.Generic;

namespace DtoGenerator
{
    public class DtoGeneratorTypesTable
    {

        public static DtoGeneratorTypesTable Instance { get; } = new DtoGeneratorTypesTable();
        private HashSet<DtoTypeInfo> _dtoTypes;

        private DtoGeneratorTypesTable()
        {
            InitializeDefaultTypes();
        }

        private void InitializeDefaultTypes()
        {
            _dtoTypes = new HashSet<DtoTypeInfo>
            {
                new DtoTypeInfo(TypeForm.Integer, "int32", typeof(int)),
                new DtoTypeInfo(TypeForm.Integer, "int64", typeof(long)),
                new DtoTypeInfo(TypeForm.Number, "float", typeof(float)),
                new DtoTypeInfo(TypeForm.Number, "double", typeof(double)),
                new DtoTypeInfo(TypeForm.String, "byte", typeof(byte)),
                new DtoTypeInfo(TypeForm.Boolean, null, typeof(bool)),
                new DtoTypeInfo(TypeForm.String, "date", typeof(DateTime)),
                new DtoTypeInfo(TypeForm.String, "string", typeof(string))
            };
        }

        public Type GetDotNetType(TypeForm form, string format)
        {
            foreach (DtoTypeInfo dtoTypeInfo in _dtoTypes)
            {
                if (dtoTypeInfo.Format.Equals(format) && dtoTypeInfo.Form.Equals(form))
                {
                    return dtoTypeInfo.DotNetType;
                }    
            }

            throw new TypeNotSupportedException();
        }
    }
}
