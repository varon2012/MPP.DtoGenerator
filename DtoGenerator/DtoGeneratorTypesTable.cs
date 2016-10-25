using System;
using System.Collections.Generic;

namespace DtoGenerator
{
    public class DtoGeneratorTypesTable
    {

        public static DtoGeneratorTypesTable Instance { get; } = new DtoGeneratorTypesTable();
        private Dictionary<DtoTypeInfoKey, DtoTypeInfo> _dtoTypes;

        private DtoGeneratorTypesTable()
        {
            InitializeDefaultTypes();
        }

        private void InitializeDefaultTypes()
        {
            _dtoTypes = new Dictionary<DtoTypeInfoKey, DtoTypeInfo>();
            _dtoTypes.Add(new DtoTypeInfoKey(TypeForm.Integer, "int32"), new DtoTypeInfo(TypeForm.Integer, "int32", typeof(int)));
            _dtoTypes.Add(new DtoTypeInfoKey(TypeForm.Integer, "int64"), new DtoTypeInfo(TypeForm.Integer, "int64", typeof(long)));
            _dtoTypes.Add(new DtoTypeInfoKey(TypeForm.Number, "float"), new DtoTypeInfo(TypeForm.Number, "float", typeof(float)));
            _dtoTypes.Add(new DtoTypeInfoKey(TypeForm.Number, "double") , new DtoTypeInfo(TypeForm.Number, "double", typeof(double)));
            _dtoTypes.Add(new DtoTypeInfoKey(TypeForm.Number, "byte"), new DtoTypeInfo(TypeForm.Number, "byte", typeof(byte)));
            _dtoTypes.Add(new DtoTypeInfoKey(TypeForm.Boolean, null), new DtoTypeInfo(TypeForm.Boolean, null, typeof(bool)));
            _dtoTypes.Add(new DtoTypeInfoKey(TypeForm.String, "date"), new DtoTypeInfo(TypeForm.String, "date", typeof(DateTime)));
            _dtoTypes.Add(new DtoTypeInfoKey(TypeForm.String, "string"), new DtoTypeInfo(TypeForm.String, "string", typeof(string)));
        }

        public DtoTypeInfo GetDotTypeInfo(TypeForm form, string format)
        {
            DtoTypeInfo dtoTypeInfo;
            if (_dtoTypes.TryGetValue(new DtoTypeInfoKey(form, format), out dtoTypeInfo))
            {
                return dtoTypeInfo;
            }
            throw new TypeNotSupportedException();
        }
    }
}
