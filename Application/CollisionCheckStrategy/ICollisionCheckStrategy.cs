using Data.Ball;

namespace Application.CollisionCheckStrategy
{
    public interface ICollisionCheckStrategy
    {
        bool AreBallsColliding(IBall ball1, IBall ball2);
    }
}
