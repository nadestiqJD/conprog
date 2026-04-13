using Data.Ball;
using System;
using System.Collections.ObjectModel;

namespace Data.Board
{
    public class EventBoard : IBoard
    {
        private int _width = 750;
        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                RaiseDimensionsChanged();
            }
        }


        private int _height = 550;
        public int Height { 
            get => _height; 
            set
            {
                _height = value;
                RaiseDimensionsChanged();
            }
        }

        public event EventHandler? DimensionsChanged;

        private void RaiseDimensionsChanged()
        {
            DimensionsChanged?.Invoke(this, EventArgs.Empty);
        }

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
