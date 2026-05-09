using System.Collections.ObjectModel;
using System.IO;

namespace SmartFileOrganizer.Models;

public class DirectoryItem
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public ObservableCollection<DirectoryItem> SubDirectories { get; set; } = new();

    public DirectoryItem(string path)
    {
        FullPath = path;
        Name = Path.GetFileName(path) ?? path;
    }

    public void LoadSubDirectories()
    {
        try
        {
            var subDirs = Directory.GetDirectories(FullPath);
            foreach (var dir in subDirs)
            {
                SubDirectories.Add(new DirectoryItem(dir));
            }
        }
        catch
        {
            // Ignore access denied
        }
    }
}