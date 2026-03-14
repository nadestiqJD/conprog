using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Vector
{
    public interface IVector
    {
        IPosition addToPosition(IPosition oldPosition);
    }
}
