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


        private int _simulationTimeInSeconds;
        public int SimulationTimeInSeconds 
        { 
            get => _simulationTimeInSeconds; 
            set
            {
                _simulationTimeInSeconds = value;
                RaisePropertyChanged();
            }
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public MainViewModel(ILogger logger, IApplicationSimulation applicationSimulation)
        {
            _applicationSimulation = applicationSimulation;
            _logger = logger;

            StartCommand = new RelayCommand<object>((_) => StartSimulation());
            StopCommand = new RelayCommand<object>((_) => StopSimulation());
        }

        public MainViewModel() : this(new FileLogger())
        {
        }

        private MainViewModel(ILogger logger) : this(
            logger,
            new TaskApplicationSimulation(
                new DataSimulation(logger),
                new DefaultBallMovement(new PositionOverlapCollisionCheckStrategy(), logger),
                logger
            )
        )
        {
        }

        private async Task StartSimulation()
        {
            Task startTask;
            Task startTimerTask = new Task(() =>
            {
                SimulationTimeInSeconds = 0;
                _simulationTimeTimer = new Timer(IncrementSecondsCounter, null, 0, 1000);
            });
            lock (_stateLock)
            {
                if (BallCount == 0)
                {
                    _logger.Log("Cannot start simulation with zero balls.");
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
                startTimerTask.Start();
            }

            Task.WaitAll(new Task[] {startTask, startTimerTask});
        }

        private async Task StopSimulation()
        {
            Task stopTask;
            lock (_stateLock)
            {
                stopTask = _applicationSimulation.Stop();
                _simulationTimeTimer?.Dispose();
                _simulationTimeTimer = null;
            }
            await stopTask;
        }

        private void IncrementSecondsCounter(Object stateInfo)
        {
            ++SimulationTimeInSeconds;
        }
        private Timer? _simulationTimeTimer;
    }
}