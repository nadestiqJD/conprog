using Application.BallMovement;
using Data.Ball;
using Data.Board;
using Data.Logger;
using Data.DataSimulation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.SimulationClock;

namespace Application.ApplicationSimulation
{
    public abstract class BaseApplicationSimulation : IApplicationSimulation
    {
        #region Protected Fields and Properties

        protected readonly int _refreshRate = 60;
        protected IBoard? Board { get; set; }

        #endregion

        #region Private fields

        private readonly object _simulationStartStopLock = new object();
        private bool _isApplicationChangingState = false;

        #endregion

        #region DI Containers

        protected readonly ILogger _logger;

        protected readonly IDataSimulation _dataSimulation;

        protected readonly IBallMovement _ballMovement;

        protected readonly ISimulationClock _simulationClock;

        #endregion

        public BaseApplicationSimulation(IDataSimulation dataSimulation, IBallMovement ballMovement, 
            ILogger logger,
            ISimulationClock simulationClock
            )
        {
            _dataSimulation = dataSimulation;
            _ballMovement = ballMovement;
            _logger = logger;
            _simulationClock = simulationClock;
        }

        #region IApplicationSimulation

        public void SetBoardDimenstions(int width, int height)
        {
            Board.Width = width;
            Board.Height = height;
            _logger.LogInfoAsync($"Board dimensions in simulation set to {width}x{height}");
        }

        public abstract Task Start(int ballCount, Action<IBall> ballCreationCallback, Action<IBoard> boardCreationCallback);
        public abstract Task Stop();

        #endregion

        protected void LockSimulationStateChange()
        {
            lock (_simulationStartStopLock)
            {
                if (_isApplicationChangingState)
                {
                    _logger.LogWarningAsync("Cannot start simulation because it is already changing state.");
                    return;
                }
                _isApplicationChangingState = true;
            }
        }

        protected void UnlockSimulationStateChange()
        {
            lock (_simulationStartStopLock)
            {
                _isApplicationChangingState = false;
            }
        }
    }
}
