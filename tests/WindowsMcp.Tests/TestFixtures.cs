using WindowsMcp.Desktop;
using WindowsMcp.Tree;

namespace WindowsMcp.Tests;

/// <summary>
/// Shared test fixtures providing sample objects (equivalent to pytest conftest.py).
/// </summary>
public static class TestFixtures
{
    public static BoundingBox CreateSampleBoundingBox() =>
        new(Left: 100, Top: 50, Right: 300, Bottom: 150, Width: 200, Height: 100);

    public static Center CreateSampleCenter() =>
        new(X: 200, Y: 100);

    public static TreeElementNode CreateSampleTreeElementNode(
        BoundingBox? boundingBox = null, Center? center = null) =>
        new()
        {
            BoundingBox = boundingBox ?? CreateSampleBoundingBox(),
            Center = center ?? CreateSampleCenter(),
            Name = "OK",
            ControlType = "Button",
            WindowName = "Notepad",
            Value = "",
            Shortcut = "Alt+O",
            Xpath = "/Pane/Button",
            IsFocused = true,
        };

    public static ScrollElementNode CreateSampleScrollElementNode(
        BoundingBox? boundingBox = null, Center? center = null) =>
        new()
        {
            Name = "Document",
            ControlType = "Pane",
            Xpath = "/Pane/ScrollViewer",
            WindowName = "Notepad",
            BoundingBox = boundingBox ?? CreateSampleBoundingBox(),
            Center = center ?? CreateSampleCenter(),
            HorizontalScrollable = false,
            HorizontalScrollPercent = 0.0,
            VerticalScrollable = true,
            VerticalScrollPercent = 42.5,
            IsFocused = false,
        };

    public static Window CreateSampleWindow(BoundingBox? boundingBox = null) =>
        new()
        {
            Name = "Untitled - Notepad",
            IsBrowser = false,
            Depth = 0,
            Status = Status.Normal,
            BoundingBox = boundingBox ?? CreateSampleBoundingBox(),
            Handle = 12345,
            ProcessId = 6789,
        };

    public static DesktopState CreateSampleDesktopState(Window? window = null)
    {
        var w = window ?? CreateSampleWindow();
        return new DesktopState
        {
            ActiveDesktop = new Dictionary<string, object?> { { "name", "Desktop 1" }, { "id", "abc-123" } },
            AllDesktops =
            [
                new Dictionary<string, object?> { { "name", "Desktop 1" }, { "id", "abc-123" } },
                new Dictionary<string, object?> { { "name", "Desktop 2" }, { "id", "def-456" } },
            ],
            ActiveWindow = w,
            Windows = [w],
        };
    }
}
