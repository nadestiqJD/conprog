using Data.Ball;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationLogger
{
    public interface IApplicationLogger
    {
        Task LogCollision(IBall ball1, IBall ball2);
        Task LogPosition(IBall ball);
        Task Log(string message);
    }
}
