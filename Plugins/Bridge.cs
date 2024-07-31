using System;
using NexusShadow.Core;

namespace NexusShadow.Plugins
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        string Author { get; }
        void Execute();
        void RegisterCommands(CommandRegistry registry);
    }
}