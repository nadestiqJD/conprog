using Application;
using Data;

namespace ApplicationTest
{
    [TestClass]
    public abstract class BaseApplicationTest
    {
        protected IApplicationSimulation _applicationSimulation;
        protected IDataSimulation _dataSimulation;

        [TestInitialize]
        public void Setup()
        {
            _dataSimulation = new DataSimulation();
            _applicationSimulation = new ApplicationSimulation(_dataSimulation);
        }
    }
}
