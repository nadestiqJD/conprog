using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model.SimulationClock
{
    public interface ISimulationClockModel : INotifyPropertyChanged
    {
        string FormattedTime { get; }
    }
}
