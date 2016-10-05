using System.Threading;

namespace DtoGenerator.Services.ThreadPools
{
    internal struct TaskItem
    {
        private WaitCallback _callback;
        private object _state;

        internal TaskItem(WaitCallback callback, object state)
        {
            _callback = callback;
            _state = state;
        }

        internal void Invoke()
        {
            _callback(_state);
        }
    }
}
