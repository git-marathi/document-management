using Microsoft.VisualBasic.FileIO;
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

    public async Task<IReadOnlyList<FileOperationPlan>> BuildOrganizePreviewAsync(string sourceFolder, string? destinationFolder, bool includeSubfolders, bool copyFiles)
    {
        if (string.IsNullOrEmpty(sourceFolder) || !Directory.Exists(sourceFolder))
            throw new ArgumentException("Invalid source folder");

        string targetRoot = destinationFolder ?? sourceFolder;
        var plans = new List<FileOperationPlan>();

        var files = includeSubfolders
            ? Directory.EnumerateFiles(sourceFolder, "*", System.IO.SearchOption.AllDirectories)
            : Directory.EnumerateFiles(sourceFolder);

        foreach (var file in files)
        {
            string category = GetCategoryForFile(file);
            string targetDir = Path.Combine(targetRoot, category);
            string targetFile = Path.Combine(targetDir, Path.GetFileName(file));
            bool hasConflict = File.Exists(targetFile);

            plans.Add(copyFiles
                ? FileOperationPlan.CreateCopy(file, targetFile, hasConflict)
                : FileOperationPlan.CreateMove(file, targetFile, hasConflict));
        }

        return await Task.FromResult(plans);
    }

    public async Task ExecutePlanAsync(IEnumerable<FileOperationPlan> plan, bool overwrite, bool dryRun, IProgress<string> progress, CancellationToken cancellationToken)
    {
        foreach (var item in plan)
        {
            cancellationToken.ThrowIfCancellationRequested();
            progress.Report(item.DisplayText);

            if (dryRun)
            {
                _loggingService.Log(item.DisplayText);
                continue;
            }

            switch (item.OperationType)
            {
                case FileOperationType.Move:
                    await ExecuteMoveAsync(item, overwrite, cancellationToken);
                    break;
                case FileOperationType.Copy:
                    await ExecuteCopyAsync(item, overwrite, cancellationToken);
                    break;
                case FileOperationType.Delete:
                    await ExecuteDeleteAsync(item, cancellationToken);
                    break;
                case FileOperationType.Recycle:
                    await ExecuteRecycleAsync(item, cancellationToken);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public async Task CreateFolderAsync(string path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Folder path is required.");

        cancellationToken.ThrowIfCancellationRequested();
        Directory.CreateDirectory(path);
        _loggingService.Log($"Created folder: {path}");
        await Task.CompletedTask;
    }

    public async Task MergeFoldersAsync(string sourceFolder, string destinationFolder, bool overwrite, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(sourceFolder) || !Directory.Exists(sourceFolder))
            throw new ArgumentException("Invalid source folder");

        if (string.IsNullOrEmpty(destinationFolder))
            throw new ArgumentException("Invalid destination folder");

        foreach (var file in Directory.EnumerateFiles(sourceFolder, "*", System.IO.SearchOption.AllDirectories))
        {
            cancellationToken.ThrowIfCancellationRequested();

            string relativePath = Path.GetRelativePath(sourceFolder, file);
            string targetFile = Path.Combine(destinationFolder, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

            if (File.Exists(targetFile))
            {
                if (overwrite)
                {
                    File.Delete(targetFile);
                }
                else
                {
                    targetFile = GetUniqueFileName(targetFile);
                }
            }

            File.Copy(file, targetFile);
            _loggingService.Log($"Merged: {file} → {targetFile}");
        }

        await Task.CompletedTask;
    }

    public async Task<IEnumerable<FileOperationPlan>> BuildDeletePreviewAsync(string sourceFolder, bool includeSubfolders, bool sendToRecycleBin)
    {
        if (string.IsNullOrEmpty(sourceFolder) || !Directory.Exists(sourceFolder))
            throw new ArgumentException("Invalid source folder");

        var plans = new List<FileOperationPlan>();
        var files = includeSubfolders
            ? Directory.EnumerateFiles(sourceFolder, "*", System.IO.SearchOption.AllDirectories)
            : Directory.EnumerateFiles(sourceFolder);

        foreach (var file in files)
        {
            plans.Add(sendToRecycleBin
                ? FileOperationPlan.CreateRecycle(file)
                : FileOperationPlan.CreateDelete(file));
        }

        return await Task.FromResult(plans);
    }

    private async Task ExecuteMoveAsync(FileOperationPlan item, bool overwrite, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Directory.CreateDirectory(Path.GetDirectoryName(item.TargetPath)!);

        if (File.Exists(item.TargetPath))
        {
            if (overwrite)
            {
                File.Delete(item.TargetPath);
            }
            else
            {
                item.TargetPath = GetUniqueFileName(item.TargetPath);
            }
        }

        File.Move(item.SourcePath, item.TargetPath);
        _loggingService.Log($"Moved: {item.SourcePath} → {item.TargetPath}");
        await Task.CompletedTask;
    }

    private async Task ExecuteCopyAsync(FileOperationPlan item, bool overwrite, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Directory.CreateDirectory(Path.GetDirectoryName(item.TargetPath)!);

        if (File.Exists(item.TargetPath))
        {
            if (overwrite)
            {
                File.Delete(item.TargetPath);
            }
            else
            {
                item.TargetPath = GetUniqueFileName(item.TargetPath);
            }
        }

        File.Copy(item.SourcePath, item.TargetPath);
        _loggingService.Log($"Copied: {item.SourcePath} → {item.TargetPath}");
        await Task.CompletedTask;
    }

    private async Task ExecuteDeleteAsync(FileOperationPlan item, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        File.Delete(item.SourcePath);
        _loggingService.Log($"Deleted: {item.SourcePath}");
        await Task.CompletedTask;
    }

    private async Task ExecuteRecycleAsync(FileOperationPlan item, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        FileSystem.DeleteFile(item.SourcePath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        _loggingService.Log($"Recycled: {item.SourcePath}");
        await Task.CompletedTask;
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