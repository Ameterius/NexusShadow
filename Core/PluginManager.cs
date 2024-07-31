using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NexusShadow.Core.Logging;
using static NexusShadow.Core.CommandRegistry;
using NexusShadow.Plugins;

namespace NexusShadow.Core
{
    public class PluginManager
    {
        private Dictionary<string, IPlugin> plugins;
        private ConfigurationManager configManager;
        private bool manualPluginLoad;
        private Logger logger;
        private CommandRegistry commandRegistry;

        public PluginManager()
        {
            plugins = new Dictionary<string, IPlugin>();
            configManager = new ConfigurationManager();
            manualPluginLoad = configManager.GetValue("ManualPluginLoad", false);
            logger = Logger.GetInstance();
            commandRegistry = new CommandRegistry();

            if (!manualPluginLoad)
            {
                LoadAllPlugins();
            }
        }

        private void LoadAllPlugins()
        {
            string pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (Directory.Exists(pluginsDir))
            {
                foreach (string pluginPath in Directory.GetFiles(pluginsDir, "*.dll"))
                {
                    LoadPlugin(Path.GetFileNameWithoutExtension(pluginPath));
                }
            }
        }

        public void LoadPlugin(string pluginName)
        {
            if (manualPluginLoad)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", $"{pluginName}.dll");
                if (File.Exists(path))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(path);
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            if (typeof(IPlugin).IsAssignableFrom(type))
                            {
                                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                                plugins[plugin.Name] = plugin;
                                plugin.RegisterCommands(commandRegistry);
                                logger.LogInfo($"Plugin '{plugin.Name}' loaded.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"Failed to load plugin '{pluginName}': {ex.Message}");
                    }
                }
                else
                {
                    logger.LogError($"Plugin '{pluginName}' not found.");
                }
            }
            else
            {
                logger.LogInfo("Manual plugin loading is disabled.");
            }
        }

        public void ExecutePlugin(string pluginName)
        {
            if (plugins.ContainsKey(pluginName))
            {
                try
                {
                    plugins[pluginName].Execute();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Failed to execute plugin '{pluginName}': {ex.Message}");
                }
            }
            else
            {
                logger.LogError($"Plugin '{pluginName}' not loaded.");
            }
        }

        public void ListPlugins()
        {
            foreach (var plugin in plugins)
            {
                logger.LogInfo($"{plugin.Key}: {plugin.Value.Description}");
            }
        }

        public CommandRegistry GetCommandRegistry()
        {
            return commandRegistry;
        }
    }
}