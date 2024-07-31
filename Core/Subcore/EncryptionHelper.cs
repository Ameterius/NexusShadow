using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static NexusShadow.Core.Subcore.Logging;

namespace NexusShadow.Core.Subcore
{
    public static class EncryptionHelper
    {
        private static readonly Dictionary<int, EncryptionConfig> encryptionConfigs = new Dictionary<int, EncryptionConfig>();

        public static bool Bind(int id, string key, string iv)
        {
            try
            {
                if (encryptionConfigs.ContainsKey(id))
                {
                    LogError($"Encryption configuration with ID {id} already exists.");
                    return false;
                }

                encryptionConfigs[id] = new EncryptionConfig
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    IV = Encoding.UTF8.GetBytes(iv)
                };

                return true;
            }
            catch (Exception e)
            {
                LogError($"Encryption bind error: {e.Message}");
                return false;
            }
        }

        public static bool Unbind(int id)
        {
            try
            {
                return encryptionConfigs.Remove(id);
            }
            catch (Exception e)
            {
                LogError($"Encryption unbind error: {e.Message}");
                return false;
            }
        }

        public static string Encrypt(int id, string plainText)
        {
            try
            {
                if (!encryptionConfigs.TryGetValue(id, out EncryptionConfig config))
                {
                    LogError($"Encryption configuration with ID {id} not found.");
                    return null;
                }

                using (Aes aes = Aes.Create())
                {
                    aes.Key = config.Key;
                    aes.IV = config.IV;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogError($"Encryption error: {e.Message}");
                return null;
            }
        }

        public static string Decrypt(int id, string cipherText)
        {
            try
            {
                if (!encryptionConfigs.TryGetValue(id, out EncryptionConfig config))
                {
                    LogError($"Encryption configuration with ID {id} not found.");
                    return null;
                }

                using (Aes aes = Aes.Create())
                {
                    aes.Key = config.Key;
                    aes.IV = config.IV;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogError($"Decryption error: {e.Message}");
                return null;
            }
        }

        private class EncryptionConfig
        {
            public byte[] Key { get; set; }
            public byte[] IV { get; set; }
        }
    }
}