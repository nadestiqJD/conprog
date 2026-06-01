using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.SimulationClock
{
    public interface ISimulationClock
    {
        event SimulationClockTickEventHandler SecTickEvent;

        Task StartClock();

        Task StopClock();
    }

    public delegate void SimulationClockTickEventHandler(object sender, SimulationClockTickEventHandlerArgs eventArgs);


    public class SimulationClockTickEventHandlerArgs : EventArgs
    {
        public int SecondsElapsed { get; set; }

        public SimulationClockTickEventHandlerArgs(int elapsed)
        {
            SecondsElapsed = elapsed;
        }
    }
}
