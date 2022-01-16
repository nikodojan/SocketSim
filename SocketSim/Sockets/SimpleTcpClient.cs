using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SocketSim.StaticLogs;

namespace SocketSim.Sockets
{
    public class SimpleTcpClient
    {
        private StreamReader _reader;
        private StreamWriter _writer;
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private IPEndPoint _remoteIPEndPoint;

        public SimpleTcpClient( )
        {
            
        }

        /// <summary>
        /// Signalizes new log entries.
        /// </summary>
        public event EventHandler LogChanged;

        public async Task Connect(IPEndPoint endPoint)
        {
            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(endPoint);

                _stream = _tcpClient.GetStream();
                _reader = new StreamReader( _stream );
                _writer = new StreamWriter( _stream );

                await LogEventAsync($"C: Connected to {endPoint.ToString()}");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                
            }
        }

        public async Task Send(string message)
        {
            try
            {
                await _writer.WriteAsync(message);
                await _writer.FlushAsync();
                await LogEventAsync($"C: {message}");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task Disconnect()
        {
            try
            {
                _tcpClient.Close();
                await LogEventAsync($"C: Disconnected");
            }
            catch (Exception e)
            {
                
            }

        }

        private async Task LogEventAsync(string text)
        {
            await TcpClientLog.AddRecordAsync($"{text}\r\n");
            LogChanged?.Invoke(this, EventArgs.Empty);
        }


    }
}
