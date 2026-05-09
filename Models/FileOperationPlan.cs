using System.IO;

namespace SmartFileOrganizer.Models;

public enum FileOperationType
{
    Move,
    Copy,
    Delete,
    Recycle
}

public class FileOperationPlan
{
    public FileOperationType OperationType { get; set; }
    public string SourcePath { get; set; } = string.Empty;
    public string TargetPath { get; set; } = string.Empty;
    public string DisplayText { get; set; } = string.Empty;
    public bool RequiresDirectory { get; set; }
    public bool HasConflict { get; set; }

    public static FileOperationPlan CreateMove(string sourcePath, string targetPath, bool hasConflict)
        => new FileOperationPlan
        {
            OperationType = FileOperationType.Move,
            SourcePath = sourcePath,
            TargetPath = targetPath,
            DisplayText = $"Move {Path.GetFileName(sourcePath)} → {targetPath}",
            RequiresDirectory = true,
            HasConflict = hasConflict
        };

    public static FileOperationPlan CreateCopy(string sourcePath, string targetPath, bool hasConflict)
        => new FileOperationPlan
        {
            OperationType = FileOperationType.Copy,
            SourcePath = sourcePath,
            TargetPath = targetPath,
            DisplayText = $"Copy {Path.GetFileName(sourcePath)} → {targetPath}",
            RequiresDirectory = true,
            HasConflict = hasConflict
        };

    public static FileOperationPlan CreateDelete(string sourcePath)
        => new FileOperationPlan
        {
            OperationType = FileOperationType.Delete,
            SourcePath = sourcePath,
            DisplayText = $"Delete {Path.GetFileName(sourcePath)}",
            RequiresDirectory = false,
            HasConflict = false
        };

    public static FileOperationPlan CreateRecycle(string sourcePath)
        => new FileOperationPlan
        {
            OperationType = FileOperationType.Recycle,
            SourcePath = sourcePath,
            DisplayText = $"Recycle {Path.GetFileName(sourcePath)}",
            RequiresDirectory = false,
            HasConflict = false
        };
}