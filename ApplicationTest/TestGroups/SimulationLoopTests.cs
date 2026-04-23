using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTest.TestGroups
{
    [TestClass]
    public sealed class SimulationLoopTests : BaseApplicationTest
    {
        [TestMethod]
        public void StartSimulationTest()
        {
            int moved = 0;

            _applicationSimulation.Start(1, (ball) =>
            {
                ball.PositionChanged += HandleBallMoved;
            }, (board) => { });


            void HandleBallMoved(object sender, EventArgs e)
            {
                moved++;
            }

            Thread.Sleep(1000);
            Assert.AreNotEqual(0, moved);
        }
    }
}
