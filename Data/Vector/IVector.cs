using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Vector
{
    public interface IVector
    {
        IPosition GetDelta();

        double Length { get; }
        int Angle { get; }
    }
}
