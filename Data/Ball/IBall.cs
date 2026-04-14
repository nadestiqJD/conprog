using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Ball
{
    public interface IBall
    {
        // TODO: remove Move method. 
        void Move(object? sender, EventArgs e);

        event EventHandler PositionChanged;

        // TODO: public setter, raise event on set
        IPosition CurrentPosition { get; }

        int Radius { get; }
    }
}
