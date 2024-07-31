using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using static NexusShadow.Core.Subcore.Logging;

// EMail utility with SMPT configurations binding

namespace NexusShadow.Core.Subcore
{
    public static class EmailUttil
    {
        private static readonly Dictionary<int, SmtpConfiguration> smtpConfigurations = new Dictionary<int, SmtpConfiguration>();

        public static bool SMTPBind(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword, int smtpID)
        {
            try
            {
                if (smtpConfigurations.ContainsKey(smtpID))
                {
                    LogError($"SMTP configuration with ID {smtpID} already exists.");
                    return false;
                }

                smtpConfigurations[smtpID] = new SmtpConfiguration
                {
                    SmtpServer = smtpServer,
                    SmtpPort = smtpPort,
                    SmtpUsername = smtpUsername,
                    SmtpPassword = smtpPassword
                };

                return true;
            }
            catch (Exception e)
            {
                LogError($"SMTP bind error: {e.Message}");
                return false;
            }
        }

        public static bool SMTPUnbind(int smtpID)
        {
            try
            {
                return smtpConfigurations.Remove(smtpID);
            }
            catch (Exception e)
            {
                LogError($"SMTP unbind error: {e.Message}");
                return false;
            }
        }

        public static bool SendEmail(int smtpID, string to, string subject, string body)
        {
            try
            {
                if (!smtpConfigurations.TryGetValue(smtpID, out SmtpConfiguration config))
                {
                    LogError($"SMTP configuration with ID {smtpID} not found.");
                    return false;
                }

                using (SmtpClient smtpClient = new SmtpClient(config.SmtpServer, config.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(config.SmtpUsername, config.SmtpPassword);
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(config.SmtpUsername),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(to);

                    smtpClient.Send(mailMessage);
                }
                return true;
            }
            catch (Exception e)
            {
                LogError($"Email error: {e.Message}");
                return false;
            }
        }

        private class SmtpConfiguration
        {
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; }
            public string SmtpUsername { get; set; }
            public string SmtpPassword { get; set; }
        }
    }
}