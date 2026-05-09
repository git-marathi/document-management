using System.Collections.ObjectModel;

namespace SmartFileOrganizer.Models;

public class FileCategory
{
    public string Name { get; set; }
    public ObservableCollection<string> Extensions { get; set; }

    public FileCategory(string name, IEnumerable<string> extensions)
    {
        Name = name;
        Extensions = new ObservableCollection<string>(extensions);
    }
}