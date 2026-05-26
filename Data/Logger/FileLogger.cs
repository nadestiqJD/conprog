using Data.Ball;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Logger
{
    public class FileLogger : ILogger
    {
        public string FormatMessage(string message)
        {
            string timestampedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            return timestampedMessage;
        }

        public string GetMessageForPosition(IBall ball)
        {
            string message = $"Ball: {ball.Id} is at position: {ball.CurrentPosition}, Vector: {ball.Vector}";
            return FormatMessage(message);
        }

        public string GetMessageForContact(IBall ball1, IBall ball2)
        {
            string message = $"Ball: {ball1.Id} has contact with Ball: {ball2.Id}";
            return FormatMessage(message);
        }
    }
}
