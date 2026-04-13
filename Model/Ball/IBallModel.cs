using Data.Ball;
using Data.Position;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.Ball
{
    public interface IBallModel : INotifyPropertyChanged
    {
        IPosition CurrentPosition { get; }
    }
}
