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
            //get => _ball.CurrentPosition; 
            get => new DefaultPosition
            {
                X = _ball.CurrentPosition.X - _ball.Radius,
                Y = _ball.CurrentPosition.Y - _ball.Radius
            }; 
        }

        public int Diameter { get => _ball.Radius * 2; }

        private void HandleNewPosition()
        {
            RaisePropertyChanged(nameof(CurrentPosition));
        }
    }
}
