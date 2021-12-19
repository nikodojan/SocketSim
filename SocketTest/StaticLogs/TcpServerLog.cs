using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTest.StaticLogs
{
    /// <summary>
    /// Static log class to temporarily hold the server output messages.
    /// </summary>
    public static class TcpServerLog
    {
        public static List<string> Log { get; private set; } = new List<string>() { "" };

        /// <summary>
        /// Add a new record to the server log.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static async Task AddRecordAsync(string record)
        {
            await Task.Run(()=>Log.Add(record));
        }

        public static async Task Reset()
        {
            await Task.Run(()=>Log = new List<string>() { "" });
        }
    }
}
