using Microsoft.Extensions.DependencyInjection;
using SmartFileOrganizer.Models;
using SmartFileOrganizer.Services;
using SmartFileOrganizer.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace SmartFileOrganizer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();

        base.OnStartup(e);
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ObservableCollection<FileCategory>>(provider =>
        {
            var categories = new ObservableCollection<FileCategory>
            {
                new FileCategory("Images", new[] { ".jpg", ".png", ".gif", ".bmp", ".webp" }),
                new FileCategory("Documents", new[] { ".pdf", ".docx", ".txt", ".xlsx" }),
                new FileCategory("Videos", new[] { ".mp4", ".mkv", ".avi" }),
                new FileCategory("Audio", new[] { ".mp3", ".wav" }),
                new FileCategory("Archives", new[] { ".zip", ".rar", ".7z" }),
                new FileCategory("Code", new[] { ".cs", ".js", ".py", ".json" })
            };
            return categories;
        });

        services.AddSingleton<ObservableCollection<LogEntry>>(new ObservableCollection<LogEntry>());
        services.AddLogging();
        services.AddSingleton<LoggingService>();

        services.AddTransient<FileOrganizerService>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<OrganizeFilesViewModel>();
        services.AddTransient<CategoriesViewModel>();
        services.AddTransient<LogsViewModel>();
        services.AddTransient<SettingsViewModel>();
    }
}