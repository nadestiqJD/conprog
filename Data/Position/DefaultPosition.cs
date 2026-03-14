using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Position
{
    public class DefaultPosition : IPosition
    {
        public float X { get; set; }
        public float Y { get; set; }

        public override String ToString()
        {
            return $"X: {X}\tY: {Y}";
        }
    }
}
