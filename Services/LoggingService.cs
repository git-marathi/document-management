using Microsoft.Extensions.Logging;
using SmartFileOrganizer.Models;
using System.Collections.ObjectModel;

namespace SmartFileOrganizer.Services;

public class LoggingService
{
    private readonly ILogger<LoggingService> _logger;
    private readonly ObservableCollection<LogEntry> _logEntries;

    public LoggingService(ILogger<LoggingService> logger, ObservableCollection<LogEntry> logEntries)
    {
        _logger = logger;
        _logEntries = logEntries;
    }

    public void Log(string message, AppLogLevel level = AppLogLevel.Info)
    {
        var entry = new LogEntry(message, level);
        _logEntries.Add(entry);

        switch (level)
        {
            case AppLogLevel.Info:
                _logger.LogInformation(message);
                break;
            case AppLogLevel.Warning:
                _logger.LogWarning(message);
                break;
            case AppLogLevel.Error:
                _logger.LogError(message);
                break;
        }
    }

    public void LogMovedFile(string fileName, string category)
    {
        Log($"Moved: {fileName} to {category}");
    }

    public void LogError(string error)
    {
        Log(error, AppLogLevel.Error);
    }
}