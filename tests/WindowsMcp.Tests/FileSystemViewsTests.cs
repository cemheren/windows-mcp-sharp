using WindowsMcp.FileSystem;
using Xunit;

namespace WindowsMcp.Tests;

public class FormatSizeTests
{
    [Fact]
    public void Bytes()
    {
        Assert.Equal("0 B", FileSystemConstants.FormatSize(0));
        Assert.Equal("512 B", FileSystemConstants.FormatSize(512));
        Assert.Equal("1023 B", FileSystemConstants.FormatSize(1023));
    }

    [Fact]
    public void Kilobytes()
    {
        Assert.Equal("1.0 KB", FileSystemConstants.FormatSize(1024));
        Assert.Equal("1.5 KB", FileSystemConstants.FormatSize(1536));
        Assert.Equal("10.0 KB", FileSystemConstants.FormatSize(10240));
    }

    [Fact]
    public void Megabytes()
    {
        Assert.Equal("1.0 MB", FileSystemConstants.FormatSize(1024L * 1024));
        Assert.Equal("5.0 MB", FileSystemConstants.FormatSize(5L * 1024 * 1024));
    }

    [Fact]
    public void Gigabytes()
    {
        Assert.Equal("1.0 GB", FileSystemConstants.FormatSize(1024L * 1024 * 1024));
        Assert.Equal("2.0 GB", FileSystemConstants.FormatSize(2L * 1024 * 1024 * 1024));
    }
}

public class FileSystemConstantsTests
{
    [Fact]
    public void MaxReadSize() => Assert.Equal(10 * 1024 * 1024, FileSystemConstants.MaxReadSize);

    [Fact]
    public void MaxResults() => Assert.Equal(500, FileSystemConstants.MaxResults);
}

public class FileEntryTests
{
    private static FileEntry MakeFile(
        string path = @"C:\test\file.txt",
        string type = "File",
        long size = 2048,
        bool readOnly = false,
        string? extension = null,
        string? linkTarget = null,
        int? contentsFiles = null,
        int? contentsDirs = null) =>
        new()
        {
            Path = path,
            Type = type,
            Size = size,
            Created = new DateTime(2025, 1, 15, 10, 30, 0),
            Modified = new DateTime(2025, 6, 20, 14, 0, 0),
            Accessed = new DateTime(2025, 6, 21, 9, 0, 0),
            ReadOnly = readOnly,
            Extension = extension,
            LinkTarget = linkTarget,
            ContentsFiles = contentsFiles,
            ContentsDirs = contentsDirs,
        };

    [Fact]
    public void ToString_Basic()
    {
        var f = MakeFile();
        var result = f.ToString();
        Assert.Contains(@"C:\test\file.txt", result);
        Assert.Contains("Type: File", result);
        Assert.Contains("2.0 KB", result);
        Assert.Contains("2025-01-15 10:30:00", result);
        Assert.Contains("Read-only: False", result);
    }

    [Fact]
    public void ToString_WithExtension()
    {
        var result = MakeFile(extension: ".txt").ToString();
        Assert.Contains("Extension: .txt", result);
    }

    [Fact]
    public void ToString_WithoutExtension()
    {
        Assert.DoesNotContain("Extension", MakeFile().ToString());
    }

    [Fact]
    public void ToString_WithContents()
    {
        var result = MakeFile(contentsFiles: 10, contentsDirs: 3).ToString();
        Assert.Contains("Contents: 10 files, 3 directories", result);
    }

    [Fact]
    public void ToString_WithoutContents()
    {
        Assert.DoesNotContain("Contents", MakeFile().ToString());
    }

    [Fact]
    public void ToString_WithLinkTarget()
    {
        var result = MakeFile(linkTarget: @"C:\actual\target.txt").ToString();
        Assert.Contains(@"Link target: C:\actual\target.txt", result);
    }

    [Fact]
    public void ToString_ReadOnly()
    {
        Assert.Contains("Read-only: True", MakeFile(readOnly: true).ToString());
    }
}

public class DirectoryEntryTests
{
    [Fact]
    public void FileEntry_ToString()
    {
        var d = new DirectoryEntry { Name = "readme.md", IsDir = false, Size = 4096 };
        var result = d.ToString();
        Assert.Contains("[FILE]", result);
        Assert.Contains("readme.md", result);
        Assert.Contains("4.0 KB", result);
    }

    [Fact]
    public void DirEntry_ToString()
    {
        var d = new DirectoryEntry { Name = "src", IsDir = true };
        var result = d.ToString();
        Assert.Contains("[DIR ]", result);
        Assert.Contains("src", result);
    }

    [Fact]
    public void DirEntry_NoSize()
    {
        var d = new DirectoryEntry { Name = "build", IsDir = true, Size = 999 };
        var result = d.ToString();
        Assert.DoesNotContain("999", result);
    }

    [Fact]
    public void RelativePath_Override()
    {
        var d = new DirectoryEntry { Name = "file.py", IsDir = false, Size = 100 };
        var result = d.ToString(relativePath: "sub/file.py");
        Assert.Contains("sub/file.py", result);
        Assert.Equal(1, result.Split("file.py").Length - 1);
    }

    [Fact]
    public void DefaultSize_Zero()
    {
        var d = new DirectoryEntry { Name = "test", IsDir = false };
        Assert.Equal(0, d.Size);
    }
}
