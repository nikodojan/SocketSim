using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest.StaticLogs
{
    public class UdpLog
    {
        public static List<string> Log { get; private set; } = new List<string>() { "" };

        public static async Task AddRecordAsync(string record)
        {
            Log.Add(record + "\r\n");
        }

        public static async Task Reset()
        {
            Log = new List<string>() { "" };
        }
    }
}
