using Data.Ball;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.CollisionCheckStrategy
{
    public class PositionOverlapCollisionCheckStrategy : ICollisionCheckStrategy
    {
        private bool AreBallsColliding(IBall ball1, IBall ball2)
        {
            if (ball1 == ball2)
            {
                return false;
            }

            bool positionsOverlap = (ball1.CurrentPosition.X - ball2.CurrentPosition.X) * (ball1.CurrentPosition.X - ball2.CurrentPosition.X)
                + (ball1.CurrentPosition.Y - ball2.CurrentPosition.Y) * (ball1.CurrentPosition.Y - ball2.CurrentPosition.Y)
                <= (ball1.Radius + ball2.Radius) * (ball1.Radius + ball2.Radius);

            return positionsOverlap;
        }

        public List<IBall> GetCollidingBallsForBall(IBall ball)
        {
            return ball.Board.Balls
                .Where(other => AreBallsColliding(ball, other))
                .ToList();
        }
    }
}
