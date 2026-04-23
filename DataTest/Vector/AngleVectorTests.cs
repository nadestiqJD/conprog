using Data.Position;
using Data.Vector;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataTest.Vector
{
    [TestClass]
    public class AngleVectorTests
    {
        [TestMethod]
        public void GetDeltaTest()
        {
            double velocity = 1;
            int angle = 45;
            double expectedX = Math.Sqrt(2) / 2;
            double expectedY = Math.Sqrt(2) / 2;

            var vector = new AngleVector(velocity, angle);
            IPosition delta = vector.GetDelta();

            Assert.AreEqual(expectedX, delta.X, 5);
            Assert.AreEqual(expectedY, delta.Y, 5);
        }

        [TestMethod]
        public void GetDeltaForNegativVelocityTest()
        {
            double velocity = -1;
            int angle = 45;
            double expectedX = -Math.Sqrt(2) / 2;
            double expectedY = -Math.Sqrt(2) / 2;

            var vector = new AngleVector(velocity, angle);
            IPosition delta = vector.GetDelta();

            Assert.AreEqual(expectedX, delta.X, 5);
            Assert.AreEqual(expectedY, delta.Y, 5);
        }
    }
}
