using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Vector
{
    public class AngleVector : IVector
    {
        public double Length { get; set; }
        public int Angle { get; set; }
        public IPosition AddToPosition(IPosition oldPosition)
        {
            double rad = Angle * (Math.PI / 180.0);
            return new DefaultPosition {X = oldPosition.X + Math.Cos(rad) * Length, Y = oldPosition.Y + Math.Sin(rad) * Length}; 
        }

        public AngleVector(double length, int angle)
        {
            Length = length;
            Angle = angle;
        }

        public AngleVector() { }

        public IPosition GetDelta()
        {
            double rad = Angle * (Math.PI / 180.0);
            return new DefaultPosition
            {
                X = Math.Cos(rad) * Length,
                Y = Math.Sin(rad) * Length
            };
        }
    }
}
