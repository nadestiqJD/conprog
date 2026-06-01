using Data.Ball;
using Data.Board;
using Model;
using Model.Ball;
using Model.Board;
using Application;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using Data;
using System.Threading.Tasks;
using Application.ApplicationSimulation;
using Application.BallMovement;
using Application.CollisionCheckStrategy;
using System.Threading;
using Data.Logger;
using Data.DataSimulation;
using Model.SimulationClock;
using Application.SimulationClock;

namespace ViewModel
{
    public class MainViewModel : ObservableObject
    {

        private readonly ILogger _logger;
        private readonly IApplicationSimulation _applicationSimulation;
        private IBoardModel _boardModel;

        private object _stateLock = new object();

        public ObservableCollection<IBallModel> Balls { get; private set; } = new ObservableCollection<IBallModel>();

        public IBoardModel? Board 
        { 
            get => _boardModel; 
            private set
            {
                _boardModel = value;
                RaisePropertyChanged();
            } 
        
        }

        #region BallCount
        private int _ballCount = 10;
        public int BallCount
        {
            get => _ballCount;
            set
            {
                if (_ballCount != value)
                {
                    _ballCount = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        public ISimulationClockModel SimulationClock { get; private set; }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public MainViewModel(ILogger logger, ISimulationClock simulationClock, IApplicationSimulation applicationSimulation)
        {
            _applicationSimulation = applicationSimulation;
            _logger = logger;
            SimulationClock = new SimulationClockModel(simulationClock);

            StartCommand = new RelayCommand<object>((_) => StartSimulation());
            StopCommand = new RelayCommand<object>((_) => StopSimulation());
        }

        public MainViewModel() : this(new FileLogger(), new SecondSimulationClock())
        {
        }

        private MainViewModel(ILogger logger, ISimulationClock simulationClock) : this(
            logger,
            simulationClock,
            new TaskApplicationSimulation(
                new DataSimulation(logger),
                new DefaultBallMovement(new PositionOverlapCollisionCheckStrategy(), logger),
                logger,
                simulationClock
            )
        )
        {
        }

        private async Task StartSimulation()
        {
            Task startTask;
            lock (_stateLock)
            {
                if (BallCount == 0)
                {
                    _logger.LogAsync("Cannot start simulation with zero balls.");
                    return;
                }

                Balls.Clear();
                startTask = _applicationSimulation.Start(
                    BallCount,
                    (ball) =>
                    {
                        IBallModel ballModel = new BallModel(ball);
                        Balls.Add(ballModel);
                    },
                    (board) => Board = new BoardModel(board));
            }

            Task.WaitAll(new Task[] {startTask});
        }

        private async Task StopSimulation()
        {
            Task stopTask;
            lock (_stateLock)
            {
                stopTask = _applicationSimulation.Stop();
            }
            await stopTask;
        }
    }
}