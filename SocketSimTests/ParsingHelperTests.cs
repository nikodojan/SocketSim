using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocketSim.Helpers;
using SocketSim.Exceptions;

namespace SocketSimTests
{
    [TestClass]
    public class ParsingHelperTests
    {
        [DataTestMethod]
        [DataRow("127.0.0.1")]
        [DataRow("0.0.0.0")]
        [DataRow("255.255.255.255")]
        public void TryParse_ValidData_Test(string ip)
        {
            string port = "1234";
            IPEndPoint expected = new IPEndPoint(IPAddress.Parse(ip), 1234);

            var actual = ParsingHelper.TryParseEndpoint(ip, port);
            
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.Port, actual.Port);
        }

        [TestMethod]
        public void TryParse_Localhost_Test()
        {
            string port = "1234";
            string ip = "localhost";
            IPEndPoint expected = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234);

            var actual = ParsingHelper.TryParseEndpoint(ip, port);

            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.Port, actual.Port);
        }

        [TestMethod]
        public void TryParse_InvalidIp_Test()
        {
            string ip = "127.0.0.A";
            string port = "1234";

            Assert.ThrowsException<EndPointParserException>(() => ParsingHelper.TryParseEndpoint(ip, port));
        }

        [TestMethod]
        public void TryParse_InvalidPort_Test()
        {
            string ip = "127.0.0.1";
            string port = "abcd";
            
            Assert.ThrowsException<EndPointParserException>(() => ParsingHelper.TryParseEndpoint(ip, port));
        }

        [TestMethod]
        public void TryParse_InvalidPortAndIp_Test()
        {
            string ip = "127.0.0.A";
            string port = "abcd";

            Assert.ThrowsException<EndPointParserException>(() => ParsingHelper.TryParseEndpoint(ip, port));
        }

        [DataTestMethod]
        [DataRow("-1")]
        [DataRow("65536")]
        public void TryParse_PortOutOfRange_Test(string testPort)
        {
            string ip = "127.0.0.1";
            string port = testPort;

            Assert.ThrowsException<EndPointParserException>(() => ParsingHelper.TryParseEndpoint(ip, port));
        }
    }
}