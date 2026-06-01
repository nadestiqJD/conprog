using Data.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataLogger
{
    public class DummyLogger : ILogger
    {
        public void Log(string message)
        {
        }

        public void Log(string message, LogLevel level)
        {
        }

        public async Task LogAsync(string message)
        {
        }

        public async Task LogAsync(string message, LogLevel level)
        {
        }

        public void LogDebug(string message)
        {
        }

        public async Task LogDebugAsync(string message)
        {
        }

        public void LogError(string message)
        {
        }

        public async Task LogErrorAsync(string message)
        {
        }

        public void LogInfo(string message)
        {
        }

        public async Task LogInfoAsync(string message)
        {
        }

        public void LogTrace(string message)
        {
        }

        public async Task LogTraceAsync(string message)
        {
        }

        public void LogWarning(string message)
        {
        }

        public async Task LogWarningAsync(string message)
        {
        }
    }
}
