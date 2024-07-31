using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static NexusShadow.Core.Subcore.Logging; // NexusShadow Logging Methods

// Network Uttility

namespace NexusShadow.Core.Subcore
{
    public static class NetworkUtility
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> GetHttpContentAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                LogError($"Request error: {e.Message}");
                return null;
            }
        }

        public static async Task<string> PostHttpContentAsync(string url, string jsonContent)
        {
            try
            {
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                LogError($"Request error: {e.Message}");
                return null;
            }
        }

        public static async Task<string> PutHttpContentAsync(string url, string jsonContent)
        {
            try
            {
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                LogError($"Request error: {e.Message}");
                return null;
            }
        }

        public static async Task<bool> DeleteHttpContentAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException e)
            {
                LogError($"Request error: {e.Message}");
                return false;
            }
        }

        public static async Task<bool> DownloadFileAsync(string url, string filePath)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fileStream);
                }
                return true;
            }
            catch (Exception e)
            {
                LogError($"File download error: {e.Message}");
                return false;
            }
        }

        public static async Task<string> SendTcpMessageAsync(string ipAddress, int port, string message)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(ipAddress, port);
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        await stream.WriteAsync(data, 0, data.Length);

                        data = new byte[256];
                        int bytes = await stream.ReadAsync(data, 0, data.Length);
                        return Encoding.UTF8.GetString(data, 0, bytes);
                    }
                }
            }
            catch (Exception e)
            {
                LogError($"TCP error: {e.Message}");
                return null;
            }
        }

        public static async Task<string> SendUdpMessageAsync(string ipAddress, int port, string message)
        {
            try
            {
                using (UdpClient client = new UdpClient())
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await client.SendAsync(data, data.Length, endPoint);

                    UdpReceiveResult result = await client.ReceiveAsync();
                    return Encoding.UTF8.GetString(result.Buffer);
                }
            }
            catch (Exception e)
            {
                LogError($"UDP error: {e.Message}");
                return null;
            }
        }
    }
}