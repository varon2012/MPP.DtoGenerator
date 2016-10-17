using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadPool
{
    public struct TaskInfo
    {
        public WaitCallback WaitCallback;
        public object Data;
    }

    public sealed class CustomThreadPool : IDisposable
    {
        public CountdownEvent Countdown { get; }

        private readonly int _maxProcessingTaskCount;
        private volatile int _currentWorkingTaskCount;
        private volatile Queue<TaskInfo> _tasks = new Queue<TaskInfo>();
        private volatile object _syncRoot = new object();

        public CustomThreadPool(int maxProcessingTaskCount)
        {
            _maxProcessingTaskCount = maxProcessingTaskCount;
            Countdown = new CountdownEvent(0);
        }

        public void AddToProcessingQueue(TaskInfo taskInfo)
        {
            lock (_syncRoot)
            {
                if (!Countdown.TryAddCount())
                {
                    Countdown.Reset(1);
                }

                if (_currentWorkingTaskCount < _maxProcessingTaskCount)
                {
                    ProcessTask(taskInfo);
                }
                else
                {
                    _tasks.Enqueue(taskInfo);
                }
            }
        }

        private void ProcessTask(TaskInfo taskInfo)
        {
            _currentWorkingTaskCount++;
            System.Threading.ThreadPool.QueueUserWorkItem(data =>
            {
                taskInfo.WaitCallback(data);
                lock (_syncRoot)
                {
                    _currentWorkingTaskCount--;
                    if (_tasks.Count > 0)
                    {
                        ProcessTask(_tasks.Dequeue());
                    }

                    Countdown.Signal();
                }
            }, taskInfo.Data);
        }

        public void Dispose()
        {
            Countdown.Dispose();
        }
    }
}
