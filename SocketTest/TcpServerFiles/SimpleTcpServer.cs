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
    public class SimpleTcpServer
    {
        private Endpoint _endPoint;
        private StreamReader _reader;
        private TcpListener _listener;
        private TcpClient _tcpClient;
        private NetworkStream _stream;

        private bool _keepListening = true;
        
        public SimpleTcpServer(Endpoint endpoint)
        {
            _endPoint = endpoint;
        }

        public event EventHandler LogChanged;
        public event EventHandler ServerStarted;
        public event EventHandler ServerStopped;

        public async Task StartListener()
        {
            IPEndPoint localIPEndPoint = new IPEndPoint(IPAddress.Parse(_endPoint.IPAddress), _endPoint.Port);

            _listener = new TcpListener(localIPEndPoint);

            _listener.Start();
            ServerStarted?.Invoke(this, EventArgs.Empty);
            await LogEvent($"S: Server begins listening on {localIPEndPoint.ToString()}");

            try
            {
                _tcpClient = await _listener.AcceptTcpClientAsync();
                await LogEvent($"S: Client connected: {_tcpClient?.Client.RemoteEndPoint?.ToString()}");

                _stream = _tcpClient?.GetStream();
                _reader = new StreamReader(_stream);
                StreamWriter writer = new StreamWriter(_stream);

                while (_keepListening)
                {
                    var incoming = await _reader.ReadLineAsync();
                    await LogEvent($"C: {incoming}");
                }
                // TODO: send message
            }
            catch (SocketException se)
            {
                await LogEvent($"S: Error: 1 \r\n {se.Message}");
            }
            catch (Exception e)
            {
                await LogEvent($"S: Error: \r\n {e}");
            }
            finally
            {
                ServerStopped?.Invoke(this, EventArgs.Empty);
                _keepListening = false;
                _reader?.Close();
                _tcpClient?.Close();
                _listener?.Stop();
            }

        }

        public async Task StopListener()
        {
            _keepListening = false;
            _reader?.Close();
            _tcpClient?.Close();
            _listener?.Stop();
            
            ServerStopped?.Invoke(this, EventArgs.Empty);
            await LogEvent("S: Server stopped listening.");
        }

        private async Task LogEvent(string text)
        {
            await TcpServerLog.AddRecord($"{text}\r\n");
            LogChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
