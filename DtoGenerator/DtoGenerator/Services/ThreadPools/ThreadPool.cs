using System;
using System.Collections.Generic;
using System.Threading;
using DtoGenerator.Contracts.Services.ThreadPools;

namespace DtoGenerator.Services.ThreadPools
{
    internal class ThreadPool : IThreadPool
    {
        #region Private Members

        private readonly object _syncObject = new object();
        private readonly Queue<TaskItem> _queue = new Queue<TaskItem>();
        private readonly Thread[] _threads;
        private int _threadsWaiting;
        private bool _shutdown;

        #endregion

        #region Ctor

        internal ThreadPool() :
           this(Environment.ProcessorCount){ }

        internal ThreadPool(int threadCount)
        {
            if (threadCount <= 0)
                throw new ArgumentOutOfRangeException("Thread Count must be greater than zero.");

            _threads = new Thread[threadCount];
            for (int i = 0; i < _threads.Length; i++)
            {
                _threads[i] = new Thread(DispatchLoop);
                _threads[i].Start();
            }
        }

        #endregion

        #region Public Methods

        public void QueueUserWorkItem(WaitCallback work, object state)
        {

            var taskItem = new TaskItem(work, state);

            lock (_syncObject)
            {
                _queue.Enqueue(new TaskItem(work,state));
                if (_threadsWaiting > 0)
                    Monitor.Pulse(_syncObject);
            }
        }

        public void Dispose()
        {
            _shutdown = true;
            lock (_syncObject)
            {
                Monitor.PulseAll(_syncObject);
            }

            for (int i = 0; i < _threads.Length; i++)
                _threads[i].Join();
        }

        #endregion

        #region Private Methods

        private void DispatchLoop()
        {
            while (true)
            {
                TaskItem workItem = default(TaskItem);
                lock (_syncObject)
                {
                    if (_shutdown)
                        return;

                    while (_queue.Count == 0)
                    {
                        _threadsWaiting++;
                        try
                        {
                            Monitor.Wait(_syncObject);
                        }
                        finally
                        {
                            _threadsWaiting--;
                        }

                        if (_shutdown)
                            return;
                    }
                    workItem = _queue.Dequeue();
                }
                workItem.Invoke();
            }
        }

        #endregion
    }
}