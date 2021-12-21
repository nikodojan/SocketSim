using System;
using System.Net;
using System.Windows;

namespace SocketSim.Helpers
{
    internal static class ParsingHelper
    {
        /// <summary>
        /// Parses the entered IP address and port to IPEndPoint.
        /// </summary>
        /// <param name="ipInput">IP addresses to be parsed</param>
        /// <param name="portInput">Port number to be parsed</param>
        /// <returns>An instance of an IPEndPoint with the entered parameters or <see langword="null"/> if the entered values are invalid. </returns>
        public static IPEndPoint ParseEndpoint(string ipInput, string portInput)
        {
            if (ipInput.ToLower() == "localhost")
            {
                ipInput = "127.0.0.1";
            }

            bool ipParsed = IPAddress.TryParse(ipInput, out IPAddress ip);
            if (!ipParsed)
                MessageBox.Show("IP Address has invalid format.", "Input error");


            bool portParsed = Int32.TryParse(portInput, out int port);
            if (!portParsed)
                MessageBox.Show("Port has invalid format.", "Input error");

            bool portIsValid = true;
            if (port < 0 || port > 65535)
            {
                MessageBox.Show("Invalid port number.\r\n" +
                                "The port number must be between 0 and 65535", "Input error");
                portIsValid = false;
            }

            if (ipParsed && portParsed && portIsValid)
            {
                //Endpoint ep = new Endpoint(serverIpTextBox.Text, port);
                IPEndPoint endPoint = new IPEndPoint(ip, port);
                return endPoint;
            }
            return null;
        }
    }
}
