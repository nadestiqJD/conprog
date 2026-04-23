using Data;
using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTest.TestGroups
{
    [TestClass]
    public class DataSimulationTest : BaseDataTest
    {
        [TestMethod]
        public void BallsAreCreatedInBoardTest()
        {
            IBoard board = new DefaultBoard();

            Assert.IsEmpty(board.Balls);

            _dataSimulation.CreateBallInBoard(board);

            Assert.HasCount(1, board.Balls);
        }

        [TestMethod]
        public void AddBallToBoardTest()
        {
            IBoard board = new DefaultBoard();
            IBall ball = new AngleBall(new DefaultPosition { X = 100, Y = 100 }, new AngleVector(1, 0), 1);

            Assert.IsEmpty(board.Balls);
            Assert.IsNull(ball.Board);
            
            _dataSimulation.AddBallToBoard(board, ball);
            
            Assert.HasCount(1, board.Balls);
            Assert.IsNotNull(ball.Board);
        }


        [TestMethod]
        public void RemoveBallFromBoardTest()
        {
            IBoard board = new DefaultBoard();
            IBall ball = new AngleBall(new DefaultPosition { X = 100, Y = 100 }, new AngleVector(1, 0), 1);

            board.Balls.Add(ball);
            ball.Board = board;

            Assert.HasCount(1, board.Balls);
            Assert.IsNotNull(ball.Board);
            
            _dataSimulation.RemoveBallFromBoard(board, ball);

            Assert.IsEmpty(board.Balls);
            Assert.IsNull(ball.Board);
        }
    }
}
