using Data.Ball;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Board
{
    public interface IBoard
    {
        void MoveBalls();

        void AddBall(IBall ball);

        void StopBall(IBall ball);
    }
}
