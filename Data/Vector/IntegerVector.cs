using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Vector
{
    public class IntegerVector : IVector
    {
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }

        public IPosition addToPosition(IPosition oldPosition)
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
