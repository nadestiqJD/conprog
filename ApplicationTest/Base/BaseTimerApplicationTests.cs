using Application.ApplicationSimulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTest.Base
{
    public class BaseTimerApplicationTests : BaseApplicationTest
    {
        [TestInitialize]
        public void Setup()
        {
            _applicationSimulation = new TimerApplicationSimulation(_dataSimulation, _ballMovement);
        }
    }
}
