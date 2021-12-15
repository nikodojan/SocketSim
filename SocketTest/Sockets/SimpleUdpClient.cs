using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SocketTest.StaticLogs;

namespace SocketTest.Sockets
{
    public class SimpleUdpClient
    {
        public event EventHandler LogChanged;

        public async Task SendAsync(IPEndPoint endPoint, string message)
        {
            using UdpClient socket = new UdpClient();
            var data = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(data, data.Length, endPoint);

            string logString = $"{DateTime.Now} \r\n" +
                               $"Sent to: {endPoint.Address}:{endPoint.Port} \r\n" +
                               $"{message}";
            await UdpLog.AddRecordAsync(logString);
            LogChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task ListenToUdp(IPEndPoint endPoint)
        {
            using UdpClient socket = new UdpClient();
            socket.Client.Bind(endPoint);

            while (true)
            {
                IPEndPoint from = null;
                byte[] receivedData = socket.Receive(ref from);
                string message = Encoding.UTF8.GetString(receivedData);
                string logString = $"{DateTime.Now} \r\n" +
                                   $"Received from: {from.Address}:{from.Port} \r\n" +
                                   $"{message}";
                await UdpLog.AddRecordAsync(logString);
                LogChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
