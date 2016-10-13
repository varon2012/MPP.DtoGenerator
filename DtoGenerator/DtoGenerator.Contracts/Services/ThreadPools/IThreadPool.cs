using System;
using System.Threading;

namespace DtoGenerator.Contracts.Services.ThreadPools
{
    public interface IThreadPool : IDisposable
    {
        void QueueUserWorkItem(WaitCallback work, object state);
    }
}
