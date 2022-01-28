using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketSim.Exceptions
{
    public class EndPointParserException : Exception
    {
        public EndPointParserException()
        {
        }

        public EndPointParserException(string message)
            : base(message)
        {
        }

        public EndPointParserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
