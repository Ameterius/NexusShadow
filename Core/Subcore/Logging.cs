using System;


// Simple methods for logging
// Use: using static NexusShadow.Core.Subcore.Logging;
namespace NexusShadow.Core.Subcore
{
    public static class Logging
    {
        public static void LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        public static void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }
    }
}