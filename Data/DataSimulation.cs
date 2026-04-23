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
            AddBallToBoard(board, ball);

            _logger.LogTrace("Ball {} created in board {}", ball, board);
            return ball;
        }

        public void RemoveBallFromBoard(IBoard board, IBall ball)
        {
            board.Balls.Remove(ball);
            ball.Board = null;

            _logger.LogTrace("Ball {} removed from {}", ball, board);
        }

        public void AddBallToBoard(IBoard board, IBall ball)
        {
            board.Balls.Add(ball);
            ball.Board = board;

            _logger.LogTrace("Ball {} added to {}", ball, board);
        }

        public void DisposeBoard(IBoard board)
        {
            foreach (var ball in board.Balls)
            {
                ball.Board = null;
            }
            board.Balls.Clear();
            _logger.LogTrace("Board {} disposed", board);
        }
    }
}
