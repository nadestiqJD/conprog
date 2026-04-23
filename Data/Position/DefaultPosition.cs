using Data.Vector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Position
{
    public class DefaultPosition : IPosition
    {
        public double X { get; set; }
        public double Y { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IPosition position))
            {
                return false;
            }
            return position.X == X && position.Y == Y;
        }

        public override String ToString()
        {
            return $"X: {X}\tY: {Y}";
        }
    }
}
