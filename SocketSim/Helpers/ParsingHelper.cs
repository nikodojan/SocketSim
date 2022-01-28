using System;
using System.Net;
using System.Windows;
using SocketSim.Exceptions;

namespace SocketSim.Helpers
{
    public static class ParsingHelper
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

        /// <summary>
        /// Parses the entered IP address and port from String to IPEndPoint.
        /// </summary>
        /// <param name="ipInput">'IP address' or 'localhost' in string format</param>
        /// <param name="portInput"></param>
        /// <param name="endPoint"></param>
        /// <returns>The parsed IP endpoint</returns>
        /// <exception cref="System.ArgumentException">When IP address or port can not be parsed, e.g. have invalid format; or when port is out of range.</exception>
        public static IPEndPoint TryParseEndpoint(string ipInput, string portInput)
        {
            if (ipInput.ToLower() == "localhost") ipInput = "127.0.0.1";
            
            if (!IPAddress.TryParse(ipInput, out IPAddress ip))
                throw new EndPointParserException("Entered IP Address has invalid format.");
            
            if (!Int32.TryParse(portInput, out int port))
                throw new EndPointParserException("Entered Port has invalid format.");

            if (port < 0 || port > 65535) 
                throw new EndPointParserException("Invalid port number.\r\n" +
                                                  "The port number must be between 0 and 65535");

            return new IPEndPoint(ip, port);
        }
    }
}
