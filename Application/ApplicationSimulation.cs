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
        private readonly uint _refreshRate = 60;
        private Timer? _timer;

        private readonly IDataSimulation _dataSimulation;
        private readonly ILogger<ApplicationSimulation> _logger;

        public ApplicationSimulation(IDataSimulation dataSimulation)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger<ApplicationSimulation>();
            _dataSimulation = dataSimulation;
        }
        private IBoard Board { get; set; } = new DefaultBoard();

        public void MoveAllBallsInBoard(IBoard board)
        {   
            List<IBall> toRemove = new List<IBall>(); 

            foreach (var ball in board.Balls)
            {
                MoveBall(ball);
            }
        }

        public void MoveBall(IBall ball)
        {
            if (ball.Board == null)
            {
                _logger.LogWarning("Ball is not in a board, cannot move");
                return;
            }

            HandleWallCollision(ball);
        }

        private void HandleWallCollision(IBall ball)
        {
            double validatedX;
            double validatedY;

            IPosition positionDelta = ball.Vector.GetDelta();

            IPosition newPosition = new DefaultPosition
            {
                X = positionDelta.X + ball.CurrentPosition.X,
                Y = positionDelta.Y + ball.CurrentPosition.Y
            };
            IVector newVector = ball.Vector;

            double rightBorder = ball.Board.Width - ball.Radius;
            double bottomBorder = ball.Board.Height - ball.Radius;
            double leftBorder = ball.Radius;
            double upperBorder = ball.Radius;

            if (newPosition.X > rightBorder)
            {
                validatedX = rightBorder - (newPosition.X - rightBorder);
                newVector = new AngleVector
                {
                    Angle = (180 - ball.Vector.Angle) % 360,
                    Length = ball.Vector.Length
                };

            }
            else if (newPosition.X < leftBorder)
            {
                validatedX = leftBorder - (newPosition.X - leftBorder);
                newVector = new AngleVector
                {
                    Angle = (180 - ball.Vector.Angle) % 360,
                    Length = ball.Vector.Length
                };
            }
            else
            {
                validatedX = newPosition.X;
            }

            if (newPosition.Y > bottomBorder)
            {
                validatedY = ball.Board.Height - ball.Radius;
                newVector = new AngleVector
                {
                    Angle = (-ball.Vector.Angle) % 360,
                    Length = ball.Vector.Length
                };
            }
            else if (newPosition.Y < upperBorder)
            {
                validatedY = ball.Radius;
                newVector = new AngleVector
                {
                    Angle = (-ball.Vector.Angle) % 360,
                    Length = ball.Vector.Length
                };
            }
            else
            {
                validatedY = newPosition.Y;
            }

            ball.CurrentPosition = new DefaultPosition { X = validatedX, Y = validatedY };
            ball.Vector = newVector;
        }

        #region Simulation

        public void Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack)
        {
            for (int i = 0; i < ballCount; i++)
            {
                ballCallBack(_dataSimulation.CreateBallInBoard(Board));
            }
            boardCallBack(Board);

            _timer = new Timer(MoveTask, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000/_refreshRate));
            _logger.LogInformation("Simulation started with {ballCount} balls", ballCount);
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _dataSimulation.DisposeBoard(Board);

                _logger.LogInformation("Simulation stopped");
            }
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

        #endregion
    }
}
