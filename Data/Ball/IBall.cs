using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Ball
{
    public interface IBall
    {
        void Move(object? sender, EventArgs e);

        IPosition CurrentPosition { get; }

        int Radius { get; }
    }
}
