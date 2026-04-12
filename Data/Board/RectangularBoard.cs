using Data.Ball;
using System;
using System.Collections.ObjectModel;

namespace Data.Board
{
    public class RectangularBoard : IBoard
    {
        public int Width { get; set; } = 750;

        public int Height { get; set; } = 550;

        public event EventHandler? OnMoveBalls;

        public void AddBall(IBall ball)
        {
            OnMoveBalls += ball.Move;
        }

        public virtual void MoveBalls()
        {
            OnMoveBalls?.Invoke(this, null);
        }

        public void StopBall(IBall ball)
        {
            OnMoveBalls -= ball.Move;
        }
    }
}
