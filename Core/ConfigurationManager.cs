using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace NexusShadow.Core
{
    public class ConfigurationManager
    {
        private JObject config;

        public ConfigurationManager()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                string configContent = File.ReadAllText(configPath);
                config = JObject.Parse(configContent);
            }
            else
            {
                config = new JObject();
            }
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            if (config.ContainsKey(key))
            {
                return config[key].ToObject<T>();
            }
            return defaultValue;
        }
    }
}