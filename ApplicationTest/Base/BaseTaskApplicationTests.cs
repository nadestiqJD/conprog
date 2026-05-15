using Application.ApplicationSimulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTest.Base
{
    public class BaseTaskApplicationTests : BaseApplicationTest
    {
        [TestInitialize]
        public void Setup()
        {
            _applicationSimulation = new TaskApplicationSimulation(_dataSimulation, _ballMovement);
        }
    }
}
