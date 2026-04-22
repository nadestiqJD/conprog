using Data.Ball;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Data.Board
{
    public class DefaultBoard : IBoard
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
        
        public List<IBall>Balls { get; } = new List<IBall>();
    }
}
