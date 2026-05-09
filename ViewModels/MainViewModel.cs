using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace SmartFileOrganizer.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private object? currentViewModel;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        // Initialize with dashboard
        CurrentViewModel = _serviceProvider.GetService(typeof(DashboardViewModel));
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        CurrentViewModel = _serviceProvider.GetService(typeof(DashboardViewModel));
    }

    [RelayCommand]
    private void NavigateToOrganizeFiles()
    {
        CurrentViewModel = _serviceProvider.GetService(typeof(OrganizeFilesViewModel));
    }

    [RelayCommand]
    private void NavigateToCategories()
    {
        CurrentViewModel = _serviceProvider.GetService(typeof(CategoriesViewModel));
    }

    [RelayCommand]
    private void NavigateToLogs()
    {
        CurrentViewModel = _serviceProvider.GetService(typeof(LogsViewModel));
    }

    [RelayCommand]
    private void NavigateToSettings()
    {
        CurrentViewModel = _serviceProvider.GetService(typeof(SettingsViewModel));
    }
}