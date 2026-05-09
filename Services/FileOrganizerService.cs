using SmartFileOrganizer.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace SmartFileOrganizer.Services;

public class FileOrganizerService
{
    private readonly ObservableCollection<FileCategory> _categories;
    private readonly LoggingService _loggingService;

    public FileOrganizerService(ObservableCollection<FileCategory> categories, LoggingService loggingService)
    {
        _categories = categories;
        _loggingService = loggingService;
    }

    public async Task OrganizeFilesAsync(string sourceFolder, string? destinationFolder, bool includeSubfolders, IProgress<string> progress, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(sourceFolder) || !Directory.Exists(sourceFolder))
            throw new ArgumentException("Invalid source folder");

        string targetRoot = destinationFolder ?? sourceFolder;

        var files = includeSubfolders
            ? Directory.EnumerateFiles(sourceFolder, "*", SearchOption.AllDirectories)
            : Directory.EnumerateFiles(sourceFolder);

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string relativePath = Path.GetRelativePath(sourceFolder, file);
            string category = GetCategoryForFile(file);
            string targetDir = Path.Combine(targetRoot, category);

            Directory.CreateDirectory(targetDir);

            string targetFile = Path.Combine(targetDir, Path.GetFileName(file));

            // Handle conflicts
            if (File.Exists(targetFile))
            {
                targetFile = GetUniqueFileName(targetFile);
            }

            File.Move(file, targetFile);

            progress.Report($"Moved: {Path.GetFileName(file)} to {category}");

            _loggingService.LogMovedFile(Path.GetFileName(file), category);

            await Task.Delay(10); // Simulate work
        }
    }

    private string GetCategoryForFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLowerInvariant();

        foreach (var category in _categories)
        {
            if (category.Extensions.Contains(extension))
            {
                return category.Name;
            }
        }

        return "Other";
    }

    private string GetUniqueFileName(string filePath)
    {
        string directory = Path.GetDirectoryName(filePath)!;
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string extension = Path.GetExtension(filePath);

        int counter = 1;
        string newFilePath;
        do
        {
            newFilePath = Path.Combine(directory, $"{fileName}({counter}){extension}");
            counter++;
        } while (File.Exists(newFilePath));

        return newFilePath;
    }
}