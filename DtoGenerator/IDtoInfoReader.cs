using System;

namespace DtoGenerator
{
    public interface IDtoInfoListReader : IDisposable
    {
        event Action<DtoInfo> OnDtoInfoRead;
        event Action OnReadCompleted; 

        void ReadList();
    }
}