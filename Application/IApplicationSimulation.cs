using Data.Ball;
using Data.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public interface IApplicationSimulation
    {
        void Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack);

    }
}
