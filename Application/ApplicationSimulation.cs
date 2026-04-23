using Data;
using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Application
{
    public class ApplicationSimulation : IApplicationSimulation
    {
        private Timer _timer;

        private readonly IDataSimulation _dataSimulation;
        private readonly ILogger<ApplicationSimulation> _logger;

        public ApplicationSimulation(ILogger<ApplicationSimulation> logger, IDataSimulation dataSimulation)
        {
            _logger = logger;
            _dataSimulation = dataSimulation;
        }
        private IBoard Board { get; set; } = new DefaultBoard();

        public void MoveAllBallsInBoard(IBoard board)
        {   
            List<IBall> toRemove = new List<IBall>(); 

            foreach (var ball in board.Balls)
            {
                if (!MoveBall(ball))
                { 
                    toRemove.Add(ball);
                    _logger.LogInformation("Ball {} collided with wall", ball);
                };
            }

            foreach (var ball in toRemove)
            {
                _dataSimulation.RemoveBallFromBoard(board, ball);
            }
        }

        public bool MoveBall(IBall ball)
        {
            if (ball.Board == null)
            {
                _logger.LogWarning("Ball is not in a board, cannot move");
                return false;
            }

            double validatedX;
            double validatedY;

            bool continueMoving = true;

            IPosition positionDelta = ball.Vector.GetDelta();

            IPosition newPosition = new DefaultPosition { X = positionDelta.X + ball.CurrentPosition.X, Y = positionDelta.Y + ball.CurrentPosition.Y};

            if (newPosition.X + 2 * ball.Radius > ball.Board.Width)
            {
                validatedX = ball.Board.Width - 2 * ball.Radius;
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

            if (newPosition.Y + 2 * ball.Radius > ball.Board.Height)
            {
                validatedY = ball.Board.Height - 2 * ball.Radius;
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

            _timer = new Timer(MoveTask, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
            _logger.LogInformation("Simulation started with {ballCount} balls", ballCount);
        }

        public void Stop()
        {
            _timer.Dispose();
            _dataSimulation.DisposeBoard(Board);

            _logger.LogInformation("Simulation stopped");
        }

        private void MoveTask(object? _)
        {
            _logger.LogTrace("Simulation tick started");
            MoveAllBallsInBoard(Board);
        }

        public void SetBoardDimenstions(int width, int height)
        {
            Board.Width = width;
            Board.Height = height;
            _logger.LogInformation("Board dimensions in simulation set to {}x{}", width, height);
        }

    }
}
