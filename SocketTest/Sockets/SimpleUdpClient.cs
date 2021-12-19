using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SocketTest.StaticLogs;

namespace SocketTest.Sockets
{
    public class SimpleUdpClient
    {
        public event EventHandler LogChanged;

        private UdpClient _listener;

        public async Task SendAsync(IPEndPoint endPoint, string message)
        {
            using UdpClient socket = new UdpClient();
            var data = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(data, data.Length, endPoint);

            string logString = $"{DateTime.Now} \r\n" +
                               $"Sent to: {endPoint.Address}:{endPoint.Port} \r\n" +
                               $"{message}\r\n";
            await UdpLog.AddRecordAsync(logString);
            LogChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task StartListening(IPEndPoint endPoint)
        {
            _listener = new UdpClient();
            _listener.Client.Bind(endPoint);
            Task.Run(() => ListenToUdp());
        }

        private async Task ListenToUdp()
        {
            await UdpLog.AddRecordAsync("Started\r\n");
            LogChanged?.Invoke(this, EventArgs.Empty);

            while (true)
            {
                IPEndPoint from = null;
                byte[] receivedData = _listener.Receive(ref from);
                string message = Encoding.UTF8.GetString(receivedData);
                string logString = $"{DateTime.Now} \r\n" +
                                   $"Received from: {from.Address}:{from.Port} \r\n" +
                                   $"{message}\r\n";
                await UdpLog.AddRecordAsync(logString);
                LogChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task StopListening()
        {
            try
            {
                _listener.Client.Close();
                _listener.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                await UdpLog.AddRecordAsync("Stopped\r\n");
                LogChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
