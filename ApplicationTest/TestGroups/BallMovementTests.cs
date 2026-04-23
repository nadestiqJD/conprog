using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;
namespace ApplicationTest.TestGroups
{
    [TestClass]
    public sealed class BallMovementTests : BaseApplicationTest
    {

        [TestMethod]
        public void BallShouldMoveIfInBoardTest()
        {
            IPosition startPosition = new DefaultPosition { X = 200, Y = 200 };
            IPosition expectedPosition = new DefaultPosition { X = 201, Y = 200 };
            IVector vector = new AngleVector(1, 0);
            IBall ball = new AngleBall(startPosition, vector, 1);

            Assert.IsFalse(_applicationSimulation.MoveBall(ball));
            Assert.AreEqual(startPosition, ball.CurrentPosition);

            IBoard board = new DefaultBoard();
            _dataSimulation.AddBallToBoard(board, ball);

            Assert.IsTrue(_applicationSimulation.MoveBall(ball));
            Assert.AreEqual(expectedPosition, ball.CurrentPosition);
        }

        [TestMethod]
        public void AllBallsShouldMoveTest()
        {
            IBoard board = new DefaultBoard() { Width = 400, Height = 400 };
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

            _applicationSimulation.MoveAllBallsInBoard(board);

            foreach (var ball in board.Balls)
            {
                Assert.AreNotEqual(startingPositions.Where(x => x.Key==ball).FirstOrDefault().Value, ball.CurrentPosition );
            }

        }

        [TestMethod]
        [DataRow(397, 200, 398, 200, 0)]
        [DataRow(1, 200, 0, 200, 180)]
        [DataRow(200, 397, 200, 398, 90)]
        [DataRow(200, 1, 200, 0, 270)]
        public void BallStopWhenHitWallTest(int startX, int startY, int endX, int endY, int angle)
        {
            IBoard board = new DefaultBoard() { Width = 400, Height = 400 };

            IPosition startPosition = new DefaultPosition { X = startX, Y = startY };
            IPosition endPosition = new DefaultPosition { X = endX, Y = endY };

            IVector vector = new AngleVector(1, angle);
            IBall ball = new AngleBall(startPosition, vector, radius: 1);

            _dataSimulation.AddBallToBoard(board, ball);

            Assert.AreEqual(startPosition, ball.CurrentPosition);

            Assert.IsTrue(_applicationSimulation.MoveBall(ball));
            Assert.AreEqual(endPosition, ball.CurrentPosition);

            Assert.IsFalse(_applicationSimulation.MoveBall(ball));
            Assert.AreEqual(endPosition, ball.CurrentPosition);
        }
    }
}
