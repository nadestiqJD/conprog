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
        void MoveBall(IBall ball);

        void SetNewPositionForBall(IBall ball);

        void HandleWallCollisionForBall(IBall ball);

        void HandleBallCollisionForBall(IBall currentlyMovingBall, IBall otherBall);
    }
}
