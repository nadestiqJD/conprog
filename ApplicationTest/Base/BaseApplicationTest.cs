using Application.ApplicationSimulation;
using Application.BallMovement;
using Application.CollisionCheckStrategy;
using Data;

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
            _dataSimulation = new DataSimulation();
            _collisionCheckStrategy = new PositionOverlapCollisionCheckStrategy();
            _ballMovement = new DefaultBallMovement(_collisionCheckStrategy);

            var testMethod = this.GetType().GetMethod(TestContext.TestName);
            var categories = testMethod?.GetCustomAttributes(typeof(TestCategoryAttribute), true)
                                    .Select(attr => ((TestCategoryAttribute)attr).TestCategories.First())
                                    .ToList();

            if (categories != null)
            {
                if (categories.Contains(TestCategories.TIMER_SIMULATION))
                {
                    _applicationSimulation = new TimerApplicationSimulation(_dataSimulation, _ballMovement);
                }
                else if (categories.Contains(TestCategories.TASK_SIMULATION))
                {
                    _applicationSimulation = new TaskApplicationSimulation(_dataSimulation, _ballMovement);
                }
                else if (categories.Contains(TestCategories.THREADED_SIMULATION))
                {
                    _applicationSimulation = new ThreadedApplicationSimulation(_dataSimulation, _ballMovement);
                }
            }
        }

        protected void SetApplicationSimulation(string applicationSimulationimplementation)
        {
            if (applicationSimulationimplementation.Equals(TestCategories.TIMER_SIMULATION))
            {
                _applicationSimulation = new TimerApplicationSimulation(_dataSimulation, _ballMovement);
            }
            else if (applicationSimulationimplementation.Equals(TestCategories.TASK_SIMULATION))
            {
                _applicationSimulation = new TaskApplicationSimulation(_dataSimulation, _ballMovement);
            }
            else if (applicationSimulationimplementation.Equals(TestCategories.THREADED_SIMULATION))
            {
                _applicationSimulation = new ThreadedApplicationSimulation(_dataSimulation, _ballMovement);
            }
        }
    }
}
