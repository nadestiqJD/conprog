using Data.Ball;
using Data.Board;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IApplicationSimulation
    {
        // Start simmulation for given amount of Balls.
        Task Start(int ballCount, Action<IBall> ballCreationCallback, Action<IBoard> boardCreationCallback);

        Task Stop();

        void SetBoardDimenstions(int width, int height);
    }
}
