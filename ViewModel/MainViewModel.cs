using Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainModel _model;

        public int BallCount
        {
            get => _model.BallCount;
            set
            {
                if (_model.BallCount != value)
                {
                    _model.BallCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Message
        {
            get => _model.Message;
            set
            {
                _model.Message = value;
                OnPropertyChanged();
            }
        }
        public int Width
        {
            get => _model.Board.Width;
            set
            {
                _model.Board.Width = value;
                OnPropertyChanged();
            }

        }

        public int Height
        {
            get => _model.Board.Height;
            set
            {
                _model.Board.Height = value;
                OnPropertyChanged();
            }

        }
        public ICommand StartCommand { get; }

        public MainViewModel()
        {
            StartCommand = new RelayCommand<object>(_ => StartSimulation());
            _model = new MainModel();
        }

        private void StartSimulation()
        {
            Message = $"Uruchamiam symulację dla {BallCount} kul.";
            Console.WriteLine(Message);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}