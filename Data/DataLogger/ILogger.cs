using Data.DataLogger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Logger
{
    public interface ILogger
    {
        Task LogInfoAsync(string message);
        void LogInfo(string message);

        Task LogWarningAsync(string message);
        void LogWarning(string message);

        Task LogTraceAsync(string message);
        void LogTrace(string message);

        Task LogDebugAsync(string message);
        void LogDebug(string message);

        Task LogErrorAsync(string message);
        void LogError(string message);


        Task LogAsync(string message);
        Task LogAsync(string message, LogLevel level);

        void Log(string message);
        void Log(string message, LogLevel level);
    }
}
