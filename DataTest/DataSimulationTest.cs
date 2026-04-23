using Data;
using Data.Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTest
{
    [TestClass]
    public class DataSimulationTest
    {
        private readonly IDataSimulation simulation = new DataSimulation();


        [TestMethod]
        public void BallsCreatesInBoardTest()
        {
            IBoard board = new DefaultBoard { Width = 400, Height = 400};

            Assert.IsEmpty(board.Balls);

            simulation.CreateBallInBoard(board);

            Assert.HasCount(1, board.Balls);
        }

    }
}
