namespace WindowsMcp.Desktop;

public static class DesktopConfig
{
    public static readonly HashSet<string> BrowserNames =
    [
        "msedge.exe",
        "chrome.exe",
        "firefox.exe",
    ];

    public static readonly HashSet<string> AvoidedApps =
    [
        "AgentUI",
    ];

    public static readonly HashSet<string> ExcludedApps =
    [
        "Progman",
        "Shell_TrayWnd",
        "Shell_SecondaryTrayWnd",
        "Microsoft.UI.Content.PopupWindowSiteBridge",
        "Windows.UI.Core.CoreWindow",
    ];
}
