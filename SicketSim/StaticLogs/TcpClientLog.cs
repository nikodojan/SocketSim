using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketSim.StaticLogs
{
    public class TcpClientLog
    { public static List<string> Log { get; private set; } = new List<string>() { "" };

        /// <summary>
        /// Add a new record to the client log.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static async Task AddRecordAsync(string record)
        {
            await Task.Run(() => Log.Add(record));
        }

        public static async Task ResetAsync()
        {
            await Task.Run(() => Log = new List<string>() { "" });
        }
    }
}
