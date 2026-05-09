using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartFileOrganizer.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;

namespace SmartFileOrganizer.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    public string WelcomeMessage => "Welcome to Smart File Organizer";

    public ObservableCollection<Feature> Features { get; } = new();

    public DashboardViewModel()
    {
        LoadFeatures();
    }

    private void LoadFeatures()
    {
        Features.Add(new Feature
        {
            Name = "Folder Selection",
            Status = "Implemented",
            Description = "Browse and select source and destination folders with directory tree and search.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Smart File Organization",
            Status = "Implemented",
            Description = "Organize files based on categories, extensions, and custom rules.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Dynamic User Categories",
            Status = "Implemented",
            Description = "Create and manage custom file categories.",
            Link = "Navigate to Categories"
        });

        Features.Add(new Feature
        {
            Name = "Recursive Scanning",
            Status = "Implemented",
            Description = "Scan subfolders recursively for file organization.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Background Processing",
            Status = "Implemented",
            Description = "Perform operations asynchronously with progress tracking.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Progress Tracking",
            Status = "Implemented",
            Description = "Real-time progress bar and status updates during operations.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Conflict Handling",
            Status = "Implemented",
            Description = "Handle file conflicts with overwrite options and unique naming.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Preview Mode",
            Status = "Implemented",
            Description = "Preview operations before execution with detailed plan.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Logging System",
            Status = "Implemented",
            Description = "Comprehensive logging of all operations and errors.",
            Link = "Navigate to Logs"
        });

        Features.Add(new Feature
        {
            Name = "File Filters",
            Status = "Implemented",
            Description = "Filter files by extensions, size, date, and custom criteria.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Drag-and-Drop Support",
            Status = "Implemented",
            Description = "Drag files/folders from Explorer to set paths.",
            Link = "Navigate to Organize Files"
        });

        Features.Add(new Feature
        {
            Name = "Undo Feature",
            Status = "Future",
            Description = "Ability to undo recent file operations.",
            Link = "https://github.com/your-repo/issues/undo-feature"
        });

        Features.Add(new Feature
        {
            Name = "Settings Persistence",
            Status = "Future",
            Description = "Save and load user settings and preferences.",
            Link = "Navigate to Settings"
        });

        Features.Add(new Feature
        {
            Name = "Bulk Operations",
            Status = "Future",
            Description = "Perform operations on multiple folders simultaneously.",
            Link = "https://github.com/your-repo/issues/bulk-ops"
        });

        Features.Add(new Feature
        {
            Name = "Cloud Integration",
            Status = "Future",
            Description = "Sync with cloud storage services like OneDrive or Google Drive.",
            Link = "https://github.com/your-repo/issues/cloud-sync"
        });
    }

    [RelayCommand]
    private void OpenLink(string link)
    {
        if (link.StartsWith("http"))
        {
            Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
        }
        else if (link.StartsWith("Navigate to "))
        {
            var viewName = link.Replace("Navigate to ", "").Replace(" ", "");
            WeakReferenceMessenger.Default.Send(new NavigateMessage(viewName));
        }
        // For navigation links, navigate
    }
}