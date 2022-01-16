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
    /// <summary>
    /// This class contains function to access the TCPClient.
    /// </summary>
    public class SimpleTcpClient
    {
        private StreamReader _reader;
        private StreamWriter _writer;
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private IPEndPoint _remoteIPEndPoint;

        private bool keepListening = true;

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

                while (keepListening)
                {
                    var incoming = await _reader.ReadLineAsync();
                    if (incoming is not null)
                    {
                        await LogEventAsync($"S: {incoming}");
                    }
                }

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
                
            }
        }

        public async Task Disconnect()
        {
            try
            {
                if (_tcpClient is not null)
                {
                    _reader?.Close();
                    _reader?.Dispose();
                    await _writer.DisposeAsync();
                    await _stream.DisposeAsync();
                    _tcpClient?.Close();
                }
                
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
