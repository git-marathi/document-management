using SmartFileOrganizer.Models;
using SmartFileOrganizer.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SmartFileOrganizer.Views;

/// <summary>
/// Interaction logic for OrganizeFilesView.xaml
/// </summary>
public partial class OrganizeFilesView : System.Windows.Controls.UserControl
{
    public OrganizeFilesView()
    {
        InitializeComponent();
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is DirectoryItem selectedItem && DataContext is OrganizeFilesViewModel viewModel)
        {
            // For now, set to source folder. Could add logic to choose source or destination
            viewModel.SourceFolder = selectedItem.FullPath;
        }
    }

    private void TreeView_Drop(object sender, System.Windows.DragEventArgs e)
    {
        if (DataContext is OrganizeFilesViewModel viewModel)
        {
            var data = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
            if (data != null && data.Length > 0)
            {
                var path = data[0];
                if (Directory.Exists(path) || File.Exists(path))
                {
                    viewModel.SourceFolder = path;
                }
            }
        }
    }
}