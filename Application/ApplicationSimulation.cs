using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Application
{
    public class ApplicationSimulation : IApplicationSimulation
    {
        readonly Random random = new Random();

        private static readonly int _radius = 10;
        private static readonly double _velocity = _radius * 0.8;
        private readonly Timer _timer;

        public ApplicationSimulation()
        {
            _timer = new Timer(MoveTask, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));

        }

        IBoard Board { get; set; } = new EventBoard();

        public void Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack)
        {
            for (int i = 0; i < ballCount; i++)
            {
                IBall ball = new AngleBall(
                    new DefaultPosition 
                    { 
                        X = random.Next(0 + _radius, Board.Width - _radius), 
                        Y = random.Next(0 + _radius, Board.Height - _radius) 
                    }, 
                    new AngleVector(random.Next(5, 11) / 10.0 * _velocity, random.Next(0, 361)), 
                    _radius
                );
                Board.AddBall(ball);

                ballCallBack(ball);
            }
            boardCallBack(Board);
        }

        private void MoveTask(object? _)
        {
            Board.MoveBalls();
        }

    }
}
