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

        public IPosition CurrentPosition { get; set; }

        public int Radius { get; }

        private IVector Vector { get; set; }

        public void Move(object sender, EventArgs e)
        {
            IBoard board = (IBoard)sender;
            IPosition newPosition = Vector.AddToPosition(CurrentPosition);

            double validatedX; 
            double validatedY;
            if (newPosition.X + 2 * Radius > board.Width) {
                validatedX = board.Width - 2 * Radius;
                board.StopBall(this);
            } else if (newPosition.X < 0) {
                validatedX = 0;
                board.StopBall(this);
            } else {
                validatedX = newPosition.X;
            }

            if (newPosition.Y + 2 * Radius > board.Height) {
                validatedY = board.Height - 2 * Radius;
                board.StopBall(this);
            } else if (newPosition.Y < 0) {
                validatedY = 0;
                board.StopBall(this);
            } else {
                validatedY = newPosition.Y;
            }

            CurrentPosition = new DefaultPosition { X = validatedX, Y = validatedY };
            RaisePositionChanged();
        }

        public AngleBall(IPosition currentPosition, IVector vector, int radius)
        {
            Radius = radius;
            Vector = vector;
            CurrentPosition = currentPosition;
        }
    }



}
