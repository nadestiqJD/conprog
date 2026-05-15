using ApplicationTest.Base;
using Data.Ball;
using Data.Position;
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
        public async Task BallsCollide(int b1X, int b1Y, int b1R, int b2X, int b2Y, int b2R, bool shouldCollide)
        {
            var b1 = new AngleBall
            {
                CurrentPosition = new DefaultPosition { X = b1X, Y = b1Y },
                Radius = b1R
            };
            var b2 = new AngleBall
            {
                CurrentPosition = new DefaultPosition { X = b2X, Y = b2Y },
                Radius = b2R
            };

            Assert.AreEqual(shouldCollide, _collisionCheckStrategy.AreBallsColliding(b1, b2));
        }
    }
}
