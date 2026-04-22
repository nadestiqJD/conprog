using Data.Board;
using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Ball
{
    public class AngleBall : IBall
    {
        public event EventHandler? PositionChanged;
        private void RaisePositionChanged()
        {
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        private IPosition _currentPosition;

        public IPosition CurrentPosition 
        {
            get => _currentPosition;
            set 
            { 
                _currentPosition = value;
                RaisePositionChanged();
            }
        }

        public int Radius { get; }

        public IVector Vector { get; set; }

        public AngleBall(IPosition currentPosition, IVector vector, int radius)
        {
            Radius = radius;
            Vector = vector;
            CurrentPosition = currentPosition;
        }
    }



}
