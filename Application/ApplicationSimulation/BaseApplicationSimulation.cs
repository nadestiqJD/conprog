using Application.BallMovement;
using Data;
using Data.Ball;
using Data.Board;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicationSimulation
{
    public abstract class BaseApplicationSimulation : IApplicationSimulation
    {
        #region Protected Fields and Properties

        protected readonly int _refreshRate = 60;
        protected IBoard? Board { get; set; }

        protected readonly object _ballMovementLock = new object();

        #endregion

        private readonly object _simulationStartStopLock = new object();
        private bool _isApplicationChangingState = false;

        #region DI Containers

        protected readonly ILogger _logger;

        protected readonly IDataSimulation _dataSimulation;

        protected readonly IBallMovement _ballMovement;

        #endregion

        public BaseApplicationSimulation(IDataSimulation dataSimulation, IBallMovement ballMovement)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger<ThreadedApplicationSimulation>();

            _dataSimulation = dataSimulation;
            _ballMovement = ballMovement;
        }

        #region IApplicationSimulation

        public void SetBoardDimenstions(int width, int height)
        {
            Board.Width = width;
            Board.Height = height;
            _logger.LogInformation("Board dimensions in simulation set to {}x{}", width, height);
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
                    _logger.LogError("Cannot start simulation because it is already changing state.");
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
