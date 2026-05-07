using Data;
using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class ApplicationSimulation : IApplicationSimulation
    {
        private readonly uint _refreshRate = 60;
        private Timer? _timer;
        private readonly object _ballMovementLock = new object();

        private readonly IDataSimulation _dataSimulation;
        private readonly ILogger<ApplicationSimulation> _logger;

        public ApplicationSimulation(IDataSimulation dataSimulation)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger<ApplicationSimulation>();
            _dataSimulation = dataSimulation;
        }

        #region Board property
        private IBoard _board;
        private IBoard Board 
        { 
            get 
            {
                lock (_ballMovementLock)
                {
                    return _board;
                }
            }
            set
            {
                lock (_ballMovementLock)
                {
                    _board = value;
                }
            }

        }
        #endregion

        public void SetBoardDimenstions(int width, int height)
        {
            Board.Width = width;
            Board.Height = height;
            _logger.LogInformation("Board dimensions in simulation set to {}x{}", width, height);
        }

        

        private bool CheckBallsCollision(IBall b1, IBall b2)
        {
            bool positionsOverlap = (b1.CurrentPosition.X - b2.CurrentPosition.X) * (b1.CurrentPosition.X - b2.CurrentPosition.X)
                + (b1.CurrentPosition.Y - b2.CurrentPosition.Y) * (b1.CurrentPosition.Y - b2.CurrentPosition.Y)
                < b1.Radius + b2.Radius;

            // balls go in the same or almost same direction and ball from behind is faster, or balls are approaching head on
            bool generalDirectionOverlaps = true;

            return positionsOverlap && generalDirectionOverlaps;
        }

        private void HandleBallsCollision(IBall b1, IBall b2)
        {
            // this method assumes that b1 and b2 are colliding and only changes vectors of both balls according to
            // https://en.wikipedia.org/wiki/Elastic_collision#Two-dimensional
            //throw new NotImplementedException();

            // be advised that code below is for experimental purposes only and is not intended to use in production
            b1.Vector = new AngleVector
            {
                Angle = (180 + b1.Vector.Angle) % 360,
                Length = b1.Vector.Length
            };
            b2.Vector = new AngleVector
            {
                Angle = (180 + b2.Vector.Angle) % 360,
                Length = b2.Vector.Length
            };
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

        public async Task Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack)
        {
            await Stop();
            lock (_ballMovementLock)
            {
                Board = _dataSimulation.CreateBoard();
            }
            for (int i = 0; i < ballCount; i++)
            {
                ballCallBack(_dataSimulation.CreateBallInBoard(Board));
            }
            boardCallBack(Board);

            _timer = new Timer(MoveTask, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000/_refreshRate));
            _logger.LogInformation("Simulation started with {ballCount} balls", ballCount);
        }

        public async Task Stop()
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

            lock (_ballMovementLock)
            {
                foreach (var ball in Board.Balls)
                {
                    MoveBall(ball);
                }
            }
        }

        public async Task MoveBall(IBall ball)
        {
            if (ball.Board == null)
            {
                _logger.LogWarning("Ball is not in a board, cannot move");
                return;
            }

            HandleWallCollision(ball);

            foreach (var otherBall in ball.Board.Balls.Where(other => CheckBallsCollision(ball, other)))
            {
                HandleBallsCollision(ball, otherBall);
            }
        }

        #endregion
    }
}
