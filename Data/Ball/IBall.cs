using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Ball
{
    public interface IBall
    {
        event EventHandler PositionChanged;

        IPosition CurrentPosition { get;  set; }

        int Radius { get; }

        IVector Vector { get; set; }
    }
}
