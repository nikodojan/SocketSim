using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SocketTest.Models;
using SocketTest.TcpServerFiles;

namespace SocketTest
{
    public class MainViewModel
    {
        private SimpleTcpServer _server;

        public event EventHandler LogChanged;

        public SimpleTcpServer Server
        {
            get => _server;
            private set => _server = value;
        }

        public async Task StartTcpServer(Endpoint ep)
        {
            _server = new SimpleTcpServer(ep);
            _server.LogChanged += OnLogChanged;

            await _server.StartListener();

        }

        public async Task StopTcpServer()
        {
            await _server.StopListener();
        }

        public void OnLogChanged(object sender, EventArgs e)
        {

        }
    }
}
