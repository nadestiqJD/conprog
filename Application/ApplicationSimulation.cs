using Data;
using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Application
{
    public class ApplicationSimulation : IApplicationSimulation
    {
        private readonly Timer _timer;

        private readonly IDataSimulation _dataSimulation;

        public ApplicationSimulation()
        {
            _timer = new Timer(MoveTask, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
            _dataSimulation = new DataSimulation();
        }

        IBoard Board { get; set; } = new EventBoard();

        public void MoveAllBallsInBoard(IBoard board)
        {
            throw new NotImplementedException();
        }

        public void MoveBall(IBall ball)
        {
            throw new NotImplementedException();
        }

        public void Start(int ballCount, Action<IBall> ballCallBack, Action<IBoard> boardCallBack)
        {
            for (int i = 0; i < ballCount; i++)
            {
                ballCallBack(_dataSimulation.CreateBallInBoard(Board));
            }
            boardCallBack(Board);
        }

        // TODO: adjust this for changes in IBoard and IBall
        private void MoveTask(object? _)
        {
            Board.MoveBalls();
        }

    }
}
