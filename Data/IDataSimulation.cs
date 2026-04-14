using Data.Ball;
using Data.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IDataSimulation
    {
        // Create new Ball with default values and add it to given Board.
        IBall CreateBallInBoard(IBoard board);
    }
}
