using ApplicationTest.Base;
using Data.Ball;

namespace ApplicationTest.TestGroups
{
    [TestClass]
    public sealed class SimulationLoopTests : BaseApplicationTest
    {
        [TestMethod]
        [DataRow(TestCategories.TIMER_SIMULATION)]
        [DataRow(TestCategories.TASK_SIMULATION)]
        [DataRow(TestCategories.THREADED_SIMULATION)]
        public void StartSimulationTest(string applicationSimulationImplementation)
        {
            SetApplicationSimulation(applicationSimulationImplementation);

            int moved = 0;

            _applicationSimulation.Start(1, (ball) =>
            {
                ball.PositionChanged += HandleBallMoved;
            }, (board) => { });


            void HandleBallMoved(object sender, EventArgs e)
            {
                moved++;
            }

            Thread.Sleep(500);
            Assert.AreNotEqual(0, moved);
        }

        [TestMethod]
        [DataRow(TestCategories.TIMER_SIMULATION)]
        [DataRow(TestCategories.TASK_SIMULATION)]
        [DataRow(TestCategories.THREADED_SIMULATION)]
        public void StartStopStartSimulationTest(string applicationSimulationImplementation) 
        {
            SetApplicationSimulation(applicationSimulationImplementation);

            int moved = 0;
            int previousMoved = 0;

            _applicationSimulation.Start(1, BallCreationCallback, (board) => { });


            Thread.Sleep(500);
            _applicationSimulation.Stop();
            previousMoved = moved;

            _applicationSimulation.Start(1, BallCreationCallback, (board) => { });

            Thread.Sleep(500);
            Assert.AreNotEqual(previousMoved, moved);

            void BallCreationCallback(IBall ball)
            {
                ball.PositionChanged += HandleBallMoved;
            }

            void HandleBallMoved(object sender, EventArgs e)
            {
                moved++;
            }
        }
    }
}
