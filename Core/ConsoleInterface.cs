using System;
using NexusShadow.Core;

namespace NexusShadow.Core
{
    public class ConsoleInterface
    {
        private PluginManager pluginManager;
        private ConfigurationManager configManager;
        private bool manualPluginLoad;
        private CommandRegistry commandRegistry;

        public ConsoleInterface()
        {
            pluginManager = new PluginManager();
            configManager = new ConfigurationManager();
            manualPluginLoad = configManager.GetValue("ManualPluginLoad", false);
            commandRegistry = pluginManager.GetCommandRegistry();
        }

        public void Run()
        {
            Console.Title = "NexusShadow | Good night good luck";
            Console.WriteLine("NexusShadow Platform [Version 1.5 | .NET 4.8.1]");
            Console.WriteLine("(c) Ametero (aka CanaryGen). All rights reserved.\n");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                ParseCommand(input);
            }
        }

        private void ParseCommand(string command)
        {
            string[] parts = command.Split(' ');
            string commandName = parts[0].ToLower();
            string[] args = new string[parts.Length - 1];
            Array.Copy(parts, 1, args, 0, args.Length);

            if (commandRegistry.ExecuteCommand(commandName, args))
            {
                return;
            }

            switch (commandName)
            {
                case "plugin":
                    if (manualPluginLoad)
                    {
                        if (parts.Length > 1)
                        {
                            if (parts[1].ToLower() == "load")
                            {
                                pluginManager.LoadPlugin(parts[2]);
                            }
                            else if (parts[1].ToLower() == "run")
                            {
                                pluginManager.ExecutePlugin(parts[2]);
                            }
                            else if (parts[1].ToLower() == "list")
                            {
                                pluginManager.ListPlugins();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Plugin commands are not available in automatic plugin load mode.");
                    }
                    break;
                case "help":
                    ShowHelp();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                    break;
            }
        }

        private void ShowHelp()
        {
            Console.WriteLine("Available commands:");
            if (manualPluginLoad)
            {
                Console.WriteLine("  plugin load <plugin> - Load a plugin");
                Console.WriteLine("  plugin run <plugin>  - Run a loaded plugin");
                Console.WriteLine("  plugin list          - List all loaded plugins");
            }
            foreach (var cmd in commandRegistry.GetCommands())
            {
                Console.WriteLine($"  {cmd} - {GetCommandDescription(cmd)}");
            }
            Console.WriteLine("  help                 - Show this help message");
            Console.WriteLine("  exit                 - Exit the application");
        }

        private string GetCommandDescription(string command)
        {
            // Placeholder for command descriptions
            return "Description not available";
        }
    }
}