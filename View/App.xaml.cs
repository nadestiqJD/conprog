using Application;
using Data;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ViewModel;
using WpfApp = System.Windows.Application;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : WpfApp
    {
        private IServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddSingleton<IDataSimulation, DataSimulation>();
            services.AddSingleton<IApplicationSimulation, ApplicationSimulation>();

            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

}
