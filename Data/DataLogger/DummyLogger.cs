using Data.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataLogger
{
    public class DummyLogger : ILogger
    {
        public async Task Log(string message)
        {
        }

        public async Task LogDebug(string message)
        {
        }

        public async Task LogError(string message)
        {
        }

        public async Task LogInfo(string message)
        {
        }

        public async Task LogTrace(string message)
        {
        }

        public async Task LogWarning(string message)
        {
        }
    }
}
