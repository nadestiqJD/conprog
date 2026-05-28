using Data.Ball;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Data.Board
{
    public class DefaultBoard : IBoard
    {
        private int _width;
        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                RaiseDimensionsChanged();
            }
        }


        private int _height;
        public int Height { 
            get => _height; 
            set
            {
                _height = value;
                RaiseDimensionsChanged();
            }
        }

        public DefaultBoard(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public event EventHandler? DimensionsChanged;

        private void RaiseDimensionsChanged()
        {
            DimensionsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            foreach (var ball in Balls)
            {
                ball.Board = null;
            }
            Balls.Clear();
        }

        public List<IBall>Balls { get; } = new List<IBall>();
    }
}
