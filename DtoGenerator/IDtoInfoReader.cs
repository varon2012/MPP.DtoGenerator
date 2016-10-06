using System;

namespace DtoGenerator
{
    public interface IDtoInfoListReader : IDisposable
    {
        event Action<DtoInfo> OnDtoInfoReaded;
        event Action OnReadCompleted; 

        void ReadList();
    }
}