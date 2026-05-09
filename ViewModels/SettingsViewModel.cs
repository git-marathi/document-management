using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartFileOrganizer.Models;
using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;
using System.IO;

namespace SmartFileOrganizer.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private static readonly string DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SmartFileOrganizer", "settings.db");

    public IEnumerable<string> OperationTypes => Enum.GetNames(typeof(FileOperationType));

    [ObservableProperty]
    private bool isDarkThemeEnabled;

    [ObservableProperty]
    private bool showPreview = true;

    [ObservableProperty]
    private bool autoOverwrite;

    [ObservableProperty]
    private string defaultOperation = "Move";

    public SettingsViewModel()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);
        LoadSettings();
    }

    private void LoadSettings()
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Settings (
                    Key TEXT PRIMARY KEY,
                    Value TEXT
                );
            ";
            command.ExecuteNonQuery();

            // Load IsDarkThemeEnabled
            command.CommandText = "SELECT Value FROM Settings WHERE Key = 'IsDarkThemeEnabled'";
            var result = command.ExecuteScalar();
            IsDarkThemeEnabled = result != null && bool.TryParse(result.ToString(), out var b) && b;

            // Load ShowPreview
            command.CommandText = "SELECT Value FROM Settings WHERE Key = 'ShowPreview'";
            result = command.ExecuteScalar();
            ShowPreview = result == null || !bool.TryParse(result.ToString(), out var sp) || sp;

            // Load AutoOverwrite
            command.CommandText = "SELECT Value FROM Settings WHERE Key = 'AutoOverwrite'";
            result = command.ExecuteScalar();
            AutoOverwrite = result != null && bool.TryParse(result.ToString(), out var ao) && ao;

            // Load DefaultOperation
            command.CommandText = "SELECT Value FROM Settings WHERE Key = 'DefaultOperation'";
            result = command.ExecuteScalar();
            DefaultOperation = result?.ToString() ?? "Move";
        }
        catch (Exception ex)
        {
            // Fallback to defaults
            IsDarkThemeEnabled = false;
            ShowPreview = true;
            AutoOverwrite = false;
            DefaultOperation = "Move";
        }
    }

    [RelayCommand]
    private void SaveSettings()
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            // Save IsDarkThemeEnabled
            using var command1 = connection.CreateCommand();
            command1.CommandText = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES ('IsDarkThemeEnabled', $value)";
            command1.Parameters.AddWithValue("$value", IsDarkThemeEnabled.ToString());
            command1.ExecuteNonQuery();

            // Save ShowPreview
            using var command2 = connection.CreateCommand();
            command2.CommandText = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES ('ShowPreview', $value)";
            command2.Parameters.AddWithValue("$value", ShowPreview.ToString());
            command2.ExecuteNonQuery();

            // Save AutoOverwrite
            using var command3 = connection.CreateCommand();
            command3.CommandText = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES ('AutoOverwrite', $value)";
            command3.Parameters.AddWithValue("$value", AutoOverwrite.ToString());
            command3.ExecuteNonQuery();

            // Save DefaultOperation
            using var command4 = connection.CreateCommand();
            command4.CommandText = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES ('DefaultOperation', $value)";
            command4.Parameters.AddWithValue("$value", DefaultOperation);
            command4.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            // Handle error, perhaps show message
        }
    }
}