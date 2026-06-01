using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Application.SimulationClock
{
    public class SecondSimulationClock : ISimulationClock
    {
        private Timer? _clockTimer;
        private int _secondsCounter;

        public SecondSimulationClock() 
        {
        }

        public event SimulationClockTickEventHandler SecTickEvent;

        public async Task StartClock()
        {
            _secondsCounter = -1;   // when timer creates, it initially runs 1st tick of HandleTimerTick
            _clockTimer = new Timer(HandleTimerTick, null, 0, 1000);
        }

        public async Task StopClock()
        {
            _clockTimer?.Dispose();
            _clockTimer = null;
        }

        private void HandleTimerTick(object? context)
        {
            ++_secondsCounter;
            SecTickEvent.Invoke(this, new SimulationClockTickEventHandlerArgs(_secondsCounter));
        }
    }
}
