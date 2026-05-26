using Application.ApplicationLogger;
using Application.BallMovement;
using Data;
using Data.Ball;
using Data.Board;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationSimulation
{
    public class TaskApplicationSimulation : BaseApplicationSimulation
    {
        public TaskApplicationSimulation(IDataSimulation dataSimulation, IBallMovement ballMovement, IApplicationLogger applicationLogger)
            : base(dataSimulation, ballMovement, applicationLogger)
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

            Task ballTask;
            for (int i = 0; i < ballCount; i++)
            {
                IBall ball = _dataSimulation.CreateBallInBoard(Board);
                ballTask = Task.Run(async () =>
                {
                    while (true)
                    {
                        ct.ThrowIfCancellationRequested();

                        await _ballMovement.MoveBall(ball);

                        await Task.Delay(1000 / _refreshRate, ct);
                    }
                }, ct);

                ballCallBack(ball);
            }
            boardCallBack(Board);

            _logger.Log($"Simulation started with {ballCount} balls");

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

                _logger.Log("Simulation stopped");
            }

            UnlockSimulationStateChange();
        }

        #endregion
    }
}
