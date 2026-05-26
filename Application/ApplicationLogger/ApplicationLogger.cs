using Data.Ball;
using Data.Logger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationLogger
{
    public class ApplicationLogger : IApplicationLogger
    {
        private readonly ILogger _logger;
        private readonly BlockingCollection<string> _logQueue;

        public ApplicationLogger(ILogger logger)
        {
            _logger = logger;
            _logQueue = new BlockingCollection<string>();
        }

        public async Task LogCollision(IBall ball1, IBall ball2)
        {
            string message = _logger.GetMessageForContact(ball1, ball2);
            _logQueue.Add(message);
        }

        public async Task LogPosition(IBall ball)
        {
            string message = _logger.GetMessageForPosition(ball);
            _logQueue.Add(message);
        }

        public async Task Log(string message)
        {
            message = _logger.FormatMessage(message);
            _logQueue.Add(message);
        }
    }
}
