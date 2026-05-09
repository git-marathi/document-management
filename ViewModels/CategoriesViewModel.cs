using CommunityToolkit.Mvvm.ComponentModel;
using SmartFileOrganizer.Models;
using System.Collections.ObjectModel;

namespace SmartFileOrganizer.ViewModels;

public partial class CategoriesViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<FileCategory> categories;

    public CategoriesViewModel(ObservableCollection<FileCategory> categories)
    {
        Categories = categories;
    }
}