using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _ballCount = 10;

        private string _message;

        public int BallCount
        {
            get => _ballCount;
            set
            {
                if (_ballCount != value)
                {
                    _ballCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; }

        public MainViewModel()
        {
            StartCommand = new RelayCommand<object>(_ => StartSimulation());
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