using Application.CollisionCheckStrategy;
using Data.Ball;
using Data.Position;
using Data.Vector;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BallMovement
{
    public class DefaultBallMovement : IBallMovement
    {
        #region DI Containers

        private readonly ICollisionCheckStrategy _collisionCheckStrategy;
        private readonly ILogger<DefaultBallMovement> _logger;

        #endregion

        private readonly object _moveBallLock;

        public DefaultBallMovement(ICollisionCheckStrategy collisionCheckStrategy)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger<DefaultBallMovement>();

            _collisionCheckStrategy = collisionCheckStrategy;

            _moveBallLock = new object();
        }

        #region IBallMovement

        public async Task HandleBallCollisionForBall(IBall currentlyMovingBall, IBall otherBall)
        {
            var delta1 = currentlyMovingBall.Vector.GetDelta();
            var delta2 = otherBall.Vector.GetDelta();

            double dx = otherBall.CurrentPosition.X - currentlyMovingBall.CurrentPosition.X;
            double dy = otherBall.CurrentPosition.Y - currentlyMovingBall.CurrentPosition.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            double nx = dx / distance;
            double ny = dy / distance;

            double vRelX = delta1.X - delta2.X;
            double vRelY = delta1.Y - delta2.Y;

            double vRelNormal = vRelX * nx + vRelY * ny;

            if (vRelNormal < 0) return;

            double m1 = currentlyMovingBall.Weight;
            double m2 = otherBall.Weight;
            double impulse = (2.0 * vRelNormal) / (m1 + m2);

            var b1DeltaVector = new DefaultPosition
            {
                X = delta1.X - impulse * m2 * nx,
                Y = delta1.Y - impulse * m2 * ny
            };

            var b2DeltaVector = new DefaultPosition
            {
                X = delta2.X + impulse * m1 * nx,
                Y = delta2.Y + impulse * m1 * ny
            };

            currentlyMovingBall.Vector = AngleVector.GetFromDelta(b1DeltaVector);
            otherBall.Vector = AngleVector.GetFromDelta(b2DeltaVector);
        }

        public async Task HandleWallCollisionForBall(IBall ball)
        {
            double validatedX;
            double validatedY;

            IVector newVector = ball.Vector;

            double rightBorder = ball.Board.Width - ball.Radius;
            double bottomBorder = ball.Board.Height - ball.Radius;
            double leftBorder = ball.Radius;
            double upperBorder = ball.Radius;

            if (ball.CurrentPosition.X > rightBorder)
            {
                validatedX = rightBorder - (ball.CurrentPosition.X - rightBorder);
                newVector = new AngleVector
                {
                    Angle = (180 - ball.Vector.Angle) % 360,
                    Length = ball.Vector.Length
                };

            }
            else if (ball.CurrentPosition.X < leftBorder)
            {
                validatedX = leftBorder - (ball.CurrentPosition.X - leftBorder);
                newVector = new AngleVector
                {
                    Angle = (180 - ball.Vector.Angle) % 360,
                    Length = ball.Vector.Length
                };
            }
            else
            {
                validatedX = ball.CurrentPosition.X;
            }

            if (ball.CurrentPosition.Y > bottomBorder)
            {
                validatedY = bottomBorder - (ball.CurrentPosition.Y - bottomBorder);
                newVector = new AngleVector
                {
                    Angle = (-ball.Vector.Angle) % 360,
                    Length = ball.Vector.Length
                };
            }
            else if (ball.CurrentPosition.Y < upperBorder)
            {
                validatedY = upperBorder - (ball.CurrentPosition.Y - upperBorder);
                newVector = new AngleVector
                {
                    Angle = (-ball.Vector.Angle) % 360,
                    Length = ball.Vector.Length
                };
            }
            else
            {
                validatedY = ball.CurrentPosition.Y;
            }

            ball.CurrentPosition = new DefaultPosition { X = validatedX, Y = validatedY };
            ball.Vector = newVector;
        }

        public async Task MoveBall(IBall ball)
        {
            if (ball.Board == null)
            {
                _logger.LogWarning("Ball is not in a board, cannot move");
                return;
            }

            lock (_moveBallLock)
            {
                SetNewPositionForBall(ball).Wait();
                HandleWallCollisionForBall(ball).Wait();

                foreach (var otherBall in _collisionCheckStrategy.GetCollidingBallsForBall(ball))
                {
                    HandleBallCollisionForBall(ball, otherBall).Wait();
                }
            }
        }

        public async Task SetNewPositionForBall(IBall ball)
        {
            IPosition positionDelta = ball.Vector.GetDelta();

            IPosition newPosition = new DefaultPosition
            {
                X = positionDelta.X + ball.CurrentPosition.X,
                Y = positionDelta.Y + ball.CurrentPosition.Y
            };

            ball.CurrentPosition = newPosition;
        }
        #endregion


        #region Private Methods

        #endregion

    }
}
