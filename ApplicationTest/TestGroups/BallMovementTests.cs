using ApplicationTest.Base;
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
        public async Task BallShouldMoveIfInBoardTest()
        {
            IPosition startPosition = new DefaultPosition { X = 200, Y = 200 };
            IPosition expectedPosition = new DefaultPosition { X = 201, Y = 200 };
            IVector vector = new AngleVector(1, 0);
            IBall ball = new AngleBall { CurrentPosition = startPosition, Vector = vector, Radius = 1, Weight = 0 };

            await _ballMovement.MoveBall(ball);     // this should check if ball has a Board
            Assert.AreEqual(startPosition, ball.CurrentPosition);
            await _ballMovement.SetNewPositionForBall(ball); // this shouldn't
            Assert.AreNotEqual(startPosition, ball.CurrentPosition);
            ball.CurrentPosition = startPosition;

            IBoard board = new DefaultBoard(500, 500);
            _dataSimulation.AddBallToBoard(board, ball);

            await _ballMovement.SetNewPositionForBall(ball);
            Assert.AreEqual(expectedPosition, ball.CurrentPosition);
        }

        [TestMethod]
        [DataRow(398, 200, 399, 200, 0)]
        [DataRow(2, 200, 1, 200, 180)]
        [DataRow(200, 398, 200, 399, 90)]
        [DataRow(200, 2, 200, 1, 270)]
        public async Task BallsChangeDirectionWhenHittingWalls(int startX, int startY, int endX, int endY, int angle)
        {
            IBoard board = new DefaultBoard(400, 400);

            IPosition startPosition = new DefaultPosition { X = startX, Y = startY };
            IPosition endPosition = new DefaultPosition { X = endX, Y = endY };

            IVector vector = new AngleVector(1, angle);
            IBall ball = new AngleBall { CurrentPosition = startPosition, Vector = vector, Radius = 1, Weight = 0 };

            _dataSimulation.AddBallToBoard(board, ball);

            Assert.AreEqual(startPosition, ball.CurrentPosition);

            await _ballMovement.MoveBall(ball);
            Assert.AreEqual(endPosition, ball.CurrentPosition);

            await _ballMovement.MoveBall(ball);
            Assert.AreEqual(startPosition, ball.CurrentPosition);
        }
    }
}
