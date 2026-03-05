#pragma warning disable CA1401
#pragma warning disable SYSLIB1054

using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using FuzzySharp;
using Microsoft.Extensions.Logging;
using ReverseMarkdown;
using SkiaSharp;
using WindowsMcp.Tree;
using WindowsMcp.Uia;
using WindowsMcp.Vdm;

namespace WindowsMcp.Desktop;

/// <summary>
/// Key-name aliases for shortcut keys that differ from UIA SpecialKeyNames.
/// </summary>
file static class KeyAliases
{
    public static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["backspace"] = "Back",
        ["capslock"] = "Capital",
        ["scrolllock"] = "Scroll",
        ["windows"] = "Win",
        ["command"] = "Win",
        ["option"] = "Alt",
    };
}

/// <summary>
/// Main service for desktop automation.
/// </summary>
public class DesktopService
{
    private static readonly ILogger<DesktopService> Logger =
        LoggerFactory.Create(b => b.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace)).CreateLogger<DesktopService>();

    private static readonly HttpClient SharedHttpClient = new() { Timeout = TimeSpan.FromSeconds(10) };
    private static readonly Random Rng = new();

    private readonly string _encoding;

    // Forward-reference: TreeService is expected in WindowsMcp.Tree namespace.
    private readonly TreeService _tree;

    public DesktopState? CurrentDesktopState { get; private set; }

    // ──────────────────────────────────────────────
    // P/Invoke helpers not exposed by UiaHelpers
    // ──────────────────────────────────────────────

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindowW(string? lpClassName, string? lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AllowSetForegroundWindow(int dwProcessId);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AttachThreadInput(int idAttach, int idAttachTo,
        [MarshalAs(UnmanagedType.Bool)] bool fAttach);

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool LockWorkStation();

    [DllImport("user32.dll")]
    private static extern int GetDpiForSystem();

    // ──────────────────────────────────────────────
    // Constructor
    // ──────────────────────────────────────────────

    public DesktopService()
    {
        _encoding = Encoding.Default.WebName;
        var (screenWidth, screenHeight) = UiaHelpers.GetVirtualScreenSize();
        _tree = new TreeService(this, screenWidth, screenHeight, IsWindowBrowser);
    }

#pragma warning disable CS8618 // Non-nullable field not initialized (test-only constructor)
    /// <summary>Test-only constructor that skips native interop initialization.</summary>
    protected DesktopService(string encoding)
    {
        _encoding = encoding;
    }
#pragma warning restore CS8618

    // ──────────────────────────────────────────────
    // SendKeys text escaping
    // ──────────────────────────────────────────────

    private static string EscapeTextForSendKeys(string text)
    {
        var sb = new StringBuilder(text.Length);
        foreach (var ch in text)
        {
            switch (ch)
            {
                case '{': sb.Append("{{}"); break;
                case '}': sb.Append("{}}"); break;
                case '\n': sb.Append("{Enter}"); break;
                case '\t': sb.Append("{Tab}"); break;
                case '\r': break; // skip
                default: sb.Append(ch); break;
            }
        }
        return sb.ToString();
    }

    // ──────────────────────────────────────────────
    // Desktop State
    // ──────────────────────────────────────────────

    public DesktopState GetState(
        bool useAnnotation = true,
        bool useVision = false,
        bool useDom = false,
        bool asBytes = false,
        float scale = 1.0f)
    {
        var startTime = Stopwatch.GetTimestamp();

        var controlsHandles = GetControlsHandles();
        var (windows, windowsHandles) = GetWindows(controlsHandles);
        var activeWindow = GetActiveWindow(windows);

        Dictionary<string, object?> activeDesktop;
        List<Dictionary<string, object?>> allDesktops;
        try
        {
            var currentInfo = VirtualDesktopManager.GetCurrentDesktopStatic();
            activeDesktop = new Dictionary<string, object?>
            {
                ["id"] = currentInfo.Id,
                ["name"] = currentInfo.Name,
            };
            allDesktops = VirtualDesktopManager.GetAllDesktopsStatic()
                .Select(d => new Dictionary<string, object?>
                {
                    ["id"] = d.Id,
                    ["name"] = d.Name,
                })
                .ToList();
        }
        catch (Exception)
        {
            activeDesktop = new Dictionary<string, object?>
            {
                ["id"] = "00000000-0000-0000-0000-000000000000",
                ["name"] = "Default Desktop",
            };
            allDesktops = [activeDesktop];
        }

        if (activeWindow is not null)
            windows.Remove(activeWindow);

        var activeWindowHandleInt = activeWindow is not null ? (int?)activeWindow.Handle : null;

        Logger.LogDebug("Active window: {ActiveWindow}", activeWindow?.Name ?? "No Active Window Found");
        Logger.LogDebug("Windows: {Count}", windows.Count);

        // Prepare handles for Tree
        var otherWindowsHandles = controlsHandles.Except(windowsHandles)
            .Select(h => h.ToInt32()).ToList();

        var treeState = _tree.GetState(activeWindowHandleInt, otherWindowsHandles, useDom);

        byte[]? screenshot = null;
        if (useVision)
        {
            SKBitmap bmp;
            if (useAnnotation)
            {
                var nodes = treeState.InteractiveNodes;
                bmp = GetAnnotatedScreenshot(nodes);
            }
            else
            {
                bmp = GetScreenshot();
            }

            if (Math.Abs(scale - 1.0f) > 0.001f)
            {
                var newWidth = (int)(bmp.Width * scale);
                var newHeight = (int)(bmp.Height * scale);
                var resized = bmp.Resize(new SKImageInfo(newWidth, newHeight), new SKSamplingOptions(SKFilterMode.Linear));
                bmp.Dispose();
                bmp = resized;
            }

            if (asBytes)
            {
                using var image = SKImage.FromBitmap(bmp);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                screenshot = data.ToArray();
            }

            bmp.Dispose();
        }

        CurrentDesktopState = new DesktopState
        {
            ActiveWindow = activeWindow,
            Windows = windows,
            ActiveDesktop = activeDesktop,
            AllDesktops = allDesktops,
            Screenshot = screenshot,
            TreeState = treeState,
        };

        var elapsed = Stopwatch.GetElapsedTime(startTime);
        Logger.LogInformation("Desktop State capture took {Elapsed:F2} seconds", elapsed.TotalSeconds);
        return CurrentDesktopState;
    }

    // ──────────────────────────────────────────────
    // Window Status
    // ──────────────────────────────────────────────

    public Status GetWindowStatus(Control control)
    {
        var handle = control.NativeWindowHandle;
        if (UiaHelpers.IsIconic(handle))
            return Status.Minimized;
        if (UiaHelpers.IsZoomed(handle))
            return Status.Maximized;
        if (UiaHelpers.IsWindowVisible(handle))
            return Status.Normal;
        return Status.Hidden;
    }

    // ──────────────────────────────────────────────
    // Cursor / Element helpers
    // ──────────────────────────────────────────────

    public (int X, int Y) GetCursorLocation() => UiaHelpers.GetCursorPos();

    public Control? GetElementUnderCursor() => UiaHelpers.ControlFromCursor();

    // ──────────────────────────────────────────────
    // Start Menu Apps
    // ──────────────────────────────────────────────

    public Dictionary<string, string> GetAppsFromStartMenu()
    {
        const string command = "Get-StartApps | ConvertTo-Csv -NoTypeInformation";
        var (output, status) = ExecuteCommand(command);

        if (status == 0 && !string.IsNullOrWhiteSpace(output))
        {
            try
            {
                var apps = ParseCsvToDictionary(output.Trim(), "Name", "AppID");
                if (apps.Count > 0)
                    return apps;
            }
            catch (Exception e)
            {
                Logger.LogWarning("Error parsing Get-StartApps output: {Error}", e.Message);
            }
        }

        Logger.LogInformation("Get-StartApps unavailable, falling back to Start Menu folder scan");
        return GetAppsFromShortcuts();
    }

    private static Dictionary<string, string> ParseCsvToDictionary(
        string csv, string keyColumn, string valueColumn)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        using var reader = new StringReader(csv);
        var headerLine = reader.ReadLine();
        if (headerLine is null) return result;

        var headers = ParseCsvLine(headerLine);
        var keyIdx = Array.IndexOf(headers, keyColumn);
        var valIdx = Array.IndexOf(headers, valueColumn);
        if (keyIdx < 0 || valIdx < 0) return result;

        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            var fields = ParseCsvLine(line);
            if (fields.Length <= Math.Max(keyIdx, valIdx)) continue;
            var key = fields[keyIdx].Trim();
            var val = fields[valIdx].Trim();
            if (key.Length > 0 && val.Length > 0)
                result.TryAdd(key.ToLowerInvariant(), val);
        }
        return result;
    }

    private static string[] ParseCsvLine(string line)
    {
        var fields = new List<string>();
        var sb = new StringBuilder();
        bool inQuotes = false;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    sb.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                fields.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }
        fields.Add(sb.ToString());
        return fields.ToArray();
    }

    private static Dictionary<string, string> GetAppsFromShortcuts()
    {
        var apps = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var startMenuPaths = new[]
        {
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu),
                "Programs"),
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                "Programs"),
        };

        foreach (var basePath in startMenuPaths)
        {
            if (!Directory.Exists(basePath)) continue;
            foreach (var lnkPath in Directory.EnumerateFiles(basePath, "*.lnk", SearchOption.AllDirectories))
            {
                var name = Path.GetFileNameWithoutExtension(lnkPath).ToLowerInvariant();
                if (name.Length > 0)
                    apps.TryAdd(name, lnkPath);
            }
        }
        return apps;
    }

    // ──────────────────────────────────────────────
    // PowerShell Execution
    // ──────────────────────────────────────────────

    public virtual (string Output, int ExitCode) ExecuteCommand(string command, int timeout = 10)
    {
        try
        {
            var encoded = Convert.ToBase64String(Encoding.Unicode.GetBytes(command));

            var psi = new ProcessStartInfo
            {
                FileName = "powershell",
                ArgumentList =
                {
                    "-NoProfile",
                    "-OutputFormat", "Text",
                    "-EncodedCommand", encoded,
                },
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            };

            // Fix PATHEXT if clobbered by venv activation
            var pathExt = Environment.GetEnvironmentVariable("PATHEXT") ?? "";
            if (!pathExt.Contains(".EXE", StringComparison.OrdinalIgnoreCase))
            {
                psi.Environment["PATHEXT"] =
                    ".COM;.EXE;.BAT;.CMD;.VBS;.VBE;.JS;.JSE;.WSF;.WSH;.MSC;.CPL;.PY;.PYW";
            }

            using var process = System.Diagnostics.Process.Start(psi);
            if (process is null)
                return ("Failed to start PowerShell process", 1);

            var stdoutTask = process.StandardOutput.ReadToEndAsync();
            var stderrTask = process.StandardError.ReadToEndAsync();

            if (!process.WaitForExit(timeout * 1000))
            {
                try { process.Kill(entireProcessTree: true); } catch { /* ignore */ }
                return ("Command execution timed out", 1);
            }

            var stdout = stdoutTask.GetAwaiter().GetResult();
            var stderr = stderrTask.GetAwaiter().GetResult();
            var output = !string.IsNullOrEmpty(stdout) ? stdout : stderr;
            return (output, process.ExitCode);
        }
        catch (Exception e) when (e is TimeoutException)
        {
            return ("Command execution timed out", 1);
        }
        catch (Exception e)
        {
            return ($"Command execution failed: {e.GetType().Name}: {e.Message}", 1);
        }
    }

    // ──────────────────────────────────────────────
    // Browser Detection
    // ──────────────────────────────────────────────

    public bool IsWindowBrowser(Control node)
    {
        try
        {
            var pid = node.ProcessId;
            if (pid is null or 0) return false;
            using var proc = System.Diagnostics.Process.GetProcessById(pid.Value);
            var processName = proc.ProcessName + ".exe";
            return BrowserExtensions.HasProcess(processName);
        }
        catch
        {
            return false;
        }
    }

    // ──────────────────────────────────────────────
    // Language
    // ──────────────────────────────────────────────

    public string GetDefaultLanguage()
    {
        const string command =
            "Get-Culture | Select-Object Name,DisplayName | ConvertTo-Csv -NoTypeInformation";
        var (response, _) = ExecuteCommand(command);
        var dict = ParseCsvToDictionary(response.Trim(), "DisplayName", "DisplayName");
        return string.Join("", dict.Keys);
    }

    // ──────────────────────────────────────────────
    // Resize App
    // ──────────────────────────────────────────────

    public (string Message, int Status) ResizeApp(
        (int Width, int Height)? size = null,
        (int X, int Y)? loc = null)
    {
        var activeWindow = CurrentDesktopState?.ActiveWindow;
        if (activeWindow is null)
            return ("No active window found", 1);
        if (activeWindow.Status == Status.Minimized)
            return ($"{activeWindow.Name} is minimized", 1);
        if (activeWindow.Status == Status.Maximized)
            return ($"{activeWindow.Name} is maximized", 1);

        var windowControl = UiaHelpers.ControlFromHandle(new IntPtr(activeWindow.Handle));
        if (windowControl is null)
            return ("Could not get window control", 1);

        var br = windowControl.BoundingRectangle;
        int x = loc?.X ?? br.Left;
        int y = loc?.Y ?? br.Top;
        int width = size?.Width ?? br.Width();
        int height = size?.Height ?? br.Height();

        UiaHelpers.MoveWindow(new IntPtr(activeWindow.Handle), x, y, width, height);
        return ($"{activeWindow.Name} resized to {width}x{height} at {x},{y}.", 0);
    }

    // ──────────────────────────────────────────────
    // Is App Running
    // ──────────────────────────────────────────────

    public bool IsAppRunning(string name)
    {
        var (windows, _) = GetWindows();
        var windowNames = windows.Select(w => w.Name).ToArray();
        var match = FuzzySharp.Process.ExtractOne(name, windowNames, cutoff: 60);
        return match is not null;
    }

    // ──────────────────────────────────────────────
    // App (launch / switch / resize)
    // ──────────────────────────────────────────────

    public string App(string mode, string? name = null,
        (int X, int Y)? loc = null, (int Width, int Height)? size = null)
    {
        switch (mode)
        {
            case "launch":
            {
                if (name is null) return "App name is required for launch.";
                var (response, status, pid) = LaunchApp(name);
                if (status != 0)
                    return response;

                bool launched = false;
                if (pid > 0)
                {
                    var ctrl = new WindowControl(processId: pid);
                    if (ctrl.Exists(maxSearchSeconds: 10))
                        launched = true;
                }

                if (!launched)
                {
                    var safeName = Regex.Escape(name);
                    var ctrl = new WindowControl(regexName: $"(?i).*{safeName}.*");
                    if (ctrl.Exists(maxSearchSeconds: 10))
                        launched = true;
                }

                return launched
                    ? $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)} launched."
                    : $"Launching {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)} sent, but window not detected yet.";
            }
            case "resize":
            {
                var (msg, _) = ResizeApp(size: size, loc: loc);
                return msg;
            }
            case "switch":
            {
                if (name is null) return "App name is required for switch.";
                var (msg, _) = SwitchApp(name);
                return msg;
            }
            default:
                return $"Unknown mode: {mode}";
        }
    }

    // ──────────────────────────────────────────────
    // Launch App
    // ──────────────────────────────────────────────

    public (string Response, int Status, int Pid) LaunchApp(string name)
    {
        var appsMap = GetAppsFromStartMenu();
        var match = FuzzySharp.Process.ExtractOne(name, appsMap.Keys, cutoff: 70);
        if (match is null)
            return ($"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)} not found in start menu.", 1, 0);

        var appName = match.Value;
        if (!appsMap.TryGetValue(appName, out var appId))
            return ($"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)} not found in start menu.", 1, 0);

        int pid = 0;
        string response;
        int status;

        if (File.Exists(appId) || appId.Contains('\\'))
        {
            var safe = DesktopUtils.PsQuote(appId);
            var command = $"Start-Process {safe} -PassThru | Select-Object -ExpandProperty Id";
            (response, status) = ExecuteCommand(command);
            if (status == 0 && int.TryParse(response.Trim(), out var parsedPid))
                pid = parsedPid;
        }
        else
        {
            // Validate app identifier
            var cleaned = appId.Replace("\\", "").Replace("_", "").Replace(".", "").Replace("-", "");
            if (!cleaned.All(char.IsLetterOrDigit))
                return ($"Invalid app identifier: {appId}", 1, 0);

            var safe = DesktopUtils.PsQuote($"shell:AppsFolder\\{appId}");
            var command = $"Start-Process {safe}";
            (response, status) = ExecuteCommand(command);
        }

        return (response, status, pid);
    }

    // ──────────────────────────────────────────────
    // Switch App
    // ──────────────────────────────────────────────

    public (string Message, int Status) SwitchApp(string name)
    {
        try
        {
            if (CurrentDesktopState is null || CurrentDesktopState.Windows.Count == 0)
                GetState();
            if (CurrentDesktopState is null)
                return ("Failed to get desktop state. Please try again.", 1);

            var windowList = new List<Window>();
            if (CurrentDesktopState.ActiveWindow is not null)
                windowList.Add(CurrentDesktopState.ActiveWindow);
            windowList.AddRange(CurrentDesktopState.Windows);

            if (windowList.Count == 0)
                return ("No windows found on the desktop.", 1);

            var windowsDict = windowList.ToDictionary(w => w.Name, w => w);
            var match = FuzzySharp.Process.ExtractOne(name, windowsDict.Keys, cutoff: 70);
            if (match is null)
                return ($"Application {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)} not found.", 1);

            var windowName = match.Value;
            var window = windowsDict[windowName];
            var targetHandle = new IntPtr(window.Handle);

            if (UiaHelpers.IsIconic(targetHandle))
            {
                UiaHelpers.ShowWindow(targetHandle, SW.Restore);
                return ($"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(windowName)} restored from Minimized state.", 0);
            }

            BringWindowToTop(targetHandle);
            return ($"Switched to {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(windowName)} window.", 0);
        }
        catch (Exception e)
        {
            return ($"Error switching app: {e.Message}", 1);
        }
    }

    // ──────────────────────────────────────────────
    // Bring Window to Top
    // ──────────────────────────────────────────────

    public void BringWindowToTop(IntPtr targetHandle)
    {
        if (!IsWindow(targetHandle))
            throw new ArgumentException("Invalid window handle");

        try
        {
            if (UiaHelpers.IsIconic(targetHandle))
                UiaHelpers.ShowWindow(targetHandle, SW.Restore);

            var foregroundHandle = UiaHelpers.GetForegroundWindow();

            if (!IsWindow(foregroundHandle))
            {
                UiaHelpers.SetForegroundWindow(targetHandle);
                UiaHelpers.BringWindowToTop(targetHandle);
                return;
            }

            var foregroundThread = GetWindowThreadProcessId(foregroundHandle, out _);
            var targetThread = GetWindowThreadProcessId(targetHandle, out _);

            if (foregroundThread == 0 || targetThread == 0 || foregroundThread == targetThread)
            {
                UiaHelpers.SetForegroundWindow(targetHandle);
                UiaHelpers.BringWindowToTop(targetHandle);
                return;
            }

            AllowSetForegroundWindow(-1);

            bool attached = false;
            try
            {
                AttachThreadInput(foregroundThread, targetThread, true);
                attached = true;

                UiaHelpers.SetForegroundWindow(targetHandle);
                UiaHelpers.BringWindowToTop(targetHandle);
                UiaHelpers.SetWindowPos(
                    targetHandle,
                    new IntPtr(SWP.HWND_Top),
                    0, 0, 0, 0,
                    (uint)(SWP.SWP_NoMove | SWP.SWP_NoSize | SWP.SWP_ShowWindow));
            }
            finally
            {
                if (attached)
                    AttachThreadInput(foregroundThread, targetThread, false);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to bring window to top");
        }
    }

    // ──────────────────────────────────────────────
    // Element / Label helpers
    // ──────────────────────────────────────────────

    public Control? GetElementHandleFromLabel(int label)
    {
        var treeState = CurrentDesktopState?.TreeState;
        if (treeState is null || label < 0 || label >= treeState.InteractiveNodes.Count)
            return null;
        var elementNode = treeState.InteractiveNodes[label];
        var xpath = elementNode.Xpath;
        return GetElementFromXpath(xpath);
    }

    public (int X, int Y) GetCoordinatesFromLabel(int label)
    {
        var element = GetElementHandleFromLabel(label);
        if (element is null)
            return (0, 0);
        var br = element.BoundingRectangle;
        return (br.XCenter(), br.YCenter());
    }

    // ──────────────────────────────────────────────
    // Input: Click
    // ──────────────────────────────────────────────

    public void Click((int X, int Y) loc, string button = "left", int clicks = 2)
    {
        int x = loc.X, y = loc.Y;
        if (clicks == 0)
        {
            UiaHelpers.SetCursorPos(x, y);
            return;
        }

        switch (button)
        {
            case "left":
                if (clicks >= 2)
                {
                    double dblWait = Win32.GetDoubleClickTime() / 2000.0;
                    for (int i = 0; i < clicks; i++)
                        UiaHelpers.Click(x, y, waitTime: i < clicks - 1 ? dblWait : 0.5);
                }
                else
                {
                    UiaHelpers.Click(x, y);
                }
                break;
            case "right":
                for (int i = 0; i < clicks; i++)
                    UiaHelpers.RightClick(x, y);
                break;
            case "middle":
                for (int i = 0; i < clicks; i++)
                    UiaHelpers.MiddleClick(x, y);
                break;
        }
    }

    // ──────────────────────────────────────────────
    // Input: Type
    // ──────────────────────────────────────────────

    public void Type(
        (int X, int Y) loc,
        string text,
        string caretPosition = "idle",
        bool clear = false,
        bool pressEnter = false)
    {
        UiaHelpers.Click(loc.X, loc.Y);

        if (caretPosition == "start")
            UiaHelpers.SendKeys("{Home}", waitTime: 0.05);
        else if (caretPosition == "end")
            UiaHelpers.SendKeys("{End}", waitTime: 0.05);

        if (clear)
        {
            Thread.Sleep(500);
            UiaHelpers.SendKeys("{Ctrl}a", waitTime: 0.05);
            UiaHelpers.SendKeys("{Back}", waitTime: 0.05);
        }

        var escaped = EscapeTextForSendKeys(text);
        UiaHelpers.SendKeys(escaped, interval: 0.02, waitTime: 0.05);

        if (pressEnter)
            UiaHelpers.SendKeys("{Enter}", waitTime: 0.05);
    }

    // ──────────────────────────────────────────────
    // Input: Scroll
    // ──────────────────────────────────────────────

    public string? Scroll(
        (int X, int Y)? loc = null,
        string type = "vertical",
        string direction = "down",
        int wheelTimes = 1)
    {
        if (loc.HasValue)
            Move(loc.Value);

        switch (type)
        {
            case "vertical":
                switch (direction)
                {
                    case "up": UiaHelpers.WheelUp(wheelTimes); break;
                    case "down": UiaHelpers.WheelDown(wheelTimes); break;
                    default: return "Invalid direction. Use \"up\" or \"down\".";
                }
                break;
            case "horizontal":
                switch (direction)
                {
                    case "left":
                        UiaHelpers.PressKey(Keys.VK_SHIFT, waitTime: 0.05);
                        UiaHelpers.WheelUp(wheelTimes);
                        Thread.Sleep(50);
                        UiaHelpers.ReleaseKey(Keys.VK_SHIFT, waitTime: 0.05);
                        break;
                    case "right":
                        UiaHelpers.PressKey(Keys.VK_SHIFT, waitTime: 0.05);
                        UiaHelpers.WheelDown(wheelTimes);
                        Thread.Sleep(50);
                        UiaHelpers.ReleaseKey(Keys.VK_SHIFT, waitTime: 0.05);
                        break;
                    default: return "Invalid direction. Use \"left\" or \"right\".";
                }
                break;
            default:
                return "Invalid type. Use \"horizontal\" or \"vertical\".";
        }
        return null;
    }

    // ──────────────────────────────────────────────
    // Input: Drag, Move, Shortcut
    // ──────────────────────────────────────────────

    public void Drag((int X, int Y) loc)
    {
        Thread.Sleep(500);
        var (cx, cy) = UiaHelpers.GetCursorPos();
        UiaHelpers.DragDrop(cx, cy, loc.X, loc.Y, moveSpeed: 1);
    }

    public void Move((int X, int Y) loc)
    {
        UiaHelpers.MoveTo(loc.X, loc.Y, moveSpeed: 10);
    }

    public void Shortcut(string shortcut)
    {
        var keys = shortcut.Split('+');
        var sb = new StringBuilder();
        foreach (var key in keys)
        {
            var k = key.Trim();
            if (k.Length == 1)
            {
                sb.Append(k);
            }
            else
            {
                var mapped = KeyAliases.Map.TryGetValue(k, out var alias) ? alias : k;
                sb.Append('{').Append(mapped).Append('}');
            }
        }
        UiaHelpers.SendKeys(sb.ToString(), interval: 0.01);
    }

    // ──────────────────────────────────────────────
    // Input: Multi-select, Multi-edit
    // ──────────────────────────────────────────────

    public void MultiSelect(bool pressCtrl, List<(int X, int Y)> locs)
    {
        if (pressCtrl)
            UiaHelpers.PressKey(Keys.VK_CONTROL, waitTime: 0.05);

        foreach (var (x, y) in locs)
        {
            UiaHelpers.Click(x, y, waitTime: 0.2);
            Thread.Sleep(500);
        }

        UiaHelpers.ReleaseKey(Keys.VK_CONTROL, waitTime: 0.05);
    }

    public void MultiEdit(List<(int X, int Y, string Text)> locs)
    {
        foreach (var (x, y, text) in locs)
            Type((x, y), text: text, clear: true);
    }

    // ──────────────────────────────────────────────
    // Scrape
    // ──────────────────────────────────────────────

    public string Scrape(string url)
    {
        try
        {
            var response = SharedHttpClient.GetAsync(url).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            var html = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var converter = new Converter();
            return converter.Convert(html);
        }
        catch (HttpRequestException e)
        {
            throw new InvalidOperationException($"HTTP error for {url}: {e.Message}", e);
        }
        catch (TaskCanceledException e)
        {
            throw new TimeoutException($"Request timed out for {url}: {e.Message}", e);
        }
    }

    // ──────────────────────────────────────────────
    // Window helpers
    // ──────────────────────────────────────────────

    public Window? GetWindowFromElement(Control? element)
    {
        if (element is null) return null;
        var topWindow = GetWindowFromElementHandle(element.NativeWindowHandle);
        if (topWindow is null) return null;
        var handle = topWindow.NativeWindowHandle;
        var (windows, _) = GetWindows();
        var handleInt = handle.ToInt32();
        return windows.FirstOrDefault(w => w.Handle == handleInt);
    }

    public bool IsWindowVisible(Control window)
    {
        var isNotMinimized = GetWindowStatus(window) != Status.Minimized;
        var br = window.BoundingRectangle;
        var area = br.Width() * br.Height();
        var isOverlay = IsOverlayWindow(window);
        return !isOverlay && isNotMinimized && area > 10;
    }

    public bool IsOverlayWindow(Control element)
    {
        var noChildren = element.GetChildren().Count == 0;
        var isNameOverlay = (element.Name ?? "").Contains("Overlay");
        return noChildren || isNameOverlay;
    }

    // ──────────────────────────────────────────────
    // Controls Handles (EnumWindows)
    // ──────────────────────────────────────────────

    public HashSet<IntPtr> GetControlsHandles()
    {
        var handles = new HashSet<IntPtr>();

        Win32.EnumWindows((hwnd, _) =>
        {
            try
            {
                if (IsWindow(hwnd)
                    && UiaHelpers.IsWindowVisible(hwnd)
                    && VirtualDesktopManager.IsWindowOnCurrentDesktopStatic(hwnd))
                {
                    handles.Add(hwnd);
                }
            }
            catch
            {
                // Skip invalid handles
            }
            return true;
        }, IntPtr.Zero);

        var desktopHwnd = FindWindowW("Progman", null);
        if (desktopHwnd != IntPtr.Zero) handles.Add(desktopHwnd);

        var taskbarHwnd = FindWindowW("Shell_TrayWnd", null);
        if (taskbarHwnd != IntPtr.Zero) handles.Add(taskbarHwnd);

        var secondaryTaskbarHwnd = FindWindowW("Shell_SecondaryTrayWnd", null);
        if (secondaryTaskbarHwnd != IntPtr.Zero) handles.Add(secondaryTaskbarHwnd);

        return handles;
    }

    // ──────────────────────────────────────────────
    // Get Active Window
    // ──────────────────────────────────────────────

    public Window? GetActiveWindow(List<Window>? windows = null)
    {
        try
        {
            if (windows is null)
                (windows, _) = GetWindows();

            var activeWindow = GetForegroundWindow();
            if (activeWindow is null || activeWindow.ClassName == "Progman")
                return null;

            var activeWindowHandle = activeWindow.NativeWindowHandle;
            var activeWindowHandleInt = activeWindowHandle.ToInt32();
            foreach (var window in windows)
            {
                if (window.Handle == activeWindowHandleInt)
                    return window;
            }

            // Active window not present in the windows list
            var br = activeWindow.BoundingRectangle;
            return new Window
            {
                Name = activeWindow.Name ?? "",
                IsBrowser = IsWindowBrowser(activeWindow),
                Depth = 0,
                BoundingBox = new BoundingBox(
                    Left: br.Left, Top: br.Top, Right: br.Right, Bottom: br.Bottom,
                    Width: br.Width(), Height: br.Height()),
                Status = GetWindowStatus(activeWindow),
                Handle = activeWindowHandleInt,
                ProcessId = activeWindow.ProcessId ?? 0,
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in GetActiveWindow");
        }
        return null;
    }

    public Control? GetForegroundWindow()
    {
        var handle = UiaHelpers.GetForegroundWindow();
        return GetWindowFromElementHandle(handle);
    }

    public Control? GetWindowFromElementHandle(IntPtr elementHandle)
    {
        var current = UiaHelpers.ControlFromHandle(elementHandle);
        if (current is null) return null;
        var rootHandle = UiaHelpers.GetRootControl().NativeWindowHandle;

        while (true)
        {
            var parent = current.GetParentControl();
            if (parent is null || parent.NativeWindowHandle == rootHandle)
                return current;
            current = parent;
        }
    }

    // ──────────────────────────────────────────────
    // Get Windows
    // ──────────────────────────────────────────────

    public (List<Window> Windows, HashSet<IntPtr> Handles) GetWindows(
        HashSet<IntPtr>? controlsHandles = null)
    {
        var windows = new List<Window>();
        var windowHandles = new HashSet<IntPtr>();

        try
        {
            controlsHandles ??= GetControlsHandles();
            int depth = 0;
            foreach (var hwnd in controlsHandles)
            {
                Control? child;
                try
                {
                    child = UiaHelpers.ControlFromHandle(hwnd);
                }
                catch
                {
                    continue;
                }

                if (child is null) continue;

                // Filter out overlays
                if (IsOverlayWindow(child))
                    continue;

                if (child is WindowControl or PaneControl)
                {
                    var windowPattern = child.GetPatternObject(PatternId.WindowPattern);
                    if (windowPattern is null)
                        continue;

                    // Check if window supports min/max via the pattern
                    // For a forward-compatible approach, check via the control properties
                    var status = GetWindowStatus(child);
                    var br = child.BoundingRectangle;
                    if (br.IsEmpty() && status != Status.Minimized)
                        continue;

                    var childHandle = child.NativeWindowHandle.ToInt32();
                    windows.Add(new Window
                    {
                        Name = child.Name ?? "",
                        Depth = depth,
                        Status = status,
                        BoundingBox = new BoundingBox(
                            Left: br.Left, Top: br.Top, Right: br.Right, Bottom: br.Bottom,
                            Width: br.Width(), Height: br.Height()),
                        Handle = childHandle,
                        ProcessId = child.ProcessId ?? 0,
                        IsBrowser = IsWindowBrowser(child),
                    });
                    windowHandles.Add(new IntPtr(childHandle));
                }
                depth++;
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in GetWindows");
            windows = [];
        }

        return (windows, windowHandles);
    }

    // ──────────────────────────────────────────────
    // XPath navigation
    // ──────────────────────────────────────────────

    public string GetXpathFromElement(Control? element)
    {
        var current = element;
        if (current is null) return "";

        var pathParts = new List<string>();
        while (current is not null)
        {
            var parent = current.GetParentControl();
            if (parent is null)
            {
                pathParts.Add(current.ControlTypeName);
                break;
            }

            var children = parent.GetChildren();
            var sameTypeIds = children
                .Where(c => c.ControlTypeId == current.ControlTypeId)
                .Select(c => string.Join("-", c.GetRuntimeId().Select(x => x.ToString())))
                .ToList();

            var currentId = string.Join("-", current.GetRuntimeId().Select(x => x.ToString()));
            var index = sameTypeIds.IndexOf(currentId);

            pathParts.Add(sameTypeIds.Count > 0
                ? $"{current.ControlTypeName}[{index + 1}]"
                : current.ControlTypeName);

            current = parent;
        }

        pathParts.Reverse();
        return string.Join("/", pathParts);
    }

    public Control GetElementFromXpath(string xpath)
    {
        var pattern = new Regex(@"(\w+)(?:\[(\d+)\])?");
        var parts = xpath.Split('/');
        var element = UiaHelpers.GetRootControl();

        foreach (var part in parts.Skip(1))
        {
            var match = pattern.Match(part);
            if (!match.Success) continue;

            var controlType = match.Groups[1].Value;
            var indexStr = match.Groups[2].Value;
            int? index = string.IsNullOrEmpty(indexStr) ? null : int.Parse(indexStr);

            var children = element.GetChildren();
            var sameTypeChildren = children.Where(c => c.ControlTypeName == controlType).ToList();

            element = index.HasValue
                ? sameTypeChildren[index.Value - 1]
                : sameTypeChildren[0];
        }

        return element;
    }

    // ──────────────────────────────────────────────
    // System Info helpers
    // ──────────────────────────────────────────────

    public string GetWindowsVersion()
    {
        var (response, status) = ExecuteCommand("(Get-CimInstance Win32_OperatingSystem).Caption");
        return status == 0 ? response.Trim() : "Windows";
    }

    public string GetUserAccountType()
    {
        var (response, status) = ExecuteCommand(
            "(Get-LocalUser -Name $env:USERNAME).PrincipalSource");
        if (status != 0) return "Local Account";
        return response.Trim() == "Local" ? "Local Account" : "Microsoft Account";
    }

    public double GetDpiScaling()
    {
        try
        {
            int dpi = GetDpiForSystem();
            return dpi > 0 ? dpi / 96.0 : 1.0;
        }
        catch
        {
            return 1.0;
        }
    }

    // ──────────────────────────────────────────────
    // Screen
    // ──────────────────────────────────────────────

    public Size GetScreenSize()
    {
        var (width, height) = UiaHelpers.GetVirtualScreenSize();
        return new Size { Width = width, Height = height };
    }

    // ──────────────────────────────────────────────
    // Screenshot (SkiaSharp-based)
    // ──────────────────────────────────────────────

    public SKBitmap GetScreenshot()
    {
        var (left, top, width, height) = UiaHelpers.GetVirtualScreenRect();
        if (width <= 0) width = 1920;
        if (height <= 0) height = 1080;

        IntPtr screenDc = Win32.GetDC(IntPtr.Zero);
        IntPtr memDc = Win32.CreateCompatibleDC(screenDc);
        IntPtr hBitmap = Win32.CreateCompatibleBitmap(screenDc, width, height);
        IntPtr oldBitmap = Win32.SelectObject(memDc, hBitmap);

        Win32.BitBlt(memDc, 0, 0, width, height, screenDc, left, top, Win32.SRCCOPY);

        Win32.SelectObject(memDc, oldBitmap);

        var bmi = new BITMAPINFOHEADER
        {
            biSize = (uint)Marshal.SizeOf<BITMAPINFOHEADER>(),
            biWidth = width,
            biHeight = -height, // negative = top-down DIB
            biPlanes = 1,
            biBitCount = 32,
            biCompression = 0, // BI_RGB
        };

        var bitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
        var pixelPtr = bitmap.GetPixels();

        Win32.GetDIBits(screenDc, hBitmap, 0, (uint)height, pixelPtr, ref bmi, 0);

        Win32.DeleteObject(hBitmap);
        Win32.DeleteDC(memDc);
        Win32.ReleaseDC(IntPtr.Zero, screenDc);

        return bitmap;
    }

    public SKBitmap GetAnnotatedScreenshot(List<TreeElementNode> nodes)
    {
        var screenshot = GetScreenshot();

        int padding = 5;
        int newWidth = (int)(screenshot.Width + 1.5 * padding);
        int newHeight = (int)(screenshot.Height + 1.5 * padding);

        var padded = new SKBitmap(newWidth, newHeight);
        using var canvas = new SKCanvas(padded);
        canvas.Clear(SKColors.White);
        canvas.DrawBitmap(screenshot, padding, padding);

        var fontSize = 12f;
        using var font = new SKFont(SKTypeface.Default, fontSize);
        using var textPaint = new SKPaint { Color = SKColors.White, IsAntialias = true };
        using var rectPaint = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = 2 };
        using var fillPaint = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Fill };

        var (leftOffset, topOffset, _, _) = UiaHelpers.GetVirtualScreenRect();

        for (int i = 0; i < nodes.Count; i++)
        {
            var node = nodes[i];
            var box = node.BoundingBox;
            var color = GetRandomColor();

            float adjLeft = box.Left - leftOffset + padding;
            float adjTop = box.Top - topOffset + padding;
            float adjRight = box.Right - leftOffset + padding;
            float adjBottom = box.Bottom - topOffset + padding;

            // Draw bounding box
            rectPaint.Color = color;
            canvas.DrawRect(adjLeft, adjTop, adjRight - adjLeft, adjBottom - adjTop, rectPaint);

            // Label
            var label = i.ToString();
            float labelWidth;
            using (var labelFont = new SKFont(SKTypeface.Default, fontSize))
            {
                labelWidth = labelFont.MeasureText(label);
            }
            float labelHeight = fontSize;

            float lx1 = adjRight - labelWidth;
            float ly1 = adjTop - labelHeight - 4;
            float lx2 = lx1 + labelWidth;
            float ly2 = ly1 + labelHeight + 4;

            fillPaint.Color = color;
            canvas.DrawRect(lx1, ly1, lx2 - lx1, ly2 - ly1, fillPaint);
            canvas.DrawText(label, lx1 + 2, ly2 - 2, font, textPaint);
        }

        screenshot.Dispose();
        return padded;
    }

    private static SKColor GetRandomColor()
    {
        return new SKColor(
            (byte)Rng.Next(256),
            (byte)Rng.Next(256),
            (byte)Rng.Next(256));
    }

    // ──────────────────────────────────────────────
    // Notifications
    // ──────────────────────────────────────────────

    public string SendNotification(string title, string message)
    {
        var safeTitle = DesktopUtils.PsQuoteForXml(title);
        var safeMessage = DesktopUtils.PsQuoteForXml(message);

        var psScript = string.Join('\n',
            "[Windows.UI.Notifications.ToastNotificationManager, Windows.UI.Notifications, ContentType = WindowsRuntime] | Out-Null",
            "[Windows.Data.Xml.Dom.XmlDocument, Windows.Data.Xml.Dom.XmlDocument, ContentType = WindowsRuntime] | Out-Null",
            $"$notifTitle = {safeTitle}",
            $"$notifMessage = {safeMessage}",
            "$template = @\"",
            "<toast>",
            "    <visual>",
            "        <binding template=\"ToastGeneric\">",
            "            <text>$notifTitle</text>",
            "            <text>$notifMessage</text>",
            "        </binding>",
            "    </visual>",
            "</toast>",
            "\"@",
            "$xml = New-Object Windows.Data.Xml.Dom.XmlDocument",
            "$xml.LoadXml($template)",
            "$notifier = [Windows.UI.Notifications.ToastNotificationManager]::CreateToastNotifier(\"Windows MCP\")",
            "$toast = New-Object Windows.UI.Notifications.ToastNotification $xml",
            "$notifier.Show($toast)");

        var (response, status) = ExecuteCommand(psScript);
        return status == 0
            ? $"Notification sent: \"{title}\" - {message}"
            : $"Notification may have been sent. PowerShell output: {response[..Math.Min(response.Length, 200)]}";
    }

    // ──────────────────────────────────────────────
    // Process Management
    // ──────────────────────────────────────────────

    public string ListProcesses(string? name = null, string sortBy = "memory", int limit = 20)
    {
        var procs = new List<(int Pid, string Name, double Cpu, double MemMb)>();

        foreach (var p in System.Diagnostics.Process.GetProcesses())
        {
            try
            {
                var memMb = p.WorkingSet64 / (1024.0 * 1024.0);
                procs.Add((p.Id, p.ProcessName, 0, Math.Round(memMb, 1)));
            }
            catch
            {
                // Access denied or process exited
            }
            finally
            {
                p.Dispose();
            }
        }

        if (!string.IsNullOrEmpty(name))
        {
            procs = procs
                .Where(p => Fuzz.PartialRatio(name.ToLowerInvariant(), p.Name.ToLowerInvariant()) > 60)
                .ToList();
        }

        procs = sortBy switch
        {
            "memory" => procs.OrderByDescending(p => p.MemMb).ToList(),
            "cpu" => procs.OrderByDescending(p => p.Cpu).ToList(),
            "name" => procs.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(),
            _ => procs.OrderByDescending(p => p.MemMb).ToList(),
        };

        procs = procs.Take(limit).ToList();

        if (procs.Count == 0)
            return $"No processes found{(name is not null ? $" matching {name}" : "")}.";

        var sb = new StringBuilder();
        sb.AppendLine($"Processes ({procs.Count} shown):");
        sb.AppendLine($"{"PID",-8} {"Name",-30} {"Memory",10}");
        sb.AppendLine(new string('-', 50));
        foreach (var (pid, pName, _, memMb) in procs)
            sb.AppendLine($"{pid,-8} {pName,-30} {memMb,7:F1} MB");

        return sb.ToString().TrimEnd();
    }

    public string KillProcess(string? name = null, int? pid = null, bool force = false)
    {
        if (pid is null && name is null)
            return "Error: Provide either pid or name parameter for kill mode.";

        var killed = new List<string>();

        if (pid is not null)
        {
            try
            {
                using var p = System.Diagnostics.Process.GetProcessById(pid.Value);
                var pName = p.ProcessName;
                if (force) p.Kill(); else p.CloseMainWindow();
                killed.Add($"{pName} (PID {pid})");
            }
            catch (ArgumentException)
            {
                return $"No process with PID {pid} found.";
            }
            catch (InvalidOperationException)
            {
                return $"Access denied to kill PID {pid}. Try running as administrator.";
            }
        }
        else
        {
            foreach (var p in System.Diagnostics.Process.GetProcesses())
            {
                try
                {
                    if (string.Equals(p.ProcessName, name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (force) p.Kill(); else p.CloseMainWindow();
                        killed.Add($"{p.ProcessName} (PID {p.Id})");
                    }
                }
                catch
                {
                    // Skip
                }
                finally
                {
                    p.Dispose();
                }
            }
        }

        if (killed.Count == 0)
            return $"No process matching \"{name}\" found or access denied.";

        var action = force ? "Force killed" : "Terminated";
        return $"{action}: {string.Join(", ", killed)}";
    }

    // ──────────────────────────────────────────────
    // Lock Screen
    // ──────────────────────────────────────────────

    public string LockScreen()
    {
        LockWorkStation();
        return "Screen locked.";
    }

    // ──────────────────────────────────────────────
    // System Info
    // ──────────────────────────────────────────────

    public string GetSystemInfo()
    {
        var cpuCount = Environment.ProcessorCount;
        var currentProcess = System.Diagnostics.Process.GetCurrentProcess();

        // Use PowerShell for CPU and detailed system info
        var (osInfo, _) = ExecuteCommand(
            "(Get-CimInstance Win32_OperatingSystem | Select-Object Caption,Version | " +
            "ForEach-Object { $_.Caption + ' ' + $_.Version })");

        var gcMem = GC.GetTotalMemory(false);

        var sb = new StringBuilder();
        sb.AppendLine("System Information:");
        sb.AppendLine($"  OS: {osInfo.Trim()}");
        sb.AppendLine($"  Machine: {Environment.MachineName}");
        sb.AppendLine($"  CPU Cores: {cpuCount}");
        sb.AppendLine($"  Process Memory: {Math.Round(currentProcess.WorkingSet64 / (1024.0 * 1024.0 * 1024.0), 1)} GB");

        // Disk info via PowerShell
        var (diskInfo, diskStatus) = ExecuteCommand(
            "(Get-PSDrive C | ForEach-Object { " +
            "[math]::Round($_.Used/1GB,1).ToString() + '/' + [math]::Round(($_.Used+$_.Free)/1GB,1).ToString() })");
        if (diskStatus == 0 && !string.IsNullOrWhiteSpace(diskInfo))
            sb.AppendLine($"  Disk C: {diskInfo.Trim()} GB");

        currentProcess.Dispose();
        return sb.ToString().TrimEnd();
    }

    // ──────────────────────────────────────────────
    // Registry
    // ──────────────────────────────────────────────

    public string RegistryGet(string path, string name)
    {
        var qPath = DesktopUtils.PsQuote(path);
        var qName = DesktopUtils.PsQuote(name);
        var command = $"Get-ItemProperty -Path {qPath} -Name {qName} | Select-Object -ExpandProperty {qName}";
        var (response, status) = ExecuteCommand(command);
        return status != 0
            ? $"Error reading registry: {response.Trim()}"
            : $"Registry value [{path}] \"{name}\" = {response.Trim()}";
    }

    public string RegistrySet(string path, string name, string value, string regType = "String")
    {
        var allowedTypes = new HashSet<string>
            { "String", "ExpandString", "Binary", "DWord", "MultiString", "QWord" };
        if (!allowedTypes.Contains(regType))
            return $"Error: invalid registry type '{regType}'. Allowed: {string.Join(", ", allowedTypes.Order())}";

        var qPath = DesktopUtils.PsQuote(path);
        var qName = DesktopUtils.PsQuote(name);
        var qValue = DesktopUtils.PsQuote(value);
        var command =
            $"if (-not (Test-Path {qPath})) {{ New-Item -Path {qPath} -Force | Out-Null }}; " +
            $"Set-ItemProperty -Path {qPath} -Name {qName} -Value {qValue} -Type {regType} -Force";
        var (response, status) = ExecuteCommand(command);
        return status != 0
            ? $"Error writing registry: {response.Trim()}"
            : $"Registry value [{path}] \"{name}\" set to \"{value}\" (type: {regType}).";
    }

    public string RegistryDelete(string path, string? name = null)
    {
        var qPath = DesktopUtils.PsQuote(path);
        if (name is not null)
        {
            var qName = DesktopUtils.PsQuote(name);
            var command = $"Remove-ItemProperty -Path {qPath} -Name {qName} -Force";
            var (response, status) = ExecuteCommand(command);
            return status != 0
                ? $"Error deleting registry value: {response.Trim()}"
                : $"Registry value [{path}] \"{name}\" deleted.";
        }
        else
        {
            var command = $"Remove-Item -Path {qPath} -Recurse -Force";
            var (response, status) = ExecuteCommand(command);
            return status != 0
                ? $"Error deleting registry key: {response.Trim()}"
                : $"Registry key [{path}] deleted.";
        }
    }

    public string RegistryList(string path)
    {
        var qPath = DesktopUtils.PsQuote(path);
        var command =
            $"$values = (Get-ItemProperty -Path {qPath} -ErrorAction Stop | " +
            $"Select-Object * -ExcludeProperty PS* | Format-List | Out-String).Trim(); " +
            $"$subkeys = (Get-ChildItem -Path {qPath} -ErrorAction SilentlyContinue | " +
            $"Select-Object -ExpandProperty PSChildName) -join \"`n\"; " +
            $"if ($values) {{ Write-Output \"Values:`n$values\" }}; " +
            $"if ($subkeys) {{ Write-Output \"`nSub-Keys:`n$subkeys\" }}; " +
            $"if (-not $values -and -not $subkeys) {{ Write-Output 'No values or sub-keys found.' }}";
        var (response, status) = ExecuteCommand(command);
        return status != 0
            ? $"Error listing registry: {response.Trim()}"
            : $"Registry key [{path}]:\n{response.Trim()}";
    }

    // ──────────────────────────────────────────────
    // Auto-Minimize (IDisposable pattern)
    // ──────────────────────────────────────────────

    public IDisposable AutoMinimize()
    {
        var handle = UiaHelpers.GetForegroundWindow();
        UiaHelpers.ShowWindow(handle, SW.Minimize);
        return new AutoRestoreHandle(handle);
    }

    private sealed class AutoRestoreHandle(IntPtr handle) : IDisposable
    {
        public void Dispose() => UiaHelpers.ShowWindow(handle, SW.Restore);
    }
}
