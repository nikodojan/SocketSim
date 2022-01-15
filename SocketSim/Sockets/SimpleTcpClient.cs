using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        public void Connect()
        {

        }

        public void Disconnect()
        {

        }


    }
}
