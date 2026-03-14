using Data.Board;
using Data.Vector;
using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Ball
{
    public class VectorBall : IBall
    {
        readonly Random random = new Random();

        public VectorBall()
        {
            Radius = 1;
            CurrentPosition = new DefaultPosition();
            MovementVector = new IntegerVector { DeltaX = random.Next(1, 10), DeltaY = random.Next(1, 10) };
        }

        public VectorBall(int radius) : this()
        {
            Radius = radius;
        }

        public VectorBall(IPosition position) : this()
        {
            CurrentPosition = position;
        }

        public VectorBall(IPosition position, IVector vector) : this(position)
        {
            MovementVector = vector;
        }

        public int Radius { get; }

        public IPosition CurrentPosition { get; private set; }

        public IVector MovementVector { get; private set; }

        public void Move(object sender, EventArgs e)
        {
            if (!(sender is IRectangularBoard)) 
            {
                throw new ArgumentException("VectorBall requires Board to be a rectangle.");
            }

            IRectangularBoard board = (IRectangularBoard) sender;
            IPosition newPosition = MovementVector.addToPosition(CurrentPosition);
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
