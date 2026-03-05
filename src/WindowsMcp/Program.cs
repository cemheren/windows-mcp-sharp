using System.CommandLine;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using WindowsMcp.Auth;
using WindowsMcp.Desktop;
using WindowsMcp.FileSystem;
using WindowsMcp.Watchdog;

namespace WindowsMcp;

#region Enums & Configuration

public enum TransportType
{
    Stdio,
    Sse,
    StreamableHttp
}

public enum ServerMode
{
    Local,
    Remote
}

public class ServerConfig
{
    public ServerMode Mode { get; set; } = ServerMode.Local;
    public string SandboxId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}

#endregion

#region Server State

/// <summary>
/// Holds global server state, mirroring Python module-level globals.
/// </summary>
public static class ServerState
{
    public static DesktopService? Desktop { get; set; }
    public static WatchDog? WatchDog { get; set; }
    public static Size? ScreenSize { get; set; }

    public const int MaxImageWidth = 1920;
    public const int MaxImageHeight = 1080;

    public const string Instructions =
        "Windows MCP server provides tools to interact directly with the Windows desktop, " +
        "thus enabling to operate the desktop on the user's behalf.";
}

#endregion

#region Lifespan Service

/// <summary>
/// Manages MCP server lifecycle: initialises Desktop and WatchDog on start and cleans up on stop.
/// </summary>
public sealed class McpLifetimeService : IHostedService
{
    private readonly ILogger<McpLifetimeService> _logger;
    private readonly IServiceProvider _services;

    public McpLifetimeService(IServiceProvider services, ILogger<McpLifetimeService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initialising Windows MCP server...");

        ServerState.Desktop = new DesktopService();
        ServerState.WatchDog = new WatchDog();
        ServerState.ScreenSize = ServerState.Desktop.GetScreenSize();

        // TODO: Wire up focus callback — requires exposing TreeService on DesktopService
        // ServerState.WatchDog.SetFocusCallback(desktop.Tree.OnFocusChange);

        ServerState.WatchDog.Start();
        await Task.Delay(1000, cancellationToken);

        _logger.LogInformation("Windows MCP server initialised (screen {Size})", ServerState.ScreenSize);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Shutting down Windows MCP server...");
        ServerState.WatchDog?.Stop();
        ServerState.WatchDog?.Dispose();
        await Task.CompletedTask;
    }
}

#endregion

#region MCP Tools — Desktop & Input

[McpServerToolType]
public static class DesktopTools
{
    [McpServerTool(Name = "App",
        Title = "App",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Manages Windows applications with three modes: 'launch' (opens the prescribed application), 'resize' (adjusts active window size/position), 'switch' (brings specific window into focus).")]
    public static string App(
        [Description("Operation mode: launch, resize, or switch")] string mode = "launch",
        [Description("Application or window name")] string? name = null,
        [Description("Window location [x, y]")] int[]? windowLoc = null,
        [Description("Window size [width, height]")] int[]? windowSize = null)
    {
        var desktop = ServerState.Desktop!;
        var loc = windowLoc is { Length: 2 } ? ((int, int)?)(windowLoc[0], windowLoc[1]) : null;
        var size = windowSize is { Length: 2 } ? ((int, int)?)(windowSize[0], windowSize[1]) : null;
        return desktop.App(mode, name, loc, size);
    }

    [McpServerTool(Name = "PowerShell",
        Title = "PowerShell",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = true)]
    [Description("A comprehensive system tool for executing any PowerShell commands. Use it to navigate the file system, manage files and processes, and execute system-level operations. Capable of accessing web content (e.g., via Invoke-WebRequest), interacting with network resources, and performing complex administrative tasks.")]
    public static string PowerShell(
        [Description("PowerShell command to execute")] string command,
        [Description("Timeout in seconds")] int timeout = 30)
    {
        try
        {
            var (output, exitCode) = ServerState.Desktop!.ExecuteCommand(command, timeout);
            return $"Response: {output}\nStatus Code: {exitCode}";
        }
        catch (Exception e)
        {
            return $"Error executing command: {e.Message}\nStatus Code: 1";
        }
    }

    [McpServerTool(Name = "Snapshot",
        Title = "Snapshot",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Captures complete desktop state including: system language, focused/opened windows, interactive elements (buttons, text fields, links, menus with coordinates), and scrollable areas. Set useVision=true to include screenshot. Set useDom=true for browser content. Always call this first to understand the current desktop state before taking actions.")]
    public static IList<ContentBlock> Snapshot(
        [Description("Include a screenshot of the desktop")] bool useVision = false,
        [Description("Use DOM content for browser windows")] bool useDom = false)
    {
        try
        {
            var desktop = ServerState.Desktop!;
            var screenSize = ServerState.ScreenSize!;

            var scaleWidth = screenSize.Width > ServerState.MaxImageWidth
                ? (float)ServerState.MaxImageWidth / screenSize.Width : 1.0f;
            var scaleHeight = screenSize.Height > ServerState.MaxImageHeight
                ? (float)ServerState.MaxImageHeight / screenSize.Height : 1.0f;
            var scale = Math.Min(scaleWidth, scaleHeight);

            var state = desktop.GetState(
                useAnnotation: useVision,
                useVision: useVision,
                useDom: useDom,
                asBytes: true,
                scale: scale);

            var interactiveElements = state.TreeState?.InteractiveElementsToString() ?? string.Empty;
            var scrollableElements = state.TreeState?.ScrollableElementsToString() ?? string.Empty;
            var windows = state.WindowsToString();
            var activeWindow = state.ActiveWindowToString();
            var activeDesktop = state.ActiveDesktopToString();
            var allDesktops = state.DesktopsToString();

            var textContent = $"""
                Active Desktop:
                {activeDesktop}

                All Desktops:
                {allDesktops}

                Focused Window:
                {activeWindow}

                Opened Windows:
                {windows}

                List of Interactive Elements:
                {(string.IsNullOrEmpty(interactiveElements) ? "No interactive elements found." : interactiveElements)}

                List of Scrollable Elements:
                {(string.IsNullOrEmpty(scrollableElements) ? "No scrollable elements found." : scrollableElements)}
                """;

            var result = new List<ContentBlock> { new TextContentBlock { Text = textContent } };

            if (useVision && state.Screenshot is { Length: > 0 })
            {
                result.Add(ImageContentBlock.FromBytes(state.Screenshot, "image/png"));
            }

            return result;
        }
        catch (Exception e)
        {
            return [new TextContentBlock { Text = $"Error capturing desktop state: {e.Message}. Please try again." }];
        }
    }

    [McpServerTool(Name = "Click",
        Title = "Click",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Performs mouse clicks at specified coordinates [x, y]. Supports button types: 'left' for selection/activation, 'right' for context menus, 'middle'. Supports clicks: 0=hover only, 1=single click, 2=double click.")]
    public static string Click(
        [Description("Coordinates [x, y]")] int[] loc,
        [Description("Mouse button: left, right, or middle")] string button = "left",
        [Description("Number of clicks (0=hover, 1=single, 2=double)")] int clicks = 1)
    {
        if (loc.Length != 2)
            throw new ArgumentException("Location must be a list of exactly 2 integers [x, y]");
        int x = loc[0], y = loc[1];
        ServerState.Desktop!.Click((x, y), button, clicks);
        var clickType = clicks switch { 0 => "Hover", 1 => "Single", 2 => "Double", _ => $"{clicks}x" };
        return $"{clickType} {button} clicked at ({x},{y}).";
    }

    [McpServerTool(Name = "Type",
        Title = "Type",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Types text at specified coordinates [x, y]. Set clear=true to clear existing text first. Set pressEnter=true to submit after typing. Set caretPosition to 'start', 'end', or 'idle'.")]
    public static string TypeText(
        [Description("Coordinates [x, y]")] int[] loc,
        [Description("Text to type")] string text,
        [Description("Clear existing text first")] bool clear = false,
        [Description("Caret position: start, idle, or end")] string caretPosition = "idle",
        [Description("Press Enter after typing")] bool pressEnter = false)
    {
        if (loc.Length != 2)
            throw new ArgumentException("Location must be a list of exactly 2 integers [x, y]");
        int x = loc[0], y = loc[1];
        ServerState.Desktop!.Type((x, y), text, caretPosition, clear, pressEnter);
        return $"Typed {text} at ({x},{y}).";
    }

    [McpServerTool(Name = "Scroll",
        Title = "Scroll",
        ReadOnly = false,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Scrolls at coordinates [x, y] or current mouse position if loc is null. Type: vertical (default) or horizontal. Direction: up/down for vertical, left/right for horizontal. wheelTimes controls amount.")]
    public static string Scroll(
        [Description("Coordinates [x, y] or null for current position")] int[]? loc = null,
        [Description("Scroll type: vertical or horizontal")] string type = "vertical",
        [Description("Scroll direction: up, down, left, or right")] string direction = "down",
        [Description("Number of wheel increments")] int wheelTimes = 1)
    {
        if (loc is not null && loc.Length != 2)
            throw new ArgumentException("Location must be a list of exactly 2 integers [x, y]");
        var tuple = loc is { Length: 2 } ? ((int, int)?)(loc[0], loc[1]) : null;
        var response = ServerState.Desktop!.Scroll(tuple, type, direction, wheelTimes);
        if (!string.IsNullOrEmpty(response))
            return response;
        var locStr = tuple.HasValue ? $" at ({tuple.Value.Item1},{tuple.Value.Item2})." : ".";
        return $"Scrolled {type} {direction} by {wheelTimes} wheel times{locStr}";
    }

    [McpServerTool(Name = "Move",
        Title = "Move",
        ReadOnly = false,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Moves mouse cursor to coordinates [x, y]. Set drag=true to perform a drag-and-drop operation from the current position to the target coordinates.")]
    public static string Move(
        [Description("Target coordinates [x, y]")] int[] loc,
        [Description("Drag from current position to target")] bool drag = false)
    {
        if (loc.Length != 2)
            throw new ArgumentException("loc must be a list of exactly 2 integers [x, y]");
        int x = loc[0], y = loc[1];
        if (drag)
        {
            ServerState.Desktop!.Drag((x, y));
            return $"Dragged to ({x},{y}).";
        }
        ServerState.Desktop!.Move((x, y));
        return $"Moved the mouse pointer to ({x},{y}).";
    }

    [McpServerTool(Name = "Shortcut",
        Title = "Shortcut",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Executes keyboard shortcuts using key combinations separated by +. Examples: \"ctrl+c\" (copy), \"ctrl+v\" (paste), \"alt+tab\" (switch apps), \"win+r\" (Run dialog).")]
    public static string Shortcut(
        [Description("Key combination (e.g. ctrl+c, alt+tab)")] string shortcut)
    {
        ServerState.Desktop!.Shortcut(shortcut);
        return $"Pressed {shortcut}.";
    }

    [McpServerTool(Name = "Wait",
        Title = "Wait",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Pauses execution for specified duration in seconds. Use when waiting for applications to launch/load, UI animations to complete, page content to render, or dialogs to appear.")]
    public static string Wait(
        [Description("Duration in seconds")] int duration)
    {
        Thread.Sleep(duration * 1000);
        return $"Waited for {duration} seconds.";
    }
}

#endregion

#region MCP Tools — File System

[McpServerToolType]
public static class FileSystemTools
{
    [McpServerTool(Name = "FileSystem",
        Title = "FileSystem",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Manages file system operations with eight modes: 'read' (read text file contents with optional line offset/limit), 'write' (create or overwrite, set append=true to append), 'copy' (copy file or directory), 'move' (move or rename), 'delete' (delete file or directory, set recursive=true for non-empty dirs), 'list' (list directory contents), 'search' (find files matching glob pattern), 'info' (get file/directory metadata). Relative paths are resolved from the user's Desktop folder.")]
    public static string FileSystem(
        [Description("Operation: read, write, copy, move, delete, list, search, info")] string mode,
        [Description("File or directory path")] string path,
        [Description("Destination path (for copy/move)")] string? destination = null,
        [Description("File content (for write)")] string? content = null,
        [Description("Glob pattern (for list/search)")] string? pattern = null,
        [Description("Process subdirectories recursively")] bool recursive = false,
        [Description("Append to file instead of overwriting")] bool append = false,
        [Description("Overwrite existing files")] bool overwrite = false,
        [Description("Line offset for read mode")] int? offset = null,
        [Description("Line limit for read mode")] int? limit = null,
        [Description("File encoding")] string encoding = "utf-8",
        [Description("Show hidden files")] bool showHidden = false)
    {
        try
        {
            var defaultDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (!Path.IsPathRooted(path))
                path = Path.Combine(defaultDir, path);
            if (destination is not null && !Path.IsPathRooted(destination))
                destination = Path.Combine(defaultDir, destination);

            return mode switch
            {
                "read" => FileSystemService.ReadFile(path, offset, limit, encoding),
                "write" when content is null => "Error: content parameter is required for write mode.",
                "write" => FileSystemService.WriteFile(path, content, append, encoding),
                "copy" when destination is null => "Error: destination parameter is required for copy mode.",
                "copy" => FileSystemService.CopyPath(path, destination, overwrite),
                "move" when destination is null => "Error: destination parameter is required for move mode.",
                "move" => FileSystemService.MovePath(path, destination, overwrite),
                "delete" => FileSystemService.DeletePath(path, recursive),
                "list" => FileSystemService.ListDirectory(path, pattern, recursive, showHidden),
                "search" when pattern is null => "Error: pattern parameter is required for search mode.",
                "search" => FileSystemService.SearchFiles(path, pattern, recursive),
                "info" => FileSystemService.GetFileInfo(path),
                _ => $"Error: Unknown mode \"{mode}\". Use: read, write, copy, move, delete, list, search, info."
            };
        }
        catch (Exception e)
        {
            return $"Error in File tool: {e.Message}";
        }
    }
}

#endregion

#region MCP Tools — Scraping & Multi-Action

[McpServerToolType]
public static class WebTools
{
    [McpServerTool(Name = "Scrape",
        Title = "Scrape",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = true)]
    [Description("Fetch content from a URL or the active browser tab. By default performs a lightweight HTTP request and returns markdown. Set useDom=true to extract visible text from the active tab's DOM using the accessibility tree.")]
    public static string Scrape(
        [Description("URL to scrape")] string url,
        [Description("Use DOM content from active browser tab")] bool useDom = false)
    {
        var desktop = ServerState.Desktop!;
        if (!useDom)
        {
            var htmlContent = desktop.Scrape(url);
            return $"URL:{url}\nContent:\n{htmlContent}";
        }

        var desktopState = desktop.GetState(useAnnotation: false, useVision: false, useDom: true, asBytes: false, scale: 1.0f);
        var treeState = desktopState.TreeState;
        if (treeState?.DomNode is null)
            return $"No DOM information found. Please open {url} in browser first.";

        var domNode = treeState.DomNode;
        var verticalScrollPercent = domNode.VerticalScrollPercent;
        var textContent = string.Join("\n", treeState.DomInformativeNodes.Select(n => n.Text));
        var headerStatus = verticalScrollPercent <= 0 ? "Reached top" : "Scroll up to see more";
        var footerStatus = verticalScrollPercent >= 100 ? "Reached bottom" : "Scroll down to see more";
        return $"URL:{url}\nContent:\n{headerStatus}\n{textContent}\n{footerStatus}";
    }
}

[McpServerToolType]
public static class MultiActionTools
{
    [McpServerTool(Name = "MultiSelect",
        Title = "MultiSelect",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Selects multiple items such as files, folders, or checkboxes if pressCtrl=true, or performs multiple clicks if false.")]
    public static string MultiSelect(
        [Description("List of coordinate pairs [[x,y], ...]")] int[][] locs,
        [Description("Hold Ctrl while clicking")] bool pressCtrl = true)
    {
        var tuples = locs.Select(l => (l[0], l[1])).ToList();
        ServerState.Desktop!.MultiSelect(pressCtrl, tuples);
        var elements = string.Join("\n", locs.Select(l => $"({l[0]},{l[1]})"));
        return $"Multi-selected elements at:\n{elements}";
    }

    [McpServerTool(Name = "MultiEdit",
        Title = "MultiEdit",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Enters text into multiple input fields at specified coordinates. Provide locs as [[x, y, \"text\"], ...].")]
    public static string MultiEdit(
        [Description("List of [x, y, text] entries as JSON")] JsonElement locs)
    {
        var entries = new List<(int X, int Y, string Text)>();
        foreach (var item in locs.EnumerateArray())
        {
            var arr = item.EnumerateArray().ToArray();
            var x = arr[0].GetInt32();
            var y = arr[1].GetInt32();
            var text = arr[2].GetString() ?? string.Empty;
            entries.Add((x, y, text));
        }
        ServerState.Desktop!.MultiEdit(entries);
        var elements = string.Join(", ", entries.Select(e => $"({e.X},{e.Y}) with text '{e.Text}'"));
        return $"Multi-edited elements at: {elements}";
    }
}

#endregion

#region MCP Tools — System

[McpServerToolType]
public static class SystemTools
{
    [McpServerTool(Name = "Clipboard",
        Title = "Clipboard",
        ReadOnly = false,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Manages Windows clipboard operations. Use mode=\"get\" to read current clipboard content, mode=\"set\" to set clipboard text.")]
    public static string Clipboard(
        [Description("Operation: get or set")] string mode,
        [Description("Text to set (required for set mode)")] string? text = null)
    {
        try
        {
            // Clipboard access on Windows uses PowerShell commands via DesktopService
            var desktop = ServerState.Desktop!;
            return mode switch
            {
                "get" => desktop.ExecuteCommand("Get-Clipboard", 5).Output is { } output
                    ? $"Clipboard content:\n{output}"
                    : "Clipboard is empty or contains non-text data.",
                "set" when text is null => "Error: text parameter required for set mode.",
                "set" => ExecuteClipboardSet(desktop, text!),
                _ => "Error: mode must be either \"get\" or \"set\"."
            };
        }
        catch (Exception e)
        {
            return $"Error managing clipboard: {e.Message}";
        }
    }

    private static string ExecuteClipboardSet(DesktopService desktop, string text)
    {
        var escaped = text.Replace("'", "''");
        desktop.ExecuteCommand($"Set-Clipboard -Value '{escaped}'", 5);
        var preview = text.Length > 100 ? text[..100] + "..." : text;
        return $"Clipboard set to: {preview}";
    }

    [McpServerTool(Name = "Process",
        Title = "Process",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Manages system processes. Use mode=\"list\" to list running processes with filtering and sorting. Use mode=\"kill\" to terminate processes by PID or name.")]
    public static string Process(
        [Description("Operation: list or kill")] string mode,
        [Description("Process name filter")] string? name = null,
        [Description("Process ID to kill")] int? pid = null,
        [Description("Sort by: memory, cpu, or name")] string sortBy = "memory",
        [Description("Maximum number of results")] int limit = 20,
        [Description("Force kill the process")] bool force = false)
    {
        try
        {
            var desktop = ServerState.Desktop!;
            return mode switch
            {
                "list" => desktop.ListProcesses(name, sortBy, limit),
                "kill" => desktop.KillProcess(name, pid, force),
                _ => "Error: mode must be either \"list\" or \"kill\"."
            };
        }
        catch (Exception e)
        {
            return $"Error managing processes: {e.Message}";
        }
    }

    [McpServerTool(Name = "SystemInfo",
        Title = "SystemInfo",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    [Description("Returns system information including CPU usage, memory usage, disk space, network stats, and uptime.")]
    public static string SystemInfo()
    {
        try
        {
            return ServerState.Desktop!.GetSystemInfo();
        }
        catch (Exception e)
        {
            return $"Error getting system info: {e.Message}";
        }
    }

    [McpServerTool(Name = "Notification",
        Title = "Notification",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Sends a Windows toast notification with a title and message.")]
    public static string Notification(
        [Description("Notification title")] string title,
        [Description("Notification message")] string message)
    {
        try
        {
            return ServerState.Desktop!.SendNotification(title, message);
        }
        catch (Exception e)
        {
            return $"Error sending notification: {e.Message}";
        }
    }

    [McpServerTool(Name = "LockScreen",
        Title = "LockScreen",
        ReadOnly = false,
        Destructive = false,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Locks the Windows workstation. Requires the user to enter their password to unlock.")]
    public static string LockScreen()
    {
        try
        {
            return ServerState.Desktop!.LockScreen();
        }
        catch (Exception e)
        {
            return $"Error locking screen: {e.Message}";
        }
    }

    [McpServerTool(Name = "Registry",
        Title = "Registry",
        ReadOnly = false,
        Destructive = true,
        Idempotent = false,
        OpenWorld = false)]
    [Description("Accesses the Windows Registry. Use mode=\"get\" to read a value, \"set\" to create/update, \"delete\" to remove, \"list\" to list values and sub-keys. Paths use PowerShell format (e.g. \"HKCU:\\\\Software\\\\MyApp\").")]
    public static string Registry(
        [Description("Operation: get, set, delete, or list")] string mode,
        [Description("Registry path")] string path,
        [Description("Value name")] string? name = null,
        [Description("Value to set")] string? value = null,
        [Description("Registry value type: String, DWord, QWord, Binary, MultiString, ExpandString")] string type = "String")
    {
        try
        {
            var desktop = ServerState.Desktop!;
            return mode switch
            {
                "get" when name is null => "Error: name parameter is required for get mode.",
                "get" => desktop.RegistryGet(path, name!),
                "set" when name is null => "Error: name parameter is required for set mode.",
                "set" when value is null => "Error: value parameter is required for set mode.",
                "set" => desktop.RegistrySet(path, name!, value!, type),
                "delete" => desktop.RegistryDelete(path, name),
                "list" => desktop.RegistryList(path),
                _ => "Error: mode must be \"get\", \"set\", \"delete\", or \"list\"."
            };
        }
        catch (Exception e)
        {
            return $"Error accessing registry: {e.Message}";
        }
    }
}

#endregion

#region Entry Point

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var transportOption = new Option<string>("--transport")
        {
            Description = "The transport layer used by the MCP server.",
            DefaultValueFactory = _ => "stdio"
        };

        var hostOption = new Option<string>("--host")
        {
            Description = "Host to bind the SSE/Streamable HTTP server.",
            DefaultValueFactory = _ => "localhost"
        };

        var portOption = new Option<int>("--port")
        {
            Description = "Port to bind the SSE/Streamable HTTP server.",
            DefaultValueFactory = _ => 8000
        };

        var rootCommand = new RootCommand("Windows MCP Server — lightweight MCP server for interacting with Windows OS");
        rootCommand.Add(transportOption);
        rootCommand.Add(hostOption);
        rootCommand.Add(portOption);

        rootCommand.SetAction(async (parseResult, cancellationToken) =>
        {
            var transport = parseResult.GetValue(transportOption) ?? "stdio";
            var host = parseResult.GetValue(hostOption) ?? "localhost";
            var port = parseResult.GetValue(portOption);

            var config = new ServerConfig
            {
                Mode = Enum.TryParse<ServerMode>(
                    Environment.GetEnvironmentVariable("MODE") ?? "Local", true, out var m)
                    ? m : ServerMode.Local,
                SandboxId = Environment.GetEnvironmentVariable("SANDBOX_ID") ?? string.Empty,
                ApiKey = Environment.GetEnvironmentVariable("API_KEY") ?? string.Empty
            };

            switch (config.Mode)
            {
                case ServerMode.Local:
                    await RunLocalServer(transport, host, port);
                    break;

                case ServerMode.Remote:
                    await RunRemoteServer(transport, host, port, config);
                    break;

                default:
                    throw new InvalidOperationException($"Invalid mode: {config.Mode}");
            }
        });

        var parseResult = rootCommand.Parse(args);
        return await parseResult.InvokeAsync();
    }

    private static async Task RunLocalServer(string transport, string host, int port)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddHostedService<McpLifetimeService>();

        builder.Services.AddMcpServer(options =>
        {
            options.ServerInfo = new() { Name = "windows-mcp", Version = "0.6.8" };
            options.ServerInstructions = ServerState.Instructions;
        })
        .WithStdioServerTransport()
        .WithToolsFromAssembly();

        switch (transport)
        {
            case "stdio":
                break;
            case "sse":
            case "streamable-http":
                // TODO: HTTP-based transports require ModelContextProtocol.AspNetCore package.
                // For now only stdio is supported. When the ASP.NET Core integration is added,
                // use .WithHttpTransport() and builder.WebHost.UseUrls($"http://{host}:{port}").
                throw new NotSupportedException(
                    $"Transport '{transport}' is not yet supported. Use --transport stdio.");
            default:
                throw new ArgumentException($"Invalid transport: {transport}");
        }

        var app = builder.Build();
        await app.RunAsync();
    }

    private static async Task RunRemoteServer(string transport, string host, int port, ServerConfig config)
    {
        if (string.IsNullOrEmpty(config.SandboxId))
            throw new InvalidOperationException("SANDBOX_ID is required for MODE: remote");
        if (string.IsNullOrEmpty(config.ApiKey))
            throw new InvalidOperationException("API_KEY is required for MODE: remote");

        using var httpClient = new HttpClient();
        using var loggerFactory = LoggerFactory.Create(b => b.AddConsole());
        var authLogger = loggerFactory.CreateLogger<AuthClient>();
        var client = new AuthClient(config.ApiKey, config.SandboxId, httpClient, authLogger);
        await client.AuthenticateAsync(CancellationToken.None);

        // In remote mode, act as a proxy forwarding to the authenticated backend.
        // TODO: Implement full proxy forwarding using client.ProxyUrl and client.ProxyHeaders.
        // The C# MCP SDK does not yet have a built-in ProxyClient equivalent to FastMCP.

        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddSingleton(httpClient);

        builder.Services.AddMcpServer(options =>
        {
            options.ServerInfo = new() { Name = "windows-mcp", Version = "0.6.8" };
            options.ServerInstructions = ServerState.Instructions;
        })
        .WithStdioServerTransport();

        var app = builder.Build();
        await app.RunAsync();
    }
}

#endregion
