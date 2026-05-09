using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartFileOrganizer.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace SmartFileOrganizer.ViewModels;

public partial class DuplicateFilesViewModel : BaseViewModel
{
    [ObservableProperty]
    private string sourceFolder = string.Empty;

    [ObservableProperty]
    private bool isScanning;

    [ObservableProperty]
    private ObservableCollection<DuplicateGroup> duplicateGroups = new();

    [ObservableProperty]
    private string statusMessage = "Ready";

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
    private async Task ScanForDuplicatesAsync()
    {
        if (string.IsNullOrWhiteSpace(SourceFolder) || !Directory.Exists(SourceFolder))
        {
            StatusMessage = "Please select a valid source folder.";
            return;
        }

        IsScanning = true;
        StatusMessage = "Scanning for duplicates...";
        DuplicateGroups.Clear();

        try
        {
            var files = Directory.GetFiles(SourceFolder, "*", SearchOption.AllDirectories);
            var fileGroups = new Dictionary<string, List<string>>();

            foreach (var file in files)
            {
                var info = new FileInfo(file);
                var key = $"{info.Name}_{info.Length}";
                if (!fileGroups.ContainsKey(key))
                    fileGroups[key] = new List<string>();
                fileGroups[key].Add(file);
            }

            foreach (var group in fileGroups.Where(g => g.Value.Count > 1))
            {
                var dupGroup = new DuplicateGroup(group.Value);
                DuplicateGroups.Add(dupGroup);
            }

            StatusMessage = $"Scan complete. Found {DuplicateGroups.Count} duplicate groups.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsScanning = false;
        }
    }
}