using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SocketTest.Models;
using SocketTest.StaticLogs;

namespace SocketTest.TcpServerFiles
{
    /// <summary>
    /// Class that handles the Tcp server.
    /// </summary>
    public class SimpleTcpServer
    {
        private Endpoint _endPoint;
        private StreamReader _reader;
        private StreamWriter _writer;
        private TcpListener _listener;
        private TcpClient _tcpClient;
        private NetworkStream _stream;

        private bool _keepListening = true;
        
        public SimpleTcpServer(Endpoint endpoint)
        {
            _endPoint = endpoint;
        }

        /// <summary>
        /// Signalizes new log entries.
        /// </summary>
        public event EventHandler LogChanged;
        /// <summary>
        /// Is invoked when the server is started.
        /// </summary>
        public event EventHandler ServerStarted;
        /// <summary>
        /// Is invoked when the server is stopped.
        /// </summary>
        public event EventHandler ServerStopped;
        /// <summary>
        /// Is invoked when the TCP listener accepted a client and the stream is created.
        /// </summary>
        public event EventHandler ClientConnected;

        /// <summary>
        /// Starts a new instance of a TCP listener and accepts clients.
        /// Adds received messages to the log.
        /// </summary>
        /// <exception cref="SocketException"></exception>
        public async Task StartListener()
        {
            IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Parse(_endPoint.IPAddress), _endPoint.Port);

            try
            {
                _listener = new TcpListener(localIPEndPoint);

                _listener.Start();
                ServerStarted?.Invoke(this, EventArgs.Empty);
                await LogEventAsync($"S: Server begins listening on {localIPEndPoint.ToString()}");

                _tcpClient = await _listener.AcceptTcpClientAsync();

                _stream = _tcpClient.GetStream();
                _reader = new StreamReader(_stream);
                _writer = new StreamWriter(_stream);
                
                ClientConnected?.Invoke(this, EventArgs.Empty);
                await LogEventAsync($"S: Client connected: {_tcpClient.Client.RemoteEndPoint?.ToString()}");

                while (_keepListening)
                {
                    var incoming = await _reader.ReadLineAsync();
                    if (incoming is not null)
                    {
                        await LogEventAsync($"C: {incoming}");
                    }
                }

            }
            catch (SocketException se)
            {
                await StopListener();
            }
            catch (Exception e)
            {
                await StopListener();
                await LogEventAsync($"S: Exception: \r\n {e}");
            }
            finally
            {
                ServerStopped?.Invoke(this, EventArgs.Empty);
                await LogEventAsync("S: Server stopped listening.");
            }
        }

        public async Task SendMessage(string message)
        {
            if (_writer is not null)
            {
                await _writer.WriteLineAsync(message);
                await _writer.FlushAsync();
                await LogEventAsync($"S: {message}");
            }
        }

        public async Task StopListener()
        {
            _keepListening = false;
            if (_tcpClient is not null)
            {
                _reader?.Close();
                _reader?.Dispose();
                await _writer.DisposeAsync();
                await _stream.DisposeAsync();
                _tcpClient?.Close();
            }

            _listener?.Stop();

            //ServerStopped?.Invoke(this, EventArgs.Empty);
            //await LogEvent("S: Server stopped listening.");
        }

        /// <summary>
        /// Adds a new record to the log and invokes LogChanged event.
        /// </summary>
        /// <param name="text">The text to be logged</param>
        private async Task LogEventAsync(string text)
        {
            await TcpServerLog.AddRecordAsync($"{text}\r\n");
            LogChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
