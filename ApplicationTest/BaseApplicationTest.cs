using Application;
using Data;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationTest
{
    [TestClass]
    public abstract class BaseApplicationTest
    {
        private ServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;

        protected IApplicationSimulation _applicationSimulation;
        protected IDataSimulation _dataSimulation;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddTransient<IDataSimulation, DataSimulation>();
            services.AddTransient<IApplicationSimulation, ApplicationSimulation>();

            _serviceProvider = services.BuildServiceProvider();

            _serviceScope = _serviceProvider.CreateScope();

            _applicationSimulation = _serviceScope.ServiceProvider.GetRequiredService<IApplicationSimulation>();
            _dataSimulation = _serviceScope.ServiceProvider.GetRequiredService<IDataSimulation>();
        }

        [TestInitialize]
        public void Cleanup()
        {
            _serviceScope?.Dispose();
            _serviceProvider?.Dispose();
        }
    }
}
