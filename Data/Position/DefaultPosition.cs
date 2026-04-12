using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Position
{
    public class DefaultPosition : IPosition
    {
        public double X { get; set; }
        public double Y { get; set; }

        public override String ToString()
        {
            return $"X: {X}\tY: {Y}";
        }
    }
}
