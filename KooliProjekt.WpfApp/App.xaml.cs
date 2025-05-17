using KooliProjekt.WpfApp.Api;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace KooliProjekt.WpfApp
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Регистрируем ApiClient
            services.AddSingleton<IApiClient>(provider =>
                new ApiClient("https://localhost:5001/")); // Укажите правильный URL к вашему API

            // Регистрируем вспомогательные классы
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetService<MainWindowViewModel>();
            mainWindow.Show();
        }
    }
}
