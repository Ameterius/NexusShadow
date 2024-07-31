using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using static NexusShadow.Core.Subcore.Logging;

namespace NexusShadow.Core.Subcore
{
    public static class QueueHelper
    {
        private static readonly ConcurrentQueue<Action> taskQueue = new ConcurrentQueue<Action>();

        public static void EnqueueTask(Action task)
        {
            taskQueue.Enqueue(task);
        }

        public static async Task ProcessQueueAsync()
        {
            while (taskQueue.TryDequeue(out Action task))
            {
                try
                {
                    await Task.Run(task);
                }
                catch (Exception e)
                {
                    LogError($"Error processing task: {e.Message}");
                }
            }
        }
    }
}