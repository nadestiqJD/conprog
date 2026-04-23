using Data;
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
        private readonly Timer _timer;

        private readonly IDataSimulation _dataSimulation;

        public ApplicationSimulation()
        {
            _timer = new Timer(MoveTask, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
            _dataSimulation = new DataSimulation();
        }
        public IBoard Board { get; set; } = new DefaultBoard();

        public void MoveAllBallsInBoard(IBoard board)
        {   
            List<IBall> toRemove = new List<IBall>(); 

            foreach (var ball in board.Balls)
            {
                if (!MoveBall(ball))
                { 
                    toRemove.Add(ball);
                };
            }

            foreach (var ball in toRemove)
            {
                board.Balls.Remove(ball);
            }
        }

        public bool MoveBall(IBall ball)
        {
            IPosition newPosition;

            double validatedX;
            double validatedY;

            bool continueMoving = true;

            IPosition positionDelta = ball.Vector.GetDelta();

            newPosition = new DefaultPosition { X = positionDelta.X + ball.CurrentPosition.X, Y = positionDelta.Y + ball.CurrentPosition.Y};

            if (newPosition.X + 2 * ball.Radius + 6 > Board.Width)
            {
                validatedX = Board.Width - 2 * ball.Radius - 6;
                continueMoving = false;
            }
            else if (newPosition.X < 0)
            {
                validatedX = 0;
                continueMoving = false;
            }
            else
            {
                validatedX = newPosition.X;
            }

            if (newPosition.Y + 2 * ball.Radius + 6 > Board.Height)
            {
                validatedY = Board.Height - 2 * ball.Radius - 6;
                continueMoving = false;
            }
            else if (newPosition.Y < 0)
            {
                validatedY = 0;
                continueMoving = false;
            }
            else
            {
                validatedY = newPosition.Y;
            }

            ball.CurrentPosition = new DefaultPosition { X = validatedX, Y = validatedY };

            return continueMoving;
        }

        public void Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack)
        {
            for (int i = 0; i < ballCount; i++)
            {
                ballCallBack(_dataSimulation.CreateBallInBoard(Board));
            }
            boardCallBack(Board);
        }

        private void MoveTask(object? _)
        {
            MoveAllBallsInBoard(Board);
        }

    }
}
