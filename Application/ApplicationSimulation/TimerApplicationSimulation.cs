using Application.BallMovement;
using Data;
using Data.Ball;
using Data.Board;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationSimulation
{
    public class TimerApplicationSimulation : BaseApplicationSimulation
    {
        private Timer? _timer;

        public TimerApplicationSimulation(IDataSimulation dataSimulation, IBallMovement ballMovement)
            : base(dataSimulation, ballMovement)
        {
        }

        #region Simulation

        public async override Task Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack)
        {
            LockSimulationStateChange();

            await Stop();
            Board = _dataSimulation.CreateBoard();

            for (int i = 0; i < ballCount; i++)
            {
                IBall ball = _dataSimulation.CreateBallInBoard(Board);
                ballCallBack(ball);
            }
            boardCallBack(Board);

            _timer = new Timer(MoveTask, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000/_refreshRate));
            _logger.LogInformation("Simulation started with {ballCount} balls", ballCount);
        }

        public async override Task Stop()
        {
            LockSimulationStateChange();

            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
                
                _dataSimulation.DisposeBoard(Board);

                _logger.LogInformation("Simulation stopped");
            }

            UnlockSimulationStateChange();
        }

        #endregion

        private void MoveTask(object? _)
        {
            lock (_ballMovementLock)
            {
                foreach (var ball in Board.Balls)
                {
                    _ballMovement.MoveBall(ball);
                }
            }
        }
    }
}
