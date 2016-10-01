using System.Threading;

namespace DtoGenerator.Services.ThreadPool
{
    internal struct TaskItem
    {
        private WaitCallback _work;
        private object _state;

        internal TaskItem(WaitCallback work, object state)
        {
            _work = work;
            _state = state;
        }

        internal void Invoke()
        {
            _work(_state);
        }
    }
}
