using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DataSimulation> _logger;

        public DataSimulation(ILogger<DataSimulation> logger)
        {
            _logger = logger;
        }

        public IBall CreateBallInBoard(IBoard board)
        {
            IBall ball = new AngleBall(
                    new DefaultPosition
                    {
                        X = random.Next(0 + 2 * _radius, board.Width - 2 * _radius),
                        Y = random.Next(0 + 2 * _radius, board.Height - 2 * _radius)
                    },
                    new AngleVector(random.Next(5, 11) / 10.0 * _velocity, random.Next(0, 361)),
                    _radius
                );
            board.Balls.Add(ball);
            return ball;
        }
    }
}
