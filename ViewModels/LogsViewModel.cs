using CommunityToolkit.Mvvm.ComponentModel;
using SmartFileOrganizer.Models;
using System.Collections.ObjectModel;

namespace SmartFileOrganizer.ViewModels;

public partial class LogsViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<LogEntry> logEntries;

    public LogsViewModel(ObservableCollection<LogEntry> logEntries)
    {
        LogEntries = logEntries;
    }
}