using System;
using System.Threading;
using System.Threading.Tasks;
using static NexusShadow.Core.Subcore.Logging;

namespace NexusShadow.Core.Subcore
{
    public static class ThreadingHelper
    {
        public static void RunInBackground(Action action)
        {
            try
            {
                Task.Run(action);
            }
            catch (Exception e)
            {
                LogError($"Threading error: {e.Message}");
            }
        }

        public static async Task RunInBackgroundAsync(Func<Task> asyncAction)
        {
            try
            {
                await Task.Run(asyncAction);
            }
            catch (Exception e)
            {
                LogError($"Threading error: {e.Message}");
            }
        }

        public static void Sleep(int milliseconds)
        {
            try
            {
                Thread.Sleep(milliseconds);
            }
            catch (Exception e)
            {
                LogError($"Threading error: {e.Message}");
            }
        }
    }
}