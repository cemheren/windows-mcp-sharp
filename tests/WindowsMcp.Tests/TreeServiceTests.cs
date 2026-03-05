using WindowsMcp.Tree;
using WindowsMcp.Uia;
using Xunit;

namespace WindowsMcp.Tests;

public class AppNameCorrectionTests
{
    [Fact]
    public void Progman()
    {
        Assert.Equal("Desktop", TreeService.AppNameCorrection("Progman"));
    }

    [Fact]
    public void ShellTrayWnd()
    {
        Assert.Equal("Taskbar", TreeService.AppNameCorrection("Shell_TrayWnd"));
    }

    [Fact]
    public void ShellSecondaryTrayWnd()
    {
        Assert.Equal("Taskbar", TreeService.AppNameCorrection("Shell_SecondaryTrayWnd"));
    }

    [Fact]
    public void PopupWindowSiteBridge()
    {
        Assert.Equal("Context Menu",
            TreeService.AppNameCorrection("Microsoft.UI.Content.PopupWindowSiteBridge"));
    }

    [Theory]
    [InlineData("Notepad")]
    [InlineData("Calculator")]
    public void Passthrough(string appName)
    {
        Assert.Equal(appName, TreeService.AppNameCorrection(appName));
    }
}

public class IouBoundingBoxTests
{
    private static TreeService CreateTreeService(int screenWidth = 1920, int screenHeight = 1080)
    {
        return new TreeService(new object(), screenWidth, screenHeight);
    }

    [Fact]
    public void FullOverlap()
    {
        var tree = CreateTreeService();
        var window = new Rect(0, 0, 500, 500);
        var element = new Rect(100, 100, 200, 200);

        var result = tree.IouBoundingBox(window, element);

        Assert.Equal(100, result.Left);
        Assert.Equal(100, result.Top);
        Assert.Equal(200, result.Right);
        Assert.Equal(200, result.Bottom);
        Assert.Equal(100, result.Width);
        Assert.Equal(100, result.Height);
    }

    [Fact]
    public void PartialOverlap()
    {
        var tree = CreateTreeService();
        var window = new Rect(0, 0, 150, 150);
        var element = new Rect(100, 100, 200, 200);

        var result = tree.IouBoundingBox(window, element);

        Assert.Equal(100, result.Left);
        Assert.Equal(100, result.Top);
        Assert.Equal(150, result.Right);
        Assert.Equal(150, result.Bottom);
        Assert.Equal(50, result.Width);
        Assert.Equal(50, result.Height);
    }

    [Fact]
    public void NoOverlap()
    {
        var tree = CreateTreeService();
        var window = new Rect(0, 0, 50, 50);
        var element = new Rect(100, 100, 200, 200);

        var result = tree.IouBoundingBox(window, element);

        Assert.Equal(0, result.Width);
        Assert.Equal(0, result.Height);
    }

    [Fact]
    public void ScreenClamping()
    {
        // Screen is 1920x1080; element extends beyond
        var tree = CreateTreeService(1920, 1080);
        var window = new Rect(0, 0, 2000, 2000);
        var element = new Rect(1900, 1060, 2000, 1200);

        var result = tree.IouBoundingBox(window, element);

        Assert.Equal(1900, result.Left);
        Assert.Equal(1060, result.Top);
        Assert.Equal(1920, result.Right);
        Assert.Equal(1080, result.Bottom);
        Assert.Equal(20, result.Width);
        Assert.Equal(20, result.Height);
    }
}
