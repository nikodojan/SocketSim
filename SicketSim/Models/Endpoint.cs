namespace SocketSim.Models
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
