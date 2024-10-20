using MainAppGraphical.ViewModels;
using MainAppGraphical.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resources.Services;
using Resources.Interfaces;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace MainAppGraphical;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "Product_List.json");

        _host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
        {
            services.AddSingleton<ProductService>();
            services.AddSingleton<FileService>(new FileService(filePath));
        

            services.AddSingleton<MainWindowModel>();
            services.AddSingleton<MainWindow>();

            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<HomeView>();

        }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        await _host.StartAsync();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
    }


