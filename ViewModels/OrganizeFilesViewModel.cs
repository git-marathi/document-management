using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartFileOrganizer.Models;
using SmartFileOrganizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace SmartFileOrganizer.ViewModels;

public partial class OrganizeFilesViewModel : BaseViewModel
{
    private readonly FileOrganizerService _organizerService;
    private readonly ObservableCollection<FileCategory> _categories;
    private CancellationTokenSource? _cancellationTokenSource;

    public ObservableCollection<FileOperationPlan> PreviewPlan { get; } = new ObservableCollection<FileOperationPlan>();
    public IEnumerable<FileOperationType> OperationTypes => Enum.GetValues<FileOperationType>();

    [ObservableProperty]
    private FileOperationType selectedOperation = FileOperationType.Move;

    [ObservableProperty]
    private string sourceFolder = string.Empty;

    [ObservableProperty]
    private string destinationFolder = string.Empty;

    [ObservableProperty]
    private bool includeSubfolders = true;

    [ObservableProperty]
    private bool overwriteExisting;

    [ObservableProperty]
    private bool dryRun;

    [ObservableProperty]
    private bool isProcessing;

    [ObservableProperty]
    private double progress;

    [ObservableProperty]
    private string currentFile = string.Empty;

    [ObservableProperty]
    private string statusMessage = "Ready";

    [ObservableProperty]
    private int totalPreviewItems;

    [ObservableProperty]
    private string directorySearchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<DirectoryItem> directoryItems = new();

    partial void OnDirectorySearchTextChanged(string value)
    {
        // Implement filtering if needed
    }

    public OrganizeFilesViewModel(FileOrganizerService organizerService, ObservableCollection<FileCategory> categories)
    {
        _organizerService = organizerService;
        _categories = categories;
        LoadDirectoryTree();
    }

    [RelayCommand]
    private void SelectSourceFolder()
    {
        using var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            SourceFolder = dialog.SelectedPath;
        }
    }

    [RelayCommand]
    private void SelectDestinationFolder()
    {
        using var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            DestinationFolder = dialog.SelectedPath;
        }
    }

    [RelayCommand]
    private async Task PreviewAsync()
    {
        PreviewPlan.Clear();
        StatusMessage = "Building preview...";

        if (string.IsNullOrWhiteSpace(SourceFolder) || !Directory.Exists(SourceFolder))
        {
            StatusMessage = "Please select a valid source folder.";
            return;
        }

        IEnumerable<FileOperationPlan> plan;
        if (SelectedOperation == FileOperationType.Delete || SelectedOperation == FileOperationType.Recycle)
        {
            plan = await _organizerService.BuildDeletePreviewAsync(SourceFolder, IncludeSubfolders, SelectedOperation == FileOperationType.Recycle);
        }
        else
        {
            plan = await _organizerService.BuildOrganizePreviewAsync(SourceFolder, DestinationFolder, IncludeSubfolders, SelectedOperation == FileOperationType.Copy);
        }

        foreach (var item in plan)
        {
            PreviewPlan.Add(item);
        }

        TotalPreviewItems = PreviewPlan.Count;
        StatusMessage = PreviewPlan.Count == 0 ? "No files found for preview." : $"Preview ready: {PreviewPlan.Count} items.";
    }

    [RelayCommand]
    private async Task ExecuteAsync()
    {
        if (PreviewPlan.Count == 0)
        {
            StatusMessage = "Preview list is empty. Run preview first.";
            return;
        }

        IsProcessing = true;
        Progress = 0;
        CurrentFile = string.Empty;
        StatusMessage = "Executing operations...";
        _cancellationTokenSource = new CancellationTokenSource();

        int executed = 0;
        var progressReporter = new Progress<string>(message =>
        {
            CurrentFile = message;
            executed += 1;
            if (TotalPreviewItems > 0)
            {
                Progress = Math.Round(executed / (double)TotalPreviewItems * 100, 0);
            }
        });

        try
        {
            await _organizerService.ExecutePlanAsync(PreviewPlan, OverwriteExisting, DryRun, progressReporter, _cancellationTokenSource.Token);
            StatusMessage = DryRun ? "Dry run complete." : "Operation complete.";
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Operation canceled.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsProcessing = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private void LoadDirectoryTree()
    {
        DirectoryItems.Clear();
        foreach (var drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady)
            {
                var driveItem = new DirectoryItem(drive.RootDirectory.FullName);
                driveItem.LoadSubDirectories();
                DirectoryItems.Add(driveItem);
            }
        }
    }
}