using Data.Ball;
using Data.Position;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Ball
{
    public class BallModel : ObservableObject, IBallModel
    {
        private readonly IBall _ball;

        public BallModel(IBall ball)
        {
            _ball = ball;
            
            // BallModel nasłuchuje zmian położenia Ball
            _ball.PositionChanged += (sender, e) => HandleNewPosition();
        }

        public IPosition CurrentPosition
        {
            get => _ball.CurrentPosition; 
        }

        public int Diameter { get => _ball.Radius * 2; }

        private void HandleNewPosition()
        {
            RaisePropertyChanged(nameof(CurrentPosition));
        }
    }
}
