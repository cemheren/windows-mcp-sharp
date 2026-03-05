using WindowsMcp.FileSystem;
using Xunit;

namespace WindowsMcp.Tests;

public class ReadFileTests : IDisposable
{
    private readonly List<string> _tempDirs = [];

    private string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), $"WindowsMcpTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        _tempDirs.Add(path);
        return path;
    }

    public void Dispose()
    {
        foreach (var dir in _tempDirs)
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
    }

    [Fact]
    public void ReadEntireFile()
    {
        var tmp = CreateTempDir();
        var f = Path.Combine(tmp, "hello.txt");
        File.WriteAllText(f, "hello world");
        var result = FileSystemService.ReadFile(f);
        Assert.Contains("hello world", result);
    }

    [Fact]
    public void ReadWithOffsetAndLimit()
    {
        var tmp = CreateTempDir();
        var f = Path.Combine(tmp, "lines.txt");
        File.WriteAllText(f, "line1\nline2\nline3\nline4\n");
        var result = FileSystemService.ReadFile(f, offset: 2, limit: 2);
        Assert.Contains("line2", result);
        Assert.Contains("line3", result);
        Assert.DoesNotContain("line4", result);
    }

    [Fact]
    public void ReadNonexistent()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.ReadFile(Path.Combine(tmp, "nope.txt"));
        Assert.Contains("Error: File not found", result);
    }

    [Fact]
    public void ReadDirectoryPath()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.ReadFile(tmp);
        Assert.Contains("Error:", result);
    }
}

public class WriteFileTests : IDisposable
{
    private readonly List<string> _tempDirs = [];

    private string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), $"WindowsMcpTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        _tempDirs.Add(path);
        return path;
    }

    public void Dispose()
    {
        foreach (var dir in _tempDirs)
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
    }

    [Fact]
    public void WriteNewFile()
    {
        var tmp = CreateTempDir();
        var f = Path.Combine(tmp, "out.txt");
        var result = FileSystemService.WriteFile(f, "content");
        Assert.Contains("Written to", result);
        Assert.Equal("content", File.ReadAllText(f));
    }

    [Fact]
    public void AppendToFile()
    {
        var tmp = CreateTempDir();
        var f = Path.Combine(tmp, "out.txt");
        File.WriteAllText(f, "first");
        FileSystemService.WriteFile(f, " second", append: true);
        Assert.Equal("first second", File.ReadAllText(f));
    }

    [Fact]
    public void CreatesParentDirs()
    {
        var tmp = CreateTempDir();
        var f = Path.Combine(tmp, "a", "b", "c.txt");
        var result = FileSystemService.WriteFile(f, "deep");
        Assert.Contains("Written to", result);
        Assert.Equal("deep", File.ReadAllText(f));
    }
}

public class CopyPathTests : IDisposable
{
    private readonly List<string> _tempDirs = [];

    private string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), $"WindowsMcpTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        _tempDirs.Add(path);
        return path;
    }

    public void Dispose()
    {
        foreach (var dir in _tempDirs)
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
    }

    [Fact]
    public void CopyFile()
    {
        var tmp = CreateTempDir();
        var src = Path.Combine(tmp, "src.txt");
        File.WriteAllText(src, "data");
        var dst = Path.Combine(tmp, "dst.txt");
        var result = FileSystemService.CopyPath(src, dst);
        Assert.Contains("Copied file", result);
        Assert.Equal("data", File.ReadAllText(dst));
    }

    [Fact]
    public void CopyDirectory()
    {
        var tmp = CreateTempDir();
        var srcDir = Path.Combine(tmp, "srcdir");
        Directory.CreateDirectory(srcDir);
        File.WriteAllText(Path.Combine(srcDir, "file.txt"), "inside");
        var dstDir = Path.Combine(tmp, "dstdir");
        var result = FileSystemService.CopyPath(srcDir, dstDir);
        Assert.Contains("Copied directory", result);
        Assert.Equal("inside", File.ReadAllText(Path.Combine(dstDir, "file.txt")));
    }

    [Fact]
    public void CopySourceNotFound()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.CopyPath(Path.Combine(tmp, "nope"), Path.Combine(tmp, "dst"));
        Assert.Contains("Error: Source not found", result);
    }

    [Fact]
    public void CopyDestinationExistsNoOverwrite()
    {
        var tmp = CreateTempDir();
        var src = Path.Combine(tmp, "src.txt");
        File.WriteAllText(src, "data");
        var dst = Path.Combine(tmp, "dst.txt");
        File.WriteAllText(dst, "existing");
        var result = FileSystemService.CopyPath(src, dst);
        Assert.Contains("Error: Destination already exists", result);
    }

    [Fact]
    public void CopyDestinationExistsOverwrite()
    {
        var tmp = CreateTempDir();
        var src = Path.Combine(tmp, "src.txt");
        File.WriteAllText(src, "new");
        var dst = Path.Combine(tmp, "dst.txt");
        File.WriteAllText(dst, "old");
        var result = FileSystemService.CopyPath(src, dst, overwrite: true);
        Assert.Contains("Copied file", result);
        Assert.Equal("new", File.ReadAllText(dst));
    }
}

public class MovePathTests : IDisposable
{
    private readonly List<string> _tempDirs = [];

    private string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), $"WindowsMcpTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        _tempDirs.Add(path);
        return path;
    }

    public void Dispose()
    {
        foreach (var dir in _tempDirs)
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
    }

    [Fact]
    public void MoveFile()
    {
        var tmp = CreateTempDir();
        var src = Path.Combine(tmp, "src.txt");
        File.WriteAllText(src, "data");
        var dst = Path.Combine(tmp, "dst.txt");
        var result = FileSystemService.MovePath(src, dst);
        Assert.Contains("Moved", result);
        Assert.False(File.Exists(src));
        Assert.Equal("data", File.ReadAllText(dst));
    }

    [Fact]
    public void MoveSourceNotFound()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.MovePath(Path.Combine(tmp, "nope"), Path.Combine(tmp, "dst"));
        Assert.Contains("Error: Source not found", result);
    }

    [Fact]
    public void MoveDestinationExistsNoOverwrite()
    {
        var tmp = CreateTempDir();
        var src = Path.Combine(tmp, "src.txt");
        File.WriteAllText(src, "data");
        var dst = Path.Combine(tmp, "dst.txt");
        File.WriteAllText(dst, "existing");
        var result = FileSystemService.MovePath(src, dst);
        Assert.Contains("Error: Destination already exists", result);
    }
}

public class DeletePathTests : IDisposable
{
    private readonly List<string> _tempDirs = [];

    private string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), $"WindowsMcpTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        _tempDirs.Add(path);
        return path;
    }

    public void Dispose()
    {
        foreach (var dir in _tempDirs)
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
    }

    [Fact]
    public void DeleteFile()
    {
        var tmp = CreateTempDir();
        var f = Path.Combine(tmp, "del.txt");
        File.WriteAllText(f, "bye");
        var result = FileSystemService.DeletePath(f);
        Assert.Contains("Deleted file", result);
        Assert.False(File.Exists(f));
    }

    [Fact]
    public void DeleteEmptyDir()
    {
        var tmp = CreateTempDir();
        var d = Path.Combine(tmp, "emptydir");
        Directory.CreateDirectory(d);
        var result = FileSystemService.DeletePath(d);
        Assert.Contains("Deleted directory", result);
        Assert.False(Directory.Exists(d));
    }

    [Fact]
    public void DeleteNonemptyDirWithoutRecursive()
    {
        var tmp = CreateTempDir();
        var d = Path.Combine(tmp, "fulldir");
        Directory.CreateDirectory(d);
        File.WriteAllText(Path.Combine(d, "file.txt"), "x");
        var result = FileSystemService.DeletePath(d, recursive: false);
        Assert.Contains("Error: Directory is not empty", result);
        Assert.True(Directory.Exists(d));
    }

    [Fact]
    public void DeleteNonemptyDirRecursive()
    {
        var tmp = CreateTempDir();
        var d = Path.Combine(tmp, "fulldir");
        Directory.CreateDirectory(d);
        File.WriteAllText(Path.Combine(d, "file.txt"), "x");
        var result = FileSystemService.DeletePath(d, recursive: true);
        Assert.Contains("Deleted directory", result);
        Assert.False(Directory.Exists(d));
    }

    [Fact]
    public void DeleteNotFound()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.DeletePath(Path.Combine(tmp, "ghost"));
        Assert.Contains("Error: Path not found", result);
    }
}

public class ListDirectoryTests : IDisposable
{
    private readonly List<string> _tempDirs = [];

    private string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), $"WindowsMcpTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        _tempDirs.Add(path);
        return path;
    }

    public void Dispose()
    {
        foreach (var dir in _tempDirs)
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
    }

    [Fact]
    public void ListBasic()
    {
        var tmp = CreateTempDir();
        File.WriteAllText(Path.Combine(tmp, "a.txt"), "a");
        File.WriteAllText(Path.Combine(tmp, "b.txt"), "b");
        var result = FileSystemService.ListDirectory(tmp);
        Assert.Contains("a.txt", result);
        Assert.Contains("b.txt", result);
    }

    [Fact]
    public void ListWithPattern()
    {
        var tmp = CreateTempDir();
        File.WriteAllText(Path.Combine(tmp, "hello.py"), "x");
        File.WriteAllText(Path.Combine(tmp, "hello.txt"), "x");
        var result = FileSystemService.ListDirectory(tmp, pattern: "*.py");
        Assert.Contains("hello.py", result);
        Assert.DoesNotContain("hello.txt", result);
    }

    [Fact]
    public void ListHidesHiddenByDefault()
    {
        var tmp = CreateTempDir();
        File.WriteAllText(Path.Combine(tmp, ".hidden"), "x");
        File.WriteAllText(Path.Combine(tmp, "visible"), "x");
        var result = FileSystemService.ListDirectory(tmp);
        Assert.DoesNotContain(".hidden", result);
        Assert.Contains("visible", result);
    }

    [Fact]
    public void ListShowsHiddenWhenEnabled()
    {
        var tmp = CreateTempDir();
        File.WriteAllText(Path.Combine(tmp, ".hidden"), "x");
        var result = FileSystemService.ListDirectory(tmp, showHidden: true);
        Assert.Contains(".hidden", result);
    }

    [Fact]
    public void ListEmpty()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.ListDirectory(tmp);
        Assert.Contains("empty", result.ToLower());
    }

    [Fact]
    public void ListNotFound()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.ListDirectory(Path.Combine(tmp, "nope"));
        Assert.Contains("Error: Directory not found", result);
    }

    [Fact]
    public void DirsListedBeforeFiles()
    {
        var tmp = CreateTempDir();
        File.WriteAllText(Path.Combine(tmp, "z_file.txt"), "x");
        Directory.CreateDirectory(Path.Combine(tmp, "a_dir"));
        var result = FileSystemService.ListDirectory(tmp);
        var dirPos = result.IndexOf("a_dir");
        var filePos = result.IndexOf("z_file.txt");
        Assert.True(dirPos < filePos);
    }
}

public class SearchFilesTests : IDisposable
{
    private readonly List<string> _tempDirs = [];

    private string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), $"WindowsMcpTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        _tempDirs.Add(path);
        return path;
    }

    public void Dispose()
    {
        foreach (var dir in _tempDirs)
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
    }

    [Fact]
    public void SearchBasic()
    {
        var tmp = CreateTempDir();
        File.WriteAllText(Path.Combine(tmp, "a.py"), "x");
        File.WriteAllText(Path.Combine(tmp, "b.txt"), "x");
        var result = FileSystemService.SearchFiles(tmp, "*.py");
        Assert.Contains("a.py", result);
        Assert.DoesNotContain("b.txt", result);
    }

    [Fact]
    public void SearchRecursive()
    {
        var tmp = CreateTempDir();
        var sub = Path.Combine(tmp, "sub");
        Directory.CreateDirectory(sub);
        File.WriteAllText(Path.Combine(sub, "deep.py"), "x");
        var result = FileSystemService.SearchFiles(tmp, "*.py", recursive: true);
        Assert.Contains("deep.py", result);
    }

    [Fact]
    public void SearchNoMatches()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.SearchFiles(tmp, "*.xyz");
        Assert.Contains("No matches found", result);
    }

    [Fact]
    public void SearchPathNotFound()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.SearchFiles(Path.Combine(tmp, "nope"), "*.py");
        Assert.Contains("Error: Search path not found", result);
    }
}

public class GetFileInfoTests : IDisposable
{
    private readonly List<string> _tempDirs = [];

    private string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), $"WindowsMcpTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        _tempDirs.Add(path);
        return path;
    }

    public void Dispose()
    {
        foreach (var dir in _tempDirs)
            try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { }
    }

    [Fact]
    public void FileInfo_Basic()
    {
        var tmp = CreateTempDir();
        var f = Path.Combine(tmp, "info.txt");
        File.WriteAllText(f, "hello");
        var result = FileSystemService.GetFileInfo(f);
        Assert.Contains("Type: File", result);
        Assert.Contains("Extension: .txt", result);
        Assert.Contains("5", result);
    }

    [Fact]
    public void DirInfo_Basic()
    {
        var tmp = CreateTempDir();
        var d = Path.Combine(tmp, "mydir");
        Directory.CreateDirectory(d);
        File.WriteAllText(Path.Combine(d, "child.txt"), "x");
        var result = FileSystemService.GetFileInfo(d);
        Assert.Contains("Type: Directory", result);
        Assert.Contains("1 files", result);
    }

    [Fact]
    public void NotFound()
    {
        var tmp = CreateTempDir();
        var result = FileSystemService.GetFileInfo(Path.Combine(tmp, "nope"));
        Assert.Contains("Error: Path not found", result);
    }
}
