using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Ball
{
    public interface IBall
    {
        void Move(object? sender, EventArgs e);

        event EventHandler PositionChanged;

        IPosition CurrentPosition { get; }

        int Radius { get; }
    }
}
