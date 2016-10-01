using System;
using System.Threading;

namespace DtoGenerator.Contracts.ThreadPool
{
    internal interface IThreadPool : IDisposable
    {
        void QueueUserWorkItem(WaitCallback work, object state);
    }
}
