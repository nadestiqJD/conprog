using Data;
using Data.Ball;
using Data.Board;
using Data.Position;
using Data.Vector;

namespace DataTest
{
    [TestClass]
    public sealed class BoardTests
    {
        [TestMethod]
        public void IndividualBallsEventHandlers_ShouldMoveBothThenOnlyBall2()
        {
            IBoard board = new RectangularBoard { Height = 100, Width = 100 };

            IBall ball1 = new VectorBall(new DefaultPosition { X = 40, Y = 60 });
            IBall ball2 = new VectorBall(new DefaultPosition { X = 70, Y = 30 });

            board.AddBall(ball1);
            board.AddBall(ball2);
            board.MoveBalls();

            IPosition oldPos1 = ball1.CurrentPosition;
            IPosition oldPos2 = ball2.CurrentPosition;

            System.Console.WriteLine(ball1.CurrentPosition);
            System.Console.WriteLine(ball2.CurrentPosition);

            board.StopBall(ball1);
            board.MoveBalls();

            IPosition newPos1 = ball1.CurrentPosition;
            IPosition newPos2 = ball2.CurrentPosition;

            System.Console.WriteLine(ball1.CurrentPosition);
            System.Console.WriteLine(ball2.CurrentPosition);

            Assert.AreEqual(oldPos1, newPos1);
            Assert.AreNotEqual(oldPos2, newPos2);
        }

        [TestMethod]
        public void BallShouldStopAtBoardBorder()
        {
            IBoard board = new RectangularBoard { Height = 100, Width = 100 };

            IBall leftBall = new VectorBall (new DefaultPosition { X = 2 , Y = 50 }, new IntegerVector { DeltaX = -1, DeltaY = 0 });
            IBall rightBall = new VectorBall (new DefaultPosition { X = 98 , Y = 50 }, new IntegerVector { DeltaX = 1, DeltaY = 0 });
            IBall topBall = new VectorBall (new DefaultPosition { X = 50 , Y = 2 }, new IntegerVector { DeltaX = 0, DeltaY = -1 });
            IBall bottomBall = new VectorBall (new DefaultPosition { X = 50 , Y = 98 }, new IntegerVector { DeltaX = 0, DeltaY = 1 });

            board.AddBall(leftBall);
            board.AddBall(rightBall);
            board.AddBall(topBall);
            board.AddBall(bottomBall);

            // przesunięcie o 1 pozycję w kierunku brzegu każdej kuli
            board.MoveBalls();

            Assert.AreEqual(1, leftBall.CurrentPosition.X);
            Assert.AreEqual(99, rightBall.CurrentPosition.X);
            Assert.AreEqual(1, topBall.CurrentPosition.Y);
            Assert.AreEqual(99, bottomBall.CurrentPosition.Y);

            // pozycja powinna zostać taka sama (kule mają promień 1)
            board.MoveBalls();

            Assert.AreEqual(1, leftBall.CurrentPosition.X);
            Assert.AreEqual(99, rightBall.CurrentPosition.X);
            Assert.AreEqual(1, topBall.CurrentPosition.Y);
            Assert.AreEqual(99, bottomBall.CurrentPosition.Y);
        }

        [TestMethod]
        public void DiagonalMovement_ShouldClipPositionToWall()
        {
            Assert.Fail("FloatVector not implemented.");
        }
    }
}
