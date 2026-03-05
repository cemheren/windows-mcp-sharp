using System.Text;

namespace WindowsMcp.FileSystem;

/// <summary>
/// Constants for filesystem operations.
/// </summary>
public static class FileSystemConstants
{
    /// <summary>Maximum file size for read operations (10 MB).</summary>
    public const long MaxReadSize = 10 * 1024 * 1024;

    /// <summary>Maximum number of results for list/search operations.</summary>
    public const int MaxResults = 500;

    /// <summary>Formats a byte count into a human-readable string.</summary>
    public static string FormatSize(long sizeBytes)
    {
        return sizeBytes switch
        {
            < 1024L => $"{sizeBytes} B",
            < 1024L * 1024 => $"{sizeBytes / 1024.0:F1} KB",
            < 1024L * 1024 * 1024 => $"{sizeBytes / (1024.0 * 1024):F1} MB",
            _ => $"{sizeBytes / (1024.0 * 1024 * 1024):F1} GB"
        };
    }
}

/// <summary>
/// Represents detailed information about a file or directory.
/// </summary>
public sealed class FileEntry
{
    public required string Path { get; init; }
    public required string Type { get; init; }
    public required long Size { get; init; }
    public required DateTime Created { get; init; }
    public required DateTime Modified { get; init; }
    public required DateTime Accessed { get; init; }
    public required bool ReadOnly { get; init; }
    public string? Extension { get; init; }
    public string? LinkTarget { get; init; }
    public int? ContentsFiles { get; init; }
    public int? ContentsDirs { get; init; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Path: {Path}");
        sb.AppendLine($"Type: {Type}");
        sb.AppendLine($"Size: {FileSystemConstants.FormatSize(Size)} ({Size:N0} bytes)");
        sb.AppendLine($"Created: {Created:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"Modified: {Modified:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"Accessed: {Accessed:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"Read-only: {ReadOnly}");

        if (ContentsFiles is not null && ContentsDirs is not null)
            sb.AppendLine($"Contents: {ContentsFiles} files, {ContentsDirs} directories");

        if (Extension is not null)
            sb.AppendLine($"Extension: {Extension}");

        if (LinkTarget is not null)
            sb.AppendLine($"Link target: {LinkTarget}");

        return sb.ToString().TrimEnd();
    }
}

/// <summary>
/// Represents an entry in a directory listing.
/// </summary>
public sealed class DirectoryEntry
{
    public required string Name { get; init; }
    public required bool IsDir { get; init; }
    public long Size { get; init; }

    public string ToString(string? relativePath = null)
    {
        var entryType = IsDir ? "DIR " : "FILE";
        var sizeStr = IsDir ? "" : FileSystemConstants.FormatSize(Size);
        var display = relativePath ?? Name;
        return $"  [{entryType}] {display}  {sizeStr}";
    }

    public override string ToString() => ToString(relativePath: null);
}


