using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest.Models
{
    public class Endpoint
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }

        public Endpoint()
        {
            
        }

        public Endpoint(string ipAddress, int port)
        {
            IPAddress = ipAddress;
            Port = port;
        }
    }
}
