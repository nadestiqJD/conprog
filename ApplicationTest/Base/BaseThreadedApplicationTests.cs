using Application.ApplicationSimulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTest.Base
{
    public class BaseThreadedApplicationTests : BaseApplicationTest
    {
        [TestInitialize]
        public void Setup()
        {
            _applicationSimulation = new ThreadedApplicationSimulation(_dataSimulation, _ballMovement);
        }
    }
}
