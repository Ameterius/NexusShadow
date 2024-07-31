using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using static NexusShadow.Core.Subcore.Logging;

namespace NexusShadow.Core.Subcore
{
    public static class XMLConfig
    {
        private static Dictionary<string, object> configData = new Dictionary<string, object>();
        private static string configFilePath;

        public static void Initialize(string filePath)
        {
            configFilePath = filePath;
            if (File.Exists(configFilePath))
            {
                using (FileStream fileStream = new FileStream(configFilePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, object>));
                    configData = (Dictionary<string, object>)serializer.Deserialize(fileStream);
                }
            }
        }

        public static bool Save()
        {
            try
            {
                using (FileStream fileStream = new FileStream(configFilePath, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, object>));
                    serializer.Serialize(fileStream, configData);
                }
                return true;
            }
            catch (Exception e)
            {
                LogError($"Error saving XML config: {e.Message}");
                return false;
            }
        }

        public static T GetValue<T>(string key, T defaultValue = default)
        {
            if (configData.TryGetValue(key, out object value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            return defaultValue;
        }

        public static void SetValue(string key, object value)
        {
            configData[key] = value;
        }
    }
}