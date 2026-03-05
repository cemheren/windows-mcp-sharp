using System.Text;
using WindowsMcp.Tree;

namespace WindowsMcp.Desktop;

public enum Browser
{
    Chrome,
    Edge,
    Firefox,
}

public static class BrowserExtensions
{
    private static readonly Dictionary<Browser, string> ProcessValues = new()
    {
        { Browser.Chrome, "chrome" },
        { Browser.Edge, "msedge" },
        { Browser.Firefox, "firefox" },
    };

    private static readonly HashSet<string> ProcessNames =
        new(ProcessValues.Values.Select(v => $"{v}.exe"), StringComparer.OrdinalIgnoreCase);

    public static string ToProcessValue(this Browser browser) => ProcessValues[browser];

    public static bool HasProcess(string processName) =>
        ProcessNames.Contains(processName.ToLowerInvariant());
}

public enum Status
{
    Maximized,
    Minimized,
    Normal,
    Hidden,
}

public static class StatusExtensions
{
    public static string ToStringValue(this Status status) => status switch
    {
        Status.Maximized => "Maximized",
        Status.Minimized => "Minimized",
        Status.Normal => "Normal",
        Status.Hidden => "Hidden",
        _ => status.ToString(),
    };
}

public class Window
{
    public required string Name { get; init; }
    public required bool IsBrowser { get; init; }
    public required int Depth { get; init; }
    public required Status Status { get; init; }
    public required BoundingBox BoundingBox { get; init; }
    public required int Handle { get; init; }
    public required int ProcessId { get; init; }

    public object[] ToRow()
    {
        return [Name, Depth, Status.ToStringValue(), BoundingBox.Width, BoundingBox.Height, Handle];
    }
}

public class Size
{
    public required int Width { get; init; }
    public required int Height { get; init; }

    public override string ToString() => $"({Width},{Height})";
}

public class DesktopState
{
    public required Dictionary<string, object?> ActiveDesktop { get; init; }
    public required List<Dictionary<string, object?>> AllDesktops { get; init; }
    public Window? ActiveWindow { get; init; }
    public List<Window> Windows { get; init; } = [];
    public byte[]? Screenshot { get; set; }
    public TreeState? TreeState { get; set; }

    public string ActiveDesktopToString()
    {
        var desktopName = ActiveDesktop.TryGetValue("name", out var name) ? name?.ToString() : null;
        var sb = new StringBuilder();
        sb.AppendLine("Name");
        sb.AppendLine(new string('-', Math.Max(4, desktopName?.Length ?? 4)));
        sb.AppendLine(desktopName);
        return sb.ToString().TrimEnd();
    }

    public string DesktopsToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Name");
        var maxLen = AllDesktops
            .Select(d => d.TryGetValue("name", out var n) ? n?.ToString()?.Length ?? 0 : 0)
            .DefaultIfEmpty(4)
            .Max();
        sb.AppendLine(new string('-', Math.Max(4, maxLen)));
        foreach (var desktop in AllDesktops)
        {
            var name = desktop.TryGetValue("name", out var n) ? n?.ToString() : null;
            sb.AppendLine(name);
        }

        return sb.ToString().TrimEnd();
    }

    public string ActiveWindowToString()
    {
        if (ActiveWindow is null)
            return "No active window found";

        var sb = new StringBuilder();
        sb.AppendLine("Name  Depth  Status  Width  Height  Handle");
        sb.AppendLine("----  -----  ------  -----  ------  ------");
        var row = ActiveWindow.ToRow();
        sb.AppendLine(string.Join("  ", row));
        return sb.ToString().TrimEnd();
    }

    public string WindowsToString()
    {
        if (Windows.Count == 0)
            return "No windows found";

        var sb = new StringBuilder();
        sb.AppendLine("Name  Depth  Status  Width  Height  Handle");
        sb.AppendLine("----  -----  ------  -----  ------  ------");
        foreach (var window in Windows)
        {
            var row = window.ToRow();
            sb.AppendLine(string.Join("  ", row));
        }

        return sb.ToString().TrimEnd();
    }
}
