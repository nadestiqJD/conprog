using Data.Ball;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Logger
{
    public interface ILogger
    {
        string FormatMessage(string message);
        string GetMessageForPosition(IBall ball);
        string GetMessageForContact(IBall ball1, IBall ball2);
    }
}
