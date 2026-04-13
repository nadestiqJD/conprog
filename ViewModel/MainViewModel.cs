using Data.Ball;
using Data.Board;
using Model;
using Model.Ball;
using Model.Board;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class MainViewModel : ObservableObject
    {

        private ObservableCollection<IBallModel> Balls = new ObservableCollection<IBallModel>();

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
        
        public ICommand StartCommand { get; }

        public MainViewModel()
        {
            StartCommand = new RelayCommand<object>(_ => StartSimulation());
        }

        private void StartSimulation()
        {
            Balls.Clear();
            
            // todo: wywołać StartSimulation z Model
        }
    }
}