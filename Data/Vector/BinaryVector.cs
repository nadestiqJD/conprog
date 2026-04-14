using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Vector
{
    // Deprecated
    public class BinaryVector : IVector
    {
        private void deltaValidator(int x)
        {
            if (x < -1 || x > 1)
            {
                throw new ArgumentException("Value needs to be 1 or 0.");
            }
        }

        private int deltaX;
        public int DeltaX 
        {
            get => deltaX; 
            set
            {
                deltaValidator(value);
                deltaX = value;
            }
        }

        private int deltaY;
        public int DeltaY 
        {
            get => deltaY; 
            set
            {
                deltaValidator(value);
                deltaY = value;
            }
        }

        public IPosition AddToPosition(IPosition oldPosition)
        {
            return new DefaultPosition
            {
                X = oldPosition.X + DeltaX,
                Y = oldPosition.Y + DeltaY,
            };
        }

        public override string ToString()
        {
            return $"Delta X: {DeltaX}\tDelta Y: {DeltaY}";
        }
    }
}
