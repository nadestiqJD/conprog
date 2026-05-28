using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Logger
{
    public interface ILogger
    {
        Task LogInfo(string message);

        Task LogWarning(string message);

        Task LogTrace(string message);

        Task LogDebug(string message);

        Task LogError(string message);

        Task Log(string message);
    }
}
