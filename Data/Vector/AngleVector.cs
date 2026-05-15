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

        /// <summary>
        /// Get AngleVector based on changes in X and Y directions.
        /// </summary>
        /// <param name="deltaVector"><see cref="IPosition"/> object representing X and Y values of delta vector</param>
        /// <returns></returns>
        public static AngleVector GetFromDelta(IPosition deltaVector)
        {
            double newLength = Math.Sqrt(deltaVector.X * deltaVector.X + deltaVector.Y * deltaVector.Y);

            double radians = Math.Atan2(deltaVector.Y, deltaVector.X);
            double degrees = radians * (180.0 / Math.PI);

            int angle = ((int)Math.Round(degrees)) % 360;
            if (angle < 0) angle += 360;

            return new AngleVector { Length = newLength, Angle = angle };
        }

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
