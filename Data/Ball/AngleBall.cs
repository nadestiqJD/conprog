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
        readonly Random random = new Random();

        public event EventHandler? PositionChanged;
        private void RaisePositionChanged()
        {
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        public IPosition CurrentPosition { get; set; }

        public int Radius { get; }

        private IVector Vector { get; set; }

        public void Move(object? sender, EventArgs e)
        {
            IBoard board = (IBoard) sender;
            IPosition newPosition = Vector.AddToPosition(CurrentPosition);

            double validatedX, validatedY;
            if (newPosition.X + Radius > board.Width) {
                validatedX = board.Width - Radius;
                board.StopBall(this);
            } else if (newPosition.X - Radius < 0) {
                validatedX = Radius;
                board.StopBall(this);
            } else {
                validatedX = newPosition.X;
            }

            if (newPosition.Y + Radius > board.Height) {
                validatedY = board.Height - Radius;
                board.StopBall(this);
            } else if (newPosition.Y - Radius < 0) {
                validatedY= Radius;
                board.StopBall(this);
            } else {
                validatedY = newPosition.Y;
            }

            CurrentPosition = new DefaultPosition { X = validatedX, Y = validatedY };
            RaisePositionChanged();
        }

        public AngleBall()
        {
            Radius = 10;
            CurrentPosition = new DefaultPosition {X = random.Next(), Y = random.Next() };
            Vector = new AngleVector(random.NextDouble(), random.Next());
        }
    }



}
