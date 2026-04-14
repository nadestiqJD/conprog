using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DataSimulation : IDataSimulation
    {
        readonly Random random = new Random();

        private static readonly int _radius = 10;
        private static readonly double _velocity = _radius * 0.8;

        public IBall CreateBallInBoard(IBoard board)
        {
            IBall ball = new AngleBall(
                    new DefaultPosition
                    {
                        X = random.Next(0 + _radius, board.Width - _radius),
                        Y = random.Next(0 + _radius, board.Height - _radius)
                    },
                    new AngleVector(random.Next(5, 11) / 10.0 * _velocity, random.Next(0, 361)),
                    _radius
                );
            board.AddBall(ball);
            return ball;
        }
    }
}
