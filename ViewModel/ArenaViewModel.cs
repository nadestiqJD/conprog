using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class ArenaViewModel : INotifyPropertyChanged
    {
        private int _ballCount = 10; // Domyślna wartość

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

        public ICommand StartCommand { get; }

        public ArenaViewModel()
        {
            // Definiujemy, co ma się stać po kliknięciu START
            StartCommand = new RelayCommand(StartSimulation);
        }

        private void StartSimulation()
        {
            // Tutaj później dodasz wywołanie logiki z Modelu
            Console.WriteLine($"Uruchamiam symulację dla {BallCount} kul.");
        }

        // Standardowa implementacja powiadamiania o zmianach
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // Prosta implementacja ICommand, aby przycisk działał
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged;
    }
}