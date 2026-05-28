using Data.Ball;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Board
{
    public interface IBoard : IDisposable
    {
        int Width { get; set; }

        int Height { get; set; }

        event EventHandler DimensionsChanged;

        List<IBall> Balls { get; }
    }
}
