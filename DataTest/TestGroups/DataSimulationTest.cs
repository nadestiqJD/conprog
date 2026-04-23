using Data;
using Data.Board;
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
            IBoard board = new DefaultBoard { Width = 400, Height = 400};

            Assert.IsEmpty(board.Balls);

            _dataSimulation.CreateBallInBoard(board);

            Assert.HasCount(1, board.Balls);
        }

    }
}
