using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Vector
{
    public class AngleVector : IVector
    {
        public double Velocity { get; private set; }
        public int Angle { get; private set; }
        public IPosition AddToPosition(IPosition oldPosition)
        {
            double rad = Angle * (Math.PI / 180.0);
            return new DefaultPosition {X = oldPosition.X + Math.Cos(rad) * Velocity, Y = oldPosition.Y + Math.Sin(rad) * Velocity}; 
        }

        public AngleVector(double velocity, int angle)
        {
            Velocity = velocity;
            Angle = angle;
        }

        public IPosition GetDelta()
        {
            double rad = Angle * (Math.PI / 180.0);
            return new DefaultPosition
            {
                X = Math.Cos(rad) * Velocity,
                Y = Math.Sin(rad) * Velocity
            };
        }
    }
}
