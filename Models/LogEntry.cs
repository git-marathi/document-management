namespace SmartFileOrganizer.Models;

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public AppLogLevel Level { get; set; }

    public LogEntry(string message, AppLogLevel level = AppLogLevel.Info)
    {
        Timestamp = DateTime.Now;
        Message = message;
        Level = level;
    }
}

public enum AppLogLevel
{
    Info,
    Warning,
    Error
}