using Application.CollisionCheckStrategy;
using Data.Ball;
using Data.Logger;
using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.BallMovement
{
    public class DefaultBallMovement : IBallMovement
    {
        #region DI Containers

        private readonly ICollisionCheckStrategy _collisionCheckStrategy;
        private readonly ILogger _logger;

        #endregion

        public DefaultBallMovement(ICollisionCheckStrategy collisionCheckStrategy, ILogger logger)
        {
            _logger = logger;
            _collisionCheckStrategy = collisionCheckStrategy;
        }

        #region IBallMovement

        public void HandleBallCollisionForBall(IBall currentlyMovingBall, IBall otherBall)
        {
            _logger.LogDebugAsync($"Ball: {currentlyMovingBall.Id} has contact with Ball: {otherBall.Id}");

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

        public void HandleWallCollisionForBall(IBall ball)
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

        public void MoveBall(IBall ball)
        {
            if (ball.Board == null)
            {
                _logger.LogErrorAsync("Ball is not in a board, cannot move");
                return;
            }


            lock (ball)
            {
                SetNewPositionForBall(ball);
                HandleWallCollisionForBall(ball);
            }

            foreach (var otherBall in ball.Board.Balls.Where(b => b != ball))
            {
                var otherIsFirst = otherBall.Id.CompareTo(ball.Id) < 0;

                IBall first, second;
                if (otherIsFirst)
                {
                    first = otherBall;
                    second = ball;
                }
                else
                {
                    first = ball;
                    second = otherBall;
                }

                lock (first)
                {
                    lock (second)
                    {
                        if (_collisionCheckStrategy.AreBallsColliding(first, second))
                        {
                            HandleBallCollisionForBall(first, second);
                        }
                    }
                }
            }

            _logger.LogTraceAsync($"Ball: {ball.Id} is at position: {ball.CurrentPosition}, Vector: {ball.Vector}");
        }

        public void SetNewPositionForBall(IBall ball)
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

    }
}
