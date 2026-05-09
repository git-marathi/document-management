using System.Collections.ObjectModel;
using System.IO;

namespace SmartFileOrganizer.Models;

public class DuplicateGroup
{
    public ObservableCollection<DuplicateFile> Files { get; } = new();

    public DuplicateGroup(IEnumerable<string> filePaths)
    {
        foreach (var path in filePaths)
        {
            Files.Add(new DuplicateFile(path));
        }
    }
}

public class DuplicateFile
{
    public string FullPath { get; }
    public string Name => Path.GetFileName(FullPath);
    public long Size { get; }
    public DateTime LastModified { get; }

    public DuplicateFile(string path)
    {
        FullPath = path;
        var info = new FileInfo(path);
        Size = info.Length;
        LastModified = info.LastWriteTime;
    }
}