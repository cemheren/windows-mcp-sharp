using WindowsMcp.Desktop;
using Xunit;

namespace WindowsMcp.Tests;

public class BrowserTests
{
    [Fact]
    public void HasProcess_Chrome() => Assert.True(BrowserExtensions.HasProcess("chrome.exe"));

    [Fact]
    public void HasProcess_Edge() => Assert.True(BrowserExtensions.HasProcess("msedge.exe"));

    [Fact]
    public void HasProcess_Firefox() => Assert.True(BrowserExtensions.HasProcess("firefox.exe"));

    [Fact]
    public void HasProcess_CaseInsensitive()
    {
        Assert.True(BrowserExtensions.HasProcess("Chrome.EXE"));
        Assert.True(BrowserExtensions.HasProcess("MSEDGE.EXE"));
    }

    [Fact]
    public void HasProcess_Unknown() => Assert.False(BrowserExtensions.HasProcess("notepad.exe"));

    [Fact]
    public void HasProcess_EmptyString() => Assert.False(BrowserExtensions.HasProcess(""));
}

public class StatusTests
{
    [Fact]
    public void EnumValues()
    {
        Assert.Equal("Maximized", Status.Maximized.ToStringValue());
        Assert.Equal("Minimized", Status.Minimized.ToStringValue());
        Assert.Equal("Normal", Status.Normal.ToStringValue());
        Assert.Equal("Hidden", Status.Hidden.ToStringValue());
    }
}

public class SizeTests
{
    [Fact]
    public void ToString_Standard()
    {
        var s = new Size { Width = 1920, Height = 1080 };
        Assert.Equal("(1920,1080)", s.ToString());
    }

    [Fact]
    public void ToString_Zero()
    {
        var s = new Size { Width = 0, Height = 0 };
        Assert.Equal("(0,0)", s.ToString());
    }
}

public class WindowTests
{
    [Fact]
    public void ToRow_Values()
    {
        var window = TestFixtures.CreateSampleWindow();
        var row = window.ToRow();
        Assert.Equal(new object[] { "Untitled - Notepad", 0, "Normal", 200, 100, 12345 }, row);
    }
}

public class DesktopStateTests
{
    [Fact]
    public void ActiveDesktopToString_ContainsName()
    {
        var ds = TestFixtures.CreateSampleDesktopState();
        var result = ds.ActiveDesktopToString();
        Assert.Contains("Desktop 1", result);
    }

    [Fact]
    public void DesktopsToString_ContainsAll()
    {
        var ds = TestFixtures.CreateSampleDesktopState();
        var result = ds.DesktopsToString();
        Assert.Contains("Desktop 1", result);
        Assert.Contains("Desktop 2", result);
    }

    [Fact]
    public void ActiveWindowToString_None()
    {
        var ds = new DesktopState
        {
            ActiveDesktop = new Dictionary<string, object?> { { "name", "Desktop 1" } },
            AllDesktops = [],
            ActiveWindow = null,
            Windows = [],
        };
        Assert.Equal("No active window found", ds.ActiveWindowToString());
    }

    [Fact]
    public void ActiveWindowToString_WithWindow()
    {
        var ds = TestFixtures.CreateSampleDesktopState();
        var result = ds.ActiveWindowToString();
        Assert.Contains("Untitled - Notepad", result);
        Assert.Contains("Normal", result);
    }

    [Fact]
    public void WindowsToString_Empty()
    {
        var ds = new DesktopState
        {
            ActiveDesktop = new Dictionary<string, object?> { { "name", "Desktop 1" } },
            AllDesktops = [],
            ActiveWindow = null,
            Windows = [],
        };
        Assert.Equal("No windows found", ds.WindowsToString());
    }

    [Fact]
    public void WindowsToString_WithWindows()
    {
        var ds = TestFixtures.CreateSampleDesktopState();
        var result = ds.WindowsToString();
        Assert.Contains("Untitled - Notepad", result);
    }
}
