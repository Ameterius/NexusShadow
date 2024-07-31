using System;

namespace NexusShadow.Core.Logging
{
    public class Logger
    {
        private static Logger instance;

        private Logger() { }

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"ERROR: {message}");
        }
    }
}