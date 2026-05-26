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
using Application.ApplicationLogger;
using Data.Logger;

namespace ViewModel
{
    public class MainViewModel : ObservableObject
    {

        private readonly IApplicationLogger _logger;
        private readonly IApplicationSimulation _applicationSimulation;
        private IBoardModel _boardModel;

        private bool _applicationStarted = false;
        private object _startLock = new object();

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

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public MainViewModel(IApplicationLogger logger, IApplicationSimulation applicationSimulation)
        {
            _applicationSimulation = applicationSimulation;
            _logger = logger;

            StartCommand = new RelayCommand<object>((_) => StartSimulation());
            StopCommand = new RelayCommand<object>((_) => StopSimulation());
        }

        public MainViewModel() : this(new ApplicationLogger(new FileLogger()))
        {
        }

        private MainViewModel(IApplicationLogger logger) : this(
            logger,
            new TaskApplicationSimulation(
                new DataSimulation(),
                new DefaultBallMovement(new PositionOverlapCollisionCheckStrategy(), logger),
                logger
            )
        )
        {
        }

        private async Task StartSimulation()
        {
            //lock (_startLock)
            //{
            //    if (_applicationStarted)
            //    {
            //        return;
            //    }
            //    _applicationStarted = true;
            //}

            if (BallCount == 0)
            {
                _logger.Log("Cannot start simulation with zero balls.");
                return;
            }

            Balls.Clear();
            await _applicationSimulation.Start(
                BallCount, 
                (ball) => {
                    IBallModel ballModel = new BallModel(ball);
                    Balls.Add(ballModel);
                }, 
                (board) => Board = new BoardModel(board));
        }

        private async Task StopSimulation()
        {
            //lock (_startLock)
            //{
            //    if (!_applicationStarted)
            //    {
            //        return;
            //    }
            //    _applicationStarted = false;
            //}
            await _applicationSimulation.Stop();
        }
    }
}