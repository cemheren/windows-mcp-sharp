using System.Text;

namespace WindowsMcp.FileSystem;

/// <summary>
/// File system service providing structured, safe file operations.
/// </summary>
public static class FileSystemService
{
    public static string ReadFile(string path, int? offset = null, int? limit = null, string encoding = "utf-8")
    {
        var filePath = Path.GetFullPath(path);

        if (!File.Exists(filePath))
            return $"Error: File not found: {filePath}";

        var fileInfo = new FileInfo(filePath);
        if (fileInfo.Attributes.HasFlag(FileAttributes.Directory))
            return $"Error: Path is not a file: {filePath}";
        if (fileInfo.Length > FileSystemConstants.MaxReadSize)
            return $"Error: File too large ({fileInfo.Length:N0} bytes). Maximum is {FileSystemConstants.MaxReadSize:N0} bytes. Use offset/limit parameters or the Shell tool for large files.";

        try
        {
            var enc = Encoding.GetEncoding(encoding);
            if (offset is not null || limit is not null)
            {
                var lines = File.ReadAllLines(filePath, enc);
                var start = Math.Max(0, (offset ?? 1) - 1);
                var end = limit is not null ? start + limit.Value : lines.Length;
                end = Math.Min(end, lines.Length);
                var selected = lines[start..end];
                var total = lines.Length;
                var content = string.Join("\n", selected);
                return $"File: {filePath}\nLines {start + 1}-{end} of {total}:\n{content}";
            }
            else
            {
                var content = File.ReadAllText(filePath, enc);
                return $"File: {filePath}\n{content}";
            }
        }
        catch (DecoderFallbackException)
        {
            return $"Error: Unable to read file as text with encoding \"{encoding}\". File may be binary.";
        }
        catch (UnauthorizedAccessException)
        {
            return $"Error: Permission denied: {filePath}";
        }
        catch (Exception e)
        {
            return $"Error reading file: {e.Message}";
        }
    }

    public static string WriteFile(string path, string content, bool append = false, string encoding = "utf-8", bool createParents = true)
    {
        var filePath = Path.GetFullPath(path);

        try
        {
            if (createParents)
            {
                var parentDir = Path.GetDirectoryName(filePath);
                if (parentDir is not null)
                    Directory.CreateDirectory(parentDir);
            }

            var enc = Encoding.GetEncoding(encoding);
            if (append)
                File.AppendAllText(filePath, content, enc);
            else
                File.WriteAllText(filePath, content, enc);

            var action = append ? "Appended to" : "Written to";
            var size = new FileInfo(filePath).Length;
            return $"{action} {filePath} ({size:N0} bytes)";
        }
        catch (UnauthorizedAccessException)
        {
            return $"Error: Permission denied: {filePath}";
        }
        catch (Exception e)
        {
            return $"Error writing file: {e.Message}";
        }
    }

    public static string CopyPath(string source, string destination, bool overwrite = false)
    {
        var src = Path.GetFullPath(source);
        var dst = Path.GetFullPath(destination);

        if (!Path.Exists(src))
            return $"Error: Source not found: {src}";
        if (Path.Exists(dst) && !overwrite)
            return $"Error: Destination already exists: {dst}. Set overwrite=True to replace.";

        try
        {
            if (File.Exists(src))
            {
                var parentDir = Path.GetDirectoryName(dst);
                if (parentDir is not null)
                    Directory.CreateDirectory(parentDir);
                File.Copy(src, dst, overwrite);
                return $"Copied file: {src} -> {dst}";
            }
            else if (Directory.Exists(src))
            {
                if (Directory.Exists(dst) && overwrite)
                    Directory.Delete(dst, recursive: true);
                CopyDirectoryRecursive(src, dst);
                return $"Copied directory: {src} -> {dst}";
            }
            else
            {
                return $"Error: Unsupported file type: {src}";
            }
        }
        catch (UnauthorizedAccessException)
        {
            return "Error: Permission denied.";
        }
        catch (Exception e)
        {
            return $"Error copying: {e.Message}";
        }
    }

    public static string MovePath(string source, string destination, bool overwrite = false)
    {
        var src = Path.GetFullPath(source);
        var dst = Path.GetFullPath(destination);

        if (!Path.Exists(src))
            return $"Error: Source not found: {src}";
        if (Path.Exists(dst) && !overwrite)
            return $"Error: Destination already exists: {dst}. Set overwrite=True to replace.";

        try
        {
            var parentDir = Path.GetDirectoryName(dst);
            if (parentDir is not null)
                Directory.CreateDirectory(parentDir);

            if (Path.Exists(dst) && overwrite)
            {
                if (Directory.Exists(dst))
                    Directory.Delete(dst, recursive: true);
                else
                    File.Delete(dst);
            }

            if (File.Exists(src))
                File.Move(src, dst);
            else
                Directory.Move(src, dst);

            return $"Moved: {src} -> {dst}";
        }
        catch (UnauthorizedAccessException)
        {
            return "Error: Permission denied.";
        }
        catch (Exception e)
        {
            return $"Error moving: {e.Message}";
        }
    }

    public static string DeletePath(string path, bool recursive = false)
    {
        var target = Path.GetFullPath(path);

        if (!Path.Exists(target))
            return $"Error: Path not found: {target}";

        try
        {
            var attrs = File.GetAttributes(target);
            var isDir = attrs.HasFlag(FileAttributes.Directory);
            var isSymlink = attrs.HasFlag(FileAttributes.ReparsePoint);

            if (!isDir)
            {
                File.Delete(target);
                return $"Deleted file: {target}";
            }

            if (isSymlink)
            {
                Directory.Delete(target);
                return $"Deleted file: {target}";
            }

            if (!recursive)
            {
                if (Directory.EnumerateFileSystemEntries(target).Any())
                    return $"Error: Directory is not empty: {target}. Set recursive=True to delete non-empty directories.";
                Directory.Delete(target);
            }
            else
            {
                Directory.Delete(target, recursive: true);
            }

            return $"Deleted directory: {target}";
        }
        catch (UnauthorizedAccessException)
        {
            return $"Error: Permission denied: {target}";
        }
        catch (Exception e)
        {
            return $"Error deleting: {e.Message}";
        }
    }

    public static string ListDirectory(string path, string? pattern = null, bool recursive = false, bool showHidden = false)
    {
        var dirPath = Path.GetFullPath(path);

        if (!Directory.Exists(dirPath))
        {
            if (File.Exists(dirPath))
                return $"Error: Path is not a directory: {dirPath}";
            return $"Error: Directory not found: {dirPath}";
        }

        try
        {
            var entries = new List<string>();
            var count = 0;

            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var iterator = Directory.EnumerateFileSystemEntries(dirPath, pattern ?? "*", searchOption);

            var sorted = iterator
                .Select(e => new { FullPath = e, Name = Path.GetFileName(e), IsDir = Directory.Exists(e) })
                .OrderBy(e => !e.IsDir)
                .ThenBy(e => e.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var entry in sorted)
            {
                if (!showHidden && entry.Name.StartsWith('.'))
                    continue;

                count++;
                if (count > FileSystemConstants.MaxResults)
                {
                    entries.Add($"... (truncated, {FileSystemConstants.MaxResults}+ items)");
                    break;
                }

                long size = 0;
                try
                {
                    size = entry.IsDir ? 0 : new FileInfo(entry.FullPath).Length;
                }
                catch (IOException) { }

                var rel = recursive
                    ? Path.GetRelativePath(dirPath, entry.FullPath)
                    : entry.Name;

                var dirEntry = new DirectoryEntry { Name = entry.Name, IsDir = entry.IsDir, Size = size };
                entries.Add(dirEntry.ToString(relativePath: rel));
            }

            if (entries.Count == 0)
            {
                var filterMsg = pattern is not null ? $" matching \"{pattern}\"" : "";
                return $"Directory {dirPath} is empty{filterMsg}.";
            }

            var header = $"Directory: {dirPath}";
            if (pattern is not null)
                header += $" (filter: {pattern})";
            return $"{header}\n{string.Join("\n", entries)}";
        }
        catch (UnauthorizedAccessException)
        {
            return $"Error: Permission denied: {dirPath}";
        }
        catch (Exception e)
        {
            return $"Error listing directory: {e.Message}";
        }
    }

    public static string SearchFiles(string path, string pattern, bool recursive = true)
    {
        var searchRoot = Path.GetFullPath(path);

        if (!Path.Exists(searchRoot))
            return $"Error: Search path not found: {searchRoot}";
        if (!Directory.Exists(searchRoot))
            return $"Error: Search path is not a directory: {searchRoot}";

        try
        {
            var results = new List<string>();
            var count = 0;

            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var iterator = Directory.EnumerateFileSystemEntries(searchRoot, pattern, searchOption);

            var sorted = iterator
                .Select(e => new { FullPath = e, Name = Path.GetFileName(e), IsDir = Directory.Exists(e) })
                .OrderBy(e => e.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var match in sorted)
            {
                count++;
                if (count > FileSystemConstants.MaxResults)
                {
                    results.Add($"... (truncated, {FileSystemConstants.MaxResults}+ matches)");
                    break;
                }

                long size = 0;
                try
                {
                    size = match.IsDir ? 0 : new FileInfo(match.FullPath).Length;
                }
                catch (IOException) { }

                var rel = Path.GetRelativePath(searchRoot, match.FullPath);
                var dirEntry = new DirectoryEntry { Name = match.Name, IsDir = match.IsDir, Size = size };
                results.Add(dirEntry.ToString(relativePath: rel));
            }

            if (results.Count == 0)
                return $"No matches found for \"{pattern}\" in {searchRoot}";

            return $"Search: \"{pattern}\" in {searchRoot} ({Math.Min(count, FileSystemConstants.MaxResults)} matches)\n{string.Join("\n", results)}";
        }
        catch (UnauthorizedAccessException)
        {
            return $"Error: Permission denied: {searchRoot}";
        }
        catch (Exception e)
        {
            return $"Error searching: {e.Message}";
        }
    }

    public static string GetFileInfo(string path)
    {
        var target = Path.GetFullPath(path);

        if (!Path.Exists(target))
            return $"Error: Path not found: {target}";

        try
        {
            var attrs = File.GetAttributes(target);
            var isDir = attrs.HasFlag(FileAttributes.Directory);
            var isSymlink = attrs.HasFlag(FileAttributes.ReparsePoint);

            string fileType;
            if (isDir) fileType = "Directory";
            else if (File.Exists(target)) fileType = "File";
            else if (isSymlink) fileType = "Symlink";
            else fileType = "Other";

            FileSystemInfo fsInfo = isDir ? new DirectoryInfo(target) : new FileInfo(target);
            var size = isDir ? 0L : ((FileInfo)fsInfo).Length;

            var fileEntry = new FileEntry
            {
                Path = target,
                Type = fileType,
                Size = size,
                Created = fsInfo.CreationTime,
                Modified = fsInfo.LastWriteTime,
                Accessed = fsInfo.LastAccessTime,
                ReadOnly = attrs.HasFlag(FileAttributes.ReadOnly),
                Extension = !isDir && fsInfo is FileInfo fi ? (fi.Extension.Length > 0 ? fi.Extension : "(none)") : null,
                LinkTarget = isSymlink ? fsInfo.LinkTarget : null,
                ContentsFiles = isDir ? SafeCount(() => Directory.EnumerateFiles(target).Count()) : null,
                ContentsDirs = isDir ? SafeCount(() => Directory.EnumerateDirectories(target).Count()) : null,
            };

            return fileEntry.ToString();
        }
        catch (UnauthorizedAccessException)
        {
            return $"Error: Permission denied: {target}";
        }
        catch (Exception e)
        {
            return $"Error getting file info: {e.Message}";
        }
    }

    private static void CopyDirectoryRecursive(string source, string destination)
    {
        Directory.CreateDirectory(destination);
        foreach (var file in Directory.EnumerateFiles(source))
            File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
        foreach (var dir in Directory.EnumerateDirectories(source))
            CopyDirectoryRecursive(dir, Path.Combine(destination, Path.GetFileName(dir)));
    }

    private static int? SafeCount(Func<int> counter)
    {
        try { return counter(); }
        catch (UnauthorizedAccessException) { return null; }
    }
}
