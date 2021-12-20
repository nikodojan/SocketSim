using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SocketSim.StaticLogs;

namespace SocketSim.Sockets
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
            try
            {
                _listener = new UdpClient();
                _listener.Client.Bind(endPoint);
                //Task.Run(() => ListenToUdp());
                await ListenToUdp();
            }
            catch (Exception e)
            {
                MessageBox.Show("StartListening method: "+ e.Message);
            }
        }

        private async Task ListenToUdp()
        {
            try
            {
                await UdpLog.AddRecordAsync("UDP listener started\r\n");
                LogChanged?.Invoke(this, EventArgs.Empty);

                while (true)
                {
                    IPEndPoint from = null;
                    //byte[] receivedData = await _listener.ReceiveAsync();
                    var datagram = await _listener.ReceiveAsync();
                    
                    //string message = Encoding.UTF8.GetString(receivedData);
                    string message = Encoding.UTF8.GetString(datagram.Buffer);
                    from = datagram.RemoteEndPoint;

                    string logString = $"{DateTime.Now} \r\n" +
                                       $"Received from: {from.Address}:{from.Port} \r\n" +
                                       $"{message}\r\n";
                    await UdpLog.AddRecordAsync(logString);
                    LogChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ListenToUpd method: " + e.Message);
                _listener?.Client?.Close();
                _listener?.Dispose();
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
                await UdpLog.AddRecordAsync("UDP listener stopped\r\n");
                LogChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
