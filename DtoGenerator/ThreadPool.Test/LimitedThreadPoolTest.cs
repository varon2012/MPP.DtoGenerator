using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ThreadPool.Test
{
    [TestClass]
    public sealed class LimitedThreadPoolTest
    {
        [TestMethod]
        public void Test()
        {
            const int defaultDelayTime = 1000;
            const int delayMultiplier = 5;
            const int maxProcessingTaskCount = 3;
            const int overallTaskCount = 5;
            const int parameterizedCallback = 1;
            var tasks = new WaitCallback[]
            {
                data =>
                {

                    Thread.Sleep(defaultDelayTime);
                },
                data =>
                {
                    Thread.Sleep(defaultDelayTime * (int)data);
                }
            };

            using (var threadPool = new LimitedThreadPool(maxProcessingTaskCount))
            {
                var random = new Random();

                for (var i = 0; i < overallTaskCount; i++)
                {
                    var taskIndex = random.Next(tasks.Length);
                    var taskInfo = new TaskInfo
                    {
                        WaitCallback = tasks[taskIndex]
                    };

                    if (taskIndex == parameterizedCallback)
                    {
                        taskInfo.Data = random.Next(delayMultiplier);
                    }

                    threadPool.AddToProcessingQueue(taskInfo);
                }

                threadPool.Countdown.Wait();
            }
        }
    }
}
