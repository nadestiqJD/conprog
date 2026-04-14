using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Vector
{
    public interface IVector
    {
        // TODO: Remove this method.
        IPosition AddToPosition(IPosition oldPosition);

        // Get deltas of x and y.
        IPosition GetDelta();
    }
}
