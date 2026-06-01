using System;
using System.Collections.Generic;
using System.Text;
using Application.SimulationClock;

namespace Model.SimulationClock
{
    public class SimulationClockModel : ObservableObject, ISimulationClockModel
    {
        private string _formattedTimeInSeconds = string.Empty;
        public string FormattedTime 
        { 
            get => _formattedTimeInSeconds; 
            set
            {
                _formattedTimeInSeconds = value;
                RaisePropertyChanged();
            } 
        }

        private readonly ISimulationClock _clock;
        public SimulationClockModel(ISimulationClock simulationClock)
        {
            _clock = simulationClock;
            simulationClock.SecTickEvent += HandleSecTickEvent;
        }

        ~SimulationClockModel()
        {
            _clock.SecTickEvent -= HandleSecTickEvent;
        }

        private void HandleSecTickEvent(object? sender, SimulationClockTickEventHandlerArgs eventArgs)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(eventArgs.SecondsElapsed);

            FormattedTime = timeSpan.ToString("c");
        }
    }
}
