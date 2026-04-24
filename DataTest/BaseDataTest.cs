using Data;

namespace DataTest
{
    public class BaseDataTest
    {
        protected IDataSimulation _dataSimulation;

        [TestInitialize]
        public void Setup()
        {
            _dataSimulation = new DataSimulation();
        }
    }
}
