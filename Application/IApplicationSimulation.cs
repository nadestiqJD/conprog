using Data.Ball;
using Data.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public interface IApplicationSimulation
    {
        // Start simmulation for given amount of Balls.
        void Start(int ballCount, Action<IBall> ballCreationCallback, Action<IBoard> boardCreationCallback);

        // Calculates and sets new position for Ball.
        bool MoveBall(IBall ball);

        // Calculates and sets new position for each Ball in Board.
        void MoveAllBallsInBoard(IBoard board);

    }
}
