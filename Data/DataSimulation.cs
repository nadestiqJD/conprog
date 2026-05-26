using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Data
{
    public class DataSimulation : IDataSimulation
    {
        readonly Random random = new Random();

        private static readonly int _radius = 10;
        private static readonly double _velocity = _radius * 0.4;

        public DataSimulation()
        {
        }

        public IBall CreateBallInBoard(IBoard board)
        {
            
            IBall ball = new AngleBall
            {
                CurrentPosition = new DefaultPosition
                {
                    X = random.Next(0 + 2 * _radius, board.Width - 2 * _radius),
                    Y = random.Next(0 + 2 * _radius, board.Height - 2 * _radius)
                },
                Vector = new AngleVector(random.Next(5, 11) / 10.0 * _velocity, random.Next(0, 361)),
                Radius = _radius,
                Weight = random.Next(5, 11)
            };
            AddBallToBoard(board, ball);

            return ball;
        }

        public void RemoveBallFromBoard(IBoard board, IBall ball)
        {
            board.Balls.Remove(ball);
            ball.Board = null;

        }

        public void AddBallToBoard(IBoard board, IBall ball)
        {
            board.Balls.Add(ball);
            ball.Board = board;

        }

        public void DisposeBoard(IBoard board)
        {
            foreach (var ball in board.Balls)
            {
                ball.Board = null;
            }
            board.Balls.Clear();
        }

        public IBoard CreateBoard(int width, int height)
        {
            int minDimension = 10;
            if (width < minDimension || height < minDimension)
                throw new ArgumentException($"Board cannot be smaller than {minDimension}x{minDimension} in size");

            return new DefaultBoard(width, height);
        }
    }
}
