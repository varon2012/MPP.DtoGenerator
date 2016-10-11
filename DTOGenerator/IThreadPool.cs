using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DTOGenerator
{
    interface IThreadPool : IDisposable
    {
        void QueueUserWorkItem(WaitCallback work, object obj);
    }
}
