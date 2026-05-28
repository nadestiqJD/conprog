using Application.ApplicationSimulation;
using Application.BallMovement;
using Application.CollisionCheckStrategy;
using Data.DataLogger;
using Data.DataSimulation;

namespace ApplicationTest.Base
{
    [TestClass]
    public abstract class BaseApplicationTest
    {
        public TestContext TestContext { get; set; }

        protected IApplicationSimulation _applicationSimulation;
        protected IDataSimulation _dataSimulation;
        protected ICollisionCheckStrategy _collisionCheckStrategy;
        protected IBallMovement _ballMovement;

        [TestInitialize]
        public void Setup()
        {
            var log = new DummyLogger();
            _dataSimulation = new DataSimulation(log);
            _collisionCheckStrategy = new PositionOverlapCollisionCheckStrategy();
            _ballMovement = new DefaultBallMovement(_collisionCheckStrategy, log);
            _applicationSimulation = new TaskApplicationSimulation(_dataSimulation, _ballMovement, log);
        }
    }
}
