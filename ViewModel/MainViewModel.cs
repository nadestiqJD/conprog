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

namespace ViewModel
{
    public class MainViewModel : ObservableObject
    {

        private readonly IApplicationSimulation _applicationSimulation;

        public ObservableCollection<IBallModel> Balls { get; private set; } = new ObservableCollection<IBallModel>();

        public IBoardModel Board { get; private set; } = new BoardModel(new EventBoard());

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

        //public int BallDiameter => Balls.FirstOrDefault().Radius * 2;
        
        public ICommand StartCommand { get; }

        public MainViewModel()
        {
            _applicationSimulation = new ApplicationSimulation();

            StartCommand = new RelayCommand<object>(_ => StartSimulation());
        }

        private void StartSimulation()
        {
            Balls.Clear();

            _applicationSimulation.Start(BallCount, HandleBallCreation, (board) => Board = new BoardModel(board));
        }

        private void HandleBallCreation(IBall ball)
        {
            IBallModel ballModel = new BallModel(ball);
            Balls.Add(ballModel);
        }
    }
}