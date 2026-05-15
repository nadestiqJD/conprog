using Data.Ball;
using Data.Position;
using System.Collections.Generic;

namespace Model.Ball
{
    public class BallModel : ObservableObject, IBallModel
    {
        private readonly IBall _ball;
        private static readonly Dictionary<int, string> _ballColorsForWeight = new Dictionary<int, string>() {
            { 5, "Red"  },
            { 6, "Green"  },
            { 7, "Blue"  },
            { 8, "Purple"  },
            { 9, "Cyan"  },
            { 10, "RosyBrown"  },
        }; 

        public BallModel(IBall ball)
        {
            _ball = ball;
            _ball.PositionChanged += (sender, e) => HandleNewPosition();
        }

        public IPosition CurrentPosition
        {
            get => new DefaultPosition
            {
                X = _ball.CurrentPosition.X - _ball.Radius,
                Y = _ball.CurrentPosition.Y - _ball.Radius
            }; 
        }

        public int Diameter { get => _ball.Radius * 2; }

        public string BallColor 
        { 
            get => _ballColorsForWeight[_ball.Weight];
        }

        private void HandleNewPosition()
        {
            RaisePropertyChanged(nameof(CurrentPosition));
        }
    }
}
