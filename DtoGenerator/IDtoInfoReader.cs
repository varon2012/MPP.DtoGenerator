using System;

namespace DtoGenerator
{
    public interface IDtoInfoListReader
    { 
        event Action<DtoInfo> OnDtoInfoRead;
        event Action OnReadCompleted; 

        void ReadList();
    }
}