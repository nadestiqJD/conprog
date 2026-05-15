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
    public class ThreadedApplicationSimulation : BaseApplicationSimulation
    {
        public ThreadedApplicationSimulation(IDataSimulation dataSimulation, IBallMovement ballMovement)
            : base(dataSimulation, ballMovement)
        {
        }


        #region Simulation

        private CancellationTokenSource? _ballsSimulationTokenSource;
        public async override Task Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack)
        {
            LockSimulationStateChange();

            await Stop();
            Board = _dataSimulation.CreateBoard();

            _ballsSimulationTokenSource = new CancellationTokenSource();
            var ct = _ballsSimulationTokenSource.Token;

            for (int i = 0; i < ballCount; i++)
            {
                IBall ball = _dataSimulation.CreateBallInBoard(Board);
                
                ballCallBack(ball);

                Thread ballThread = new Thread(() =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        lock (_ballMovementLock)
                        {
                            _ballMovement.MoveBall(ball);
                        }

                        int delay = 1000 / _refreshRate;

                        if (ct.WaitHandle.WaitOne(delay))
                        {
                            break;
                        }
                    }
                });

                ballThread.IsBackground = true;
                ballThread.Name = $"Ball {i} thread";
                ballThread.Start();
            }
            boardCallBack(Board);

            _logger.LogInformation("Simulation started with {ballCount} balls", ballCount);

            UnlockSimulationStateChange();
        }

        public async override Task Stop()
        {
            LockSimulationStateChange();

            if (_ballsSimulationTokenSource != null)
            {
                _ballsSimulationTokenSource.Cancel();
                _ballsSimulationTokenSource.Dispose();
                _ballsSimulationTokenSource = null;

                _dataSimulation.DisposeBoard(Board);

                _logger.LogInformation("Simulation stopped");
            }

            UnlockSimulationStateChange();
        }

        #endregion
    }
}
