using Data.DataLogger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        private readonly BlockingCollection<string> _logQueue = new BlockingCollection<string>();

        private readonly CancellationToken _cancellationToken;

        private readonly LogLevel _minimumLogLevel = LogLevel.TRACE;

        private readonly LogLevel _defaultLogLevel = LogLevel.INFO;

        private async Task DumpLogQueue()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
                {
                    while (_logQueue.TryTake(out string logMessage))
                    {
                        writer.WriteLine(logMessage);
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task DumpLogQueueJob(CancellationToken ct) 
        {
            try
            {
                while (true)
                {
                    ct.ThrowIfCancellationRequested();
                    await DumpLogQueue();
                }
            }
            catch (OperationCanceledException ex)
            {
                await DumpLogQueue();
            }
        }
        
        public FileLogger() : this("log")
        {
        }
        public FileLogger(string logDirectory) : this(logDirectory, CancellationToken.None)
        {
        }

        public FileLogger(string logDirectory, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(logDirectory);

            _logFilePath = Path.Combine(logDirectory, $"log{Directory.GetFiles(logDirectory).Length}.txt");
            Task.Run(() => DumpLogQueueJob(_cancellationToken), cancellationToken: _cancellationToken);
        }

        public async Task LogDebugAsync(string message) => await Task.Run(() => LogDebug(message));
        public async Task LogInfoAsync(string message) => await Task.Run(() => LogInfo(message));
        public async Task LogTraceAsync(string message) => await Task.Run(() => LogTrace(message));
        public async Task LogWarningAsync(string message) => await Task.Run(() => LogWarning(message));
        public async Task LogErrorAsync(string message) => await Task.Run(() => LogError(message));

        public async Task LogAsync(string message) => await Task.Run(() => Log(message));
        public async Task LogAsync(string message, LogLevel level) => await Task.Run(() => Log(message, level));

        private string FormatLogMessage(string message, string level)
        {
            return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {level.ToUpper()}: {message}";
        }

        public void LogDebug(string message) 
        {
            if (LogLevel.DEBUG >= _minimumLogLevel)
            {
                _logQueue.Add(FormatLogMessage(message, LogLevel.DEBUG.ToString()));
            }
        }
        public void LogInfo(string message)
        {
            if (LogLevel.INFO >= _minimumLogLevel)
            {
                _logQueue.Add(FormatLogMessage(message, LogLevel.INFO.ToString()));
            }
        }
        public void LogTrace(string message)
        {
            if (LogLevel.TRACE >= _minimumLogLevel)
            {
                _logQueue.Add(FormatLogMessage(message, LogLevel.TRACE.ToString()));
            }
        }
        public void LogWarning(string message)
        {
            if (LogLevel.WARNING >= _minimumLogLevel)
            {
                _logQueue.Add(FormatLogMessage(message, LogLevel.WARNING.ToString()));
            }
        }
        public void LogError(string message)
        {
            if (LogLevel.ERROR >= _minimumLogLevel)
            {
                _logQueue.Add(FormatLogMessage(message, LogLevel.ERROR.ToString()));
            }
        }

        public void Log(string message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.ERROR: LogError(message); break;
                case LogLevel.WARNING: LogWarning(message); break;
                case LogLevel.INFO: LogInfo(message); break;
                case LogLevel.DEBUG: LogDebug(message); break;
                case LogLevel.TRACE: LogTrace(message); break;
            }
        }

        public void Log(string message) => Log(message, _defaultLogLevel);
    }
}
