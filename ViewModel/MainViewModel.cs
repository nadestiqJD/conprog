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
using Microsoft.Extensions.Logging;

namespace ViewModel
{
    public class MainViewModel : ObservableObject
    {

        private readonly ILogger _logger;
        private readonly IApplicationSimulation _applicationSimulation;

        public ObservableCollection<IBallModel> Balls { get; private set; } = new ObservableCollection<IBallModel>();

        public IBoardModel? Board { get; private set; } = new BoardModel(new DefaultBoard());

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

        public MainViewModel(ILogger<MainViewModel> logger, IApplicationSimulation applicationSimulation)
        {
            _logger = logger;
            _applicationSimulation = applicationSimulation;

            StartCommand = new RelayCommand<object>(_ => StartSimulation());
            StopCommand = new RelayCommand<object>(_ => StopSimulation());
        }

        private void StartSimulation()
        {
            if (BallCount == 0)
            {
                _logger.LogWarning("Cannot start simulation with zero balls.");
                return;
            }

            Balls.Clear();
            _applicationSimulation.Start(BallCount, HandleBallCreation, (board) => Board = new BoardModel(board));
        }

        private void StopSimulation()
        {
            _applicationSimulation.Stop();
        }

        private void HandleBallCreation(IBall ball)
        {
            IBallModel ballModel = new BallModel(ball);
            Balls.Add(ballModel);
        }
    }
}