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

        // TODO: Remove this method
        void MoveBalls();

        void AddBall(IBall ball);

        // TODO: Rename to RemoveBall
        void StopBall(IBall ball);
    }
}
