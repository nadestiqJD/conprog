using Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataTest
{
    public class BaseDataTest
    {
        private ServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        protected IDataSimulation _dataSimulation;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddTransient<IDataSimulation, DataSimulation>();

            _serviceProvider = services.BuildServiceProvider();

            _serviceScope = _serviceProvider.CreateScope();
            _dataSimulation = _serviceScope.ServiceProvider.GetRequiredService<IDataSimulation>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _serviceScope.Dispose();
            _serviceProvider.Dispose();
        }
    }
}
