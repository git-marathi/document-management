using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SmartFileOrganizer.Models;
using System;

namespace SmartFileOrganizer.ViewModels;

public partial class MainViewModel : BaseViewModel, IRecipient<NavigateMessage>
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private object? currentViewModel;

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        // Initialize with dashboard
        CurrentViewModel = _serviceProvider.GetService(typeof(DashboardViewModel));
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(NavigateMessage message)
    {
        NavigateTo(message.ViewName);
    }

    private void NavigateTo(string viewName)
    {
        CurrentViewModel = viewName switch
        {
            "Dashboard" => _serviceProvider.GetService(typeof(DashboardViewModel)),
            "OrganizeFiles" => _serviceProvider.GetService(typeof(OrganizeFilesViewModel)),
            "Categories" => _serviceProvider.GetService(typeof(CategoriesViewModel)),
            "Logs" => _serviceProvider.GetService(typeof(LogsViewModel)),
            "Settings" => _serviceProvider.GetService(typeof(SettingsViewModel)),
            _ => CurrentViewModel
        };
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