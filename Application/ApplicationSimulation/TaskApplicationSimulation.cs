using Application.BallMovement;
using Application.SimulationClock;
using Data.Ball;
using Data.Board;
using Data.DataSimulation;
using Data.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicationSimulation
{
    public class TaskApplicationSimulation : BaseApplicationSimulation
    {
        public TaskApplicationSimulation(
            IDataSimulation dataSimulation, 
            IBallMovement ballMovement,
            ILogger logger,
            ISimulationClock simulationClock
            )
            : base(dataSimulation, ballMovement, logger, simulationClock)
        {
        }


        #region Simulation

        private CancellationTokenSource? _ballsSimulationTokenSource;
        public async override Task Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack)
        {
            await Stop();
            LockSimulationStateChange();

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

            _logger.LogAsync($"Simulation started with {ballCount} balls");

            await _simulationClock.StartClock();

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

                Board?.Dispose();

                _logger.LogAsync("Simulation stopped");

                await _simulationClock.StopClock();
            }

            UnlockSimulationStateChange();
        }

        #endregion
    }
}
