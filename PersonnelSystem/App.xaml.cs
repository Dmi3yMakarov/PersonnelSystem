using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonnelSystem.AppLayer.Handlers;
using PersonnelSystem.AppLayer.Interfaces;
using PersonnelSystem.InfrastructureLayer.QuerySources;
using PersonnelSystem.ViewModels;
using PersonnelSystem.Views;
using System.Windows;

namespace PersonnelSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;

        public App()
        {
            host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices(services =>
            {
                services.AddSingleton<IDepartmentQuerySource, DepartmentQuerySource>()
                        .AddSingleton<IEmployeeQuerySource, EmployeeQuerySource>()
                        .AddSingleton<IDepartmentHistoryQuerySource, DepartmentHistoryQuerySource>()
                        .AddSingleton<IEmployeeHistoryQuerySource, EmployeeHistoryQuerySource>();

                services.AddScoped<CreateDepartmentHandler>()
                        .AddScoped<CreateEmployeeHandler>()
                        .AddScoped<DeleteDepartmentHandler>()
                        .AddScoped<DeleteEmployeeHandler>()
                        .AddScoped<EditDepartmentHandler>()
                        .AddScoped<EditEmployeeHandler>()
                        .AddScoped<GetActualDepartmentListHandler>()
                        .AddScoped<GetDepartmentListForDateHandler>()
                        .AddScoped<GetEmployeeListForPeriodHandler>();

                services.AddTransient<PersonnelSystemView>();
                services.AddScoped<PersonnelSystemViewModel>(provider => new PersonnelSystemViewModel(provider)
                );
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            host.Start();

            var viewModel = host.Services.GetRequiredService<PersonnelSystemViewModel>();
            Window mainWindow = host.Services.GetRequiredService<PersonnelSystemView>();

            mainWindow.DataContext = viewModel;
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
