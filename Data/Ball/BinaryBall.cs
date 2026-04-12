using Data.Board;
using Data.Vector;
using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Ball
{
    public class BinaryBall : IBall
    {
        readonly Random random = new Random();

        public BinaryBall()
        {
            Radius = 1;
            CurrentPosition = new DefaultPosition();
            MovementVector = new BinaryVector { DeltaX = random.Next(0, 2), DeltaY = random.Next(0, 2) };
        }

        public BinaryBall(int radius) : this()
        {
            Radius = radius;
        }

        public BinaryBall(IPosition position) : this()
        {
            CurrentPosition = position;
        }

        public BinaryBall(IPosition position, IVector vector) : this(position)
        {
            MovementVector = vector;
        }

        public int Radius { get; }

        public IPosition CurrentPosition { get; private set; }

        public IVector MovementVector { get; private set; }

        public void Move(object sender, EventArgs e)
        {
            IBoard board = (IBoard) sender;
            IPosition newPosition = MovementVector.AddToPosition(CurrentPosition);
            if (newPosition.X - Radius < 0 || newPosition.Y - Radius < 0 
                || newPosition.X + Radius > board.Width || newPosition.Y + Radius > board.Height)
            {
                board.StopBall(this);
            }
            else
            {
                CurrentPosition = newPosition;
            }
        }
    }
}
