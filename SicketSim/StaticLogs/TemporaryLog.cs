using System.Collections.Generic;

namespace SocketSim.StaticLogs
{
    public class TemporaryLog
    {
        private List<string> _log = new List<string>();

        public List<string> GetLog
        {
            get => _log;
        }

        public void AppendLine(string newLine)
        {
            _log.Add(newLine);
        }

        public void Reset()
        {
            _log = new List<string>();
        }
    }
}
