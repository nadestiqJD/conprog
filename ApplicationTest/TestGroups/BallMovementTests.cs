using Data.Ball;
using Data.Position;
using Data.Vector;
namespace ApplicationTest.TestGroups
{
    [TestClass]
    public sealed class BallMovementTests : BaseApplicationTest
    {
        [TestInitialize]
        public void Initialize()
        {
            _applicationSimulation.Board.Width = 400;
            _applicationSimulation.Board.Height = 400;
        }

        [TestMethod]
        public void BallShouldMoveTest()
        {
            IPosition startPosition = new DefaultPosition { X = 200, Y = 200 };
            IPosition expectedPosition = new DefaultPosition { X = 201, Y = 200 };
            IVector vector = new AngleVector(1, 0);
            IBall ball = new AngleBall(startPosition, vector, 1);
            _applicationSimulation.MoveBall(ball);
            Assert.AreEqual(expectedPosition, ball.CurrentPosition);
        }

        [TestMethod]
        public void AllBallsShouldMoveTest()
        {
            var dataList = new List<(int X, int Y, int Angle)>
            {
                (100, 100, 0),
                (200, 200, 0),
                (300, 300, 0),
            };

            Dictionary<IBall, IPosition> startingPositions = new Dictionary<IBall, IPosition>();

            foreach (var item in dataList)
            {
                IPosition startPosition = new DefaultPosition { X = item.X, Y = item.Y };
                IVector vector = new AngleVector(1, item.Angle);
                IBall ball = new AngleBall(startPosition, vector, 1);
                startingPositions.Add(ball, startPosition);
            }

            _applicationSimulation.MoveAllBallsInBoard(_applicationSimulation.Board);

            foreach (var ball in _applicationSimulation.Board.Balls)
            {
                Assert.AreNotEqual(startingPositions.Where(x => x.Key==ball).FirstOrDefault().Value, ball.CurrentPosition );
            }

        }

        [TestMethod]
        public void BallStopWhenHitWallTest()
        {
            var dataList = new List<(int X, int Y, int Angle)>
            {
                (392, 200, 0),
                (0, 200, 180),
                (200, 392, 90),
                (200, 0, 270)
            };

            foreach (var item in dataList)
            {
                IPosition startPosition = new DefaultPosition { X = item.X, Y = item.Y };
                IVector vector = new AngleVector(1, item.Angle);
                IBall ball = new AngleBall(startPosition, vector, 1);
                _applicationSimulation.MoveBall(ball);
                Assert.AreEqual(startPosition, ball.CurrentPosition);
            }
        }
    }
}
