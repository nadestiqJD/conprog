using Data.DataLogger;
using Data.DataSimulation;

namespace DataTest
{
    public class BaseDataTest
    {
        protected IDataSimulation _dataSimulation;

        [TestInitialize]
        public void Setup()
        {
            _dataSimulation = new DataSimulation(new DummyLogger());
        }
    }
}
