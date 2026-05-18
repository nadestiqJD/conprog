using ApplicationTest.Base;
using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTest.TestGroups
{
    [TestClass]
    public class CollisionTests : BaseApplicationTest
    {
        [TestMethod]
        [DataRow(5, 5, 1, 6, 5, 1, true)]
        [DataRow(5, 5, 1, 6, 4, 1, true)]
        [DataRow(5, 5, 1, 6, 3, 1, false)]
        [DataRow(100, 100, 1, 100, 100, 1, true)]
        [DataRow(100, 100, 1, 101, 100, 1, true)]
        [DataRow(100, 100, 1, 100, 101, 1, true)]
        [DataRow(100, 100, 1, 101, 101, 1, true)]
        [DataRow(100, 100, 1, 102, 100, 1, true)] // balls touch
        [DataRow(100, 100, 1, 100, 102, 1, true)] // balls touch
        [DataRow(100, 100, 1, 101, 102, 1, false)]
        [DataRow(100, 100, 1, 102, 101, 1, false)]
        [DataRow(100, 100, 1, 102, 102, 1, false)]
        public async Task BallsCollide(int b1X, int b1Y, int b1R, int b2X, int b2Y, int b2R, bool shouldCollide)
        {
            IBoard board = new DefaultBoard(500, 500);
            var b1 = new AngleBall
            {
                Board = board,
                CurrentPosition = new DefaultPosition { X = b1X, Y = b1Y },
                Radius = b1R
            };
            var b2 = new AngleBall
            {
                Board = board,
                CurrentPosition = new DefaultPosition { X = b2X, Y = b2Y },
                Radius = b2R
            };
            board.Balls.Add(b1);
            board.Balls.Add(b2);

            Assert.AreEqual(shouldCollide, _collisionCheckStrategy.GetCollidingBallsForBall(b1).Contains(b2));
            Assert.AreEqual(shouldCollide, _collisionCheckStrategy.GetCollidingBallsForBall(b2).Contains(b1));
        }
    }
}
