using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SmartFileOrganizer.Models;
using SmartFileOrganizer.Services;
using System.Collections.ObjectModel;

namespace SmartFileOrganizer.ViewModels;

public partial class OrganizeFilesViewModel : BaseViewModel
{
    private readonly FileOrganizerService _organizerService;
    private readonly ObservableCollection<FileCategory> _categories;

    [ObservableProperty]
    private string sourceFolder = string.Empty;

    [ObservableProperty]
    private string destinationFolder = string.Empty;

    [ObservableProperty]
    private bool includeSubfolders = true;

    [ObservableProperty]
    private bool isProcessing;

    [ObservableProperty]
    private double progress;

    [ObservableProperty]
    private string currentFile = string.Empty;

    private CancellationTokenSource? _cancellationTokenSource;

    public OrganizeFilesViewModel(FileOrganizerService organizerService, ObservableCollection<FileCategory> categories)
    {
        _organizerService = organizerService;
        _categories = categories;
    }

    [RelayCommand]
    private void SelectSourceFolder()
    {
        var dialog = new OpenFolderDialog();
        if (dialog.ShowDialog() == true)
        {
            SourceFolder = dialog.FolderName;
        }
    }

    [RelayCommand]
    private void SelectDestinationFolder()
    {
        var dialog = new OpenFolderDialog();
        if (dialog.ShowDialog() == true)
        {
            DestinationFolder = dialog.FolderName;
        }
    }

    [RelayCommand]
    private async Task OrganizeFilesAsync()
    {
        if (string.IsNullOrEmpty(SourceFolder))
            return;

        IsProcessing = true;
        Progress = 0;
        CurrentFile = string.Empty;

        _cancellationTokenSource = new CancellationTokenSource();

        var progressReporter = new Progress<string>(file => CurrentFile = file);

        try
        {
            await _organizerService.OrganizeFilesAsync(SourceFolder, DestinationFolder, IncludeSubfolders, progressReporter, _cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation
        }
        finally
        {
            IsProcessing = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    [RelayCommand]
    private void CancelOrganization()
    {
        _cancellationTokenSource?.Cancel();
    }
}