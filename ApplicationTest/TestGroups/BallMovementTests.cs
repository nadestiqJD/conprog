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

            _ballMovement.MoveBall(ball);     // this should check if ball has a Board
            Assert.AreEqual(startPosition, ball.CurrentPosition);
            _ballMovement.SetNewPositionForBall(ball); // this shouldn't
            Assert.AreNotEqual(startPosition, ball.CurrentPosition);
            ball.CurrentPosition = startPosition;

            IBoard board = new DefaultBoard(500, 500);
            _dataSimulation.AddBallToBoard(board, ball);

            _ballMovement.SetNewPositionForBall(ball);
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

            _ballMovement.MoveBall(ball);
            Assert.AreEqual(endPosition, ball.CurrentPosition);

            _ballMovement.MoveBall(ball);
            Assert.AreEqual(startPosition, ball.CurrentPosition);
        }

        [TestMethod]
        public async Task BallShouldWaitTillOtherBallFinishedMovingTest()
        {
            const int ballRadius = 1;
            const int ballWeight = 1;
            const int ballSpeed = 1;
            int ball1X = 100;
            int ball1Y = 100;
            int ball1Angle = 0;
            int ball2X = 104;
            int ball2Y = 100;
            int ball2Angle = 180;
            int ball3X = 100;
            int ball3Y = 103;
            int ball3Angle = 270;

            IBoard board = new DefaultBoard(500, 500);

            var ball1StartPosition = new DefaultPosition { X = ball1X, Y = ball1Y };
            var ball2StartPosition = new DefaultPosition { X = ball2X, Y = ball2Y };
            var ball3StartPosition = new DefaultPosition { X = ball3X, Y = ball3Y };
            var ball1 = new AngleBall
            {
                Board = board,
                Radius = ballRadius,
                Weight = ballWeight,
                Vector = new AngleVector(ballSpeed, ball1Angle),
                CurrentPosition = ball1StartPosition
            };
            var ball1OrphanClone = new AngleBall
            {
                Board = null,
                Radius = ballRadius,
                Weight = ballWeight,
                Vector = new AngleVector(ballSpeed, ball1Angle),
                CurrentPosition = ball1StartPosition
            };
            var ball2 = new AngleBall
            {
                Board = board,
                Radius = ballRadius,
                Weight = ballWeight,
                Vector = new AngleVector(ballSpeed, ball2Angle),
                CurrentPosition = ball2StartPosition
            };
            var ball2OrphanClone = new AngleBall
            {
                Board = null,
                Radius = ballRadius,
                Weight = ballWeight,
                Vector = new AngleVector(ballSpeed, ball2Angle),
                CurrentPosition = ball2StartPosition
            };
            var ball3 = new AngleBall
            {
                Board = board,
                Radius = ballRadius,
                Weight = ballWeight,
                Vector = new AngleVector(ballSpeed, ball3Angle),
                CurrentPosition = ball3StartPosition
            };
            var ball3OrphanClone = new AngleBall
            {
                Board = null,
                Radius = ballRadius,
                Weight = ballWeight,
                Vector = new AngleVector(ballSpeed, ball3Angle),
                CurrentPosition = ball3StartPosition
            };

            List<Task> moveTasks = new List<Task>();
            moveTasks.Add(Task.Run(() => _ballMovement.MoveBall(ball1)));
            moveTasks.Add(Task.Run(() => _ballMovement.MoveBall(ball2)));
            moveTasks.Add(Task.Run(() => _ballMovement.MoveBall(ball3)));

            await Task.WhenAll(moveTasks);

            Assert.AreEqual(new DefaultPosition { X = ball1X + 1, Y = ball1Y }, ball1.CurrentPosition);
            Assert.AreEqual(new DefaultPosition { X = ball2X - 1, Y = ball2Y }, ball2.CurrentPosition);
            Assert.AreEqual(new DefaultPosition { X = ball3X, Y = ball3Y - 1 }, ball3.CurrentPosition);
        }
    }
}
