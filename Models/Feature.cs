namespace SmartFileOrganizer.Models;

public class Feature
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // "Implemented" or "Future"
    public string Description { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty; // URL or command
}