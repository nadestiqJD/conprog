using Data.Ball;
using System.Collections.Generic;

namespace Application.CollisionCheckStrategy
{
    public interface ICollisionCheckStrategy
    {
        List<IBall> GetCollidingBallsForBall(IBall ball);
    }
}
