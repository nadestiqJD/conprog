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

        private void DumpLogQueue(Object state)
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
                Console.WriteLine($"Failed to write logs to file: {ex.Message}");
            }
        }

        private readonly Timer dumpLogQueueTimer;
        
        public FileLogger() : this("log")
        {
        }
        public FileLogger(string logDirectory)
        {
            Directory.CreateDirectory(logDirectory);
            
            _logFilePath = Path.Combine(logDirectory, $"log{Directory.GetFiles(logDirectory).Length}.txt");
            dumpLogQueueTimer = new Timer(DumpLogQueue, null, 0, 1000);
        }

        public async Task LogDebug(string message) => _logQueue.Add(FormatLogMessage(message, "debug"));
        public async Task LogInfo(string message) => _logQueue.Add(FormatLogMessage(message, "info"));
        public async Task LogTrace(string message) => _logQueue.Add(FormatLogMessage(message, "trace"));
        public async Task LogWarning(string message) => _logQueue.Add(FormatLogMessage(message, "warning"));
        public async Task LogError(string message) => _logQueue.Add(FormatLogMessage(message, "error"));

        public async Task Log(string message) => await LogInfo(message);

        private string FormatLogMessage(string message, string level)
        {
            return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {level.ToUpper()}: {message}";
        }
    }
}
