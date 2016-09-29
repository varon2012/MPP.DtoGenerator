using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Threading;

namespace DtoGenerator
{
    internal class ThreadPool : IDisposable
    {
        private readonly int concurrencyLevel;
        private readonly bool flowExecutionContext;
        private readonly Queue<WorkItem> workingQueue = new Queue<WorkItem>();
        private Thread[] threads;
        private int threadsWaitingCount;
        private bool isNeedShutdown;

        public delegate void OnThreadFinishCallback();

        internal ThreadPool() : this(Environment.ProcessorCount, true)
        {
        }

        internal ThreadPool(int concurrencyLevel) : this(concurrencyLevel, true)
        {
        }

        internal ThreadPool(bool flowExecutionContext) : this(Environment.ProcessorCount, flowExecutionContext)
        {
        }

        internal ThreadPool(int concurrencyLevel, bool flowExecutionContext)
        {
            if (concurrencyLevel <= 0)
                throw new ArgumentOutOfRangeException("ConcurrencyLevel should be > 0");
            
            this.concurrencyLevel = concurrencyLevel;
            this.flowExecutionContext = flowExecutionContext;

            // If suppressing flow, we need to demand permissions.
            if (!flowExecutionContext)
                new SecurityPermission(SecurityPermissionFlag.Infrastructure).Demand();
        }

        // Each worker item consists of a closure: worker + (optional) state obj + context.
        struct WorkItem
        {
            internal WaitCallback CallbackFunction { get;}
            internal object ParamObject { get;  }
            internal OnThreadFinishCallback OnThreadFinishCallback { get; }
            private ExecutionContext ExecutionContext { get;  }
        

            internal WorkItem(WaitCallback work, object obj, OnThreadFinishCallback onFinishCallback, ExecutionContext execContext)
            {
                CallbackFunction = work;
                ParamObject = obj;
                ExecutionContext = execContext;
                OnThreadFinishCallback = onFinishCallback;
            }

            internal void Invoke()
            {
                // Run normally (delegate invoke) or under context, as appropriate.
                if (ExecutionContext == null)
                {
                    CallbackFunction(ParamObject);
                }
                else
                {
                    ExecutionContext.Run(ExecutionContext.CreateCopy(), ContextInvoke, null);
                }
            }

            internal void InvokeOnFinishMethod()
            {
                if (OnThreadFinishCallback != null)
                {
                    if (ExecutionContext == null)
                    {
                        OnThreadFinishCallback();
                    }
                    else
                    {
                        ExecutionContext.Run(ExecutionContext.CreateCopy(), OnFinishContextInvoke, null);
                    }
                }
            }

            private void ContextInvoke(object obj)
            {
                CallbackFunction(ParamObject);
            }

            private void OnFinishContextInvoke(object obj)
            {
                OnThreadFinishCallback();
            }
        }


        internal void QueueUserWorkItem(WaitCallback worker, object obj, OnThreadFinishCallback onFinishCallback)
        {

            ExecutionContext executionContext = null;
            if (flowExecutionContext)
                executionContext = ExecutionContext.Capture();

            WorkItem workItem = new WorkItem(worker, obj, onFinishCallback, executionContext);

            // Make sure the pool is started (threads created, etc).
            EnsureStarted();

            // Now insert the worker item into the queue, possibly waking a thread.
            lock (workingQueue)
            {
                workingQueue.Enqueue(workItem);
                if (threadsWaitingCount > 0)
                    Monitor.Pulse(workingQueue);
            }
        }

        // Disposing will signal shutdown, and then wait for all threads to finish.
        public void Dispose()
        {
            isNeedShutdown = true;
            lock (workingQueue)
            {
                Monitor.PulseAll(workingQueue);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

        }

        // Ensures that threads have begun executing.
        private void EnsureStarted()
        {
            if (threads == null)
            {
                lock (workingQueue)
                {
                    if (threads == null)
                    {
                        threads = new Thread[concurrencyLevel];
                        for (int i = 0; i < threads.Length; i++)
                        {
                            threads[i] = new Thread(DispatchLoop);
                            threads[i].Start();
                        }
                    }
                }
            }
        }

        // Each thread runs the dispatch loop.
        private void DispatchLoop()
        {
            while (true)
            {
                WorkItem workItem = default(WorkItem);
                lock (workingQueue)
                {
                    // If shutdown was requested, exit the thread.
                    if (isNeedShutdown)
                        return;

                    // Find a new worker item to execute.
                    while (workingQueue.Count == 0)
                    {
                        threadsWaitingCount++;
                        try
                        {
                            Monitor.Wait(workingQueue);
                        }
                        finally
                        {
                            threadsWaitingCount--;
                        }

                        // If we were signaled due to shutdown, exit the thread.
                        if (isNeedShutdown)
                            return;
                    }

                    // We found a worker item! Grab it ...
                    workItem = workingQueue.Dequeue();
                }
                
                try
                {
                    workItem.Invoke();
                }
                finally
                {
                    workItem.InvokeOnFinishMethod();
                }
                
            }
        }
    }
}
