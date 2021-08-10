using AdonisUI;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using SqlStressTester.ViewModels;
using SqlStressTester.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SqlStressTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; }
        public new static App Current => (App)Application.Current;

        public App()
        {
            Services = ConfigureServices();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Services.GetRequiredService<MainWindow>().ShowDialog();
        }

        private IServiceProvider ConfigureServices()
        {
            ServiceCollection serviceCollection = new();

            serviceCollection.AddScoped<MainWindowViewModel>();
            serviceCollection.AddScoped<SettingsViewModel>();
            serviceCollection.AddScoped<DataSourcesViewModel>();
            serviceCollection.AddScoped<StressTesterViewModel>();
            serviceCollection.AddScoped<IDialogService, DialogService>();

            serviceCollection.AddSingleton<MainWindow>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
