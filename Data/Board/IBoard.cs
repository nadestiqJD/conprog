using Data.Ball;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Board
{
    public interface IBoard
    {
        int Width { get; set; }

        int Height { get; set; }

        event EventHandler DimensionsChanged;

        void MoveBalls();

        void AddBall(IBall ball);

        void StopBall(IBall ball);
    }
}
