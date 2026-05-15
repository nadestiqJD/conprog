using Data.Ball;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.BallMovement
{
    public interface IBallMovement
    {
        /// <summary>
        /// Aggregate method to set new position based on vector, handle wall and balls collision
        /// </summary>
        /// <param name="ball"><see cref="IBall"/> object to move</param>
        /// <returns></returns>
        Task MoveBall(IBall ball);

        Task SetNewPositionForBall(IBall ball);

        Task<List<IBall>> FindCollidingBallsForBall(IBall ball);

        Task HandleWallCollisionForBall(IBall ball);

        Task HandleBallCollisionForBall(IBall currentlyMovingBall, IBall otherBall);
    }
}
