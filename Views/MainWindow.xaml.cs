using SmartFileOrganizer.ViewModels;
using System.Windows;

namespace SmartFileOrganizer.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = (MainViewModel)App.ServiceProvider.GetService(typeof(MainViewModel))!;
    }
}