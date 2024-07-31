using System;
using System.Collections.Generic;

namespace NexusShadow.Core
{
    public class CommandRegistry
    {
        private Dictionary<string, Action<string[]>> commands = new Dictionary<string, Action<string[]>>();

        public void RegisterCommand(string command, Action<string[]> action)
        {
            commands[command] = action;
        }

        public bool ExecuteCommand(string command, string[] args)
        {
            if (commands.ContainsKey(command))
            {
                commands[command](args);
                return true;
            }
            return false;
        }

        public IEnumerable<string> GetCommands()
        {
            return commands.Keys;
        }
    }
}