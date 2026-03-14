using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Board
{
    public interface IRectangularBoard : IBoard
    {
        int Width { get; }

        int Height { get; }
    }
}
