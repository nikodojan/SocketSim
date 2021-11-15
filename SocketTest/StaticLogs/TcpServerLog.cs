using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest.StaticLogs
{
    public static class TcpServerLog
    {
        public static List<string> Log { get; private set; } = new List<string>() { "" };



        public static async Task AddRecord(string record)
        {
            Log.Add(record);
        }

        public static async Task Reset()
        {
            Log = new List<string>() { "" };
        }
    }
}
