using WindowsMcp.Desktop;
using Xunit;

namespace WindowsMcp.Tests;

/// <summary>
/// Testable subclass of DesktopService for GetSystemInfo tests.
/// </summary>
file class SystemInfoDesktopService : DesktopService
{
    private readonly Dictionary<string, (string Output, int ExitCode)> _responses = new();

    public SystemInfoDesktopService() : base("utf-8") { }

    public void SetResponse(string commandSubstring, string output, int exitCode)
    {
        _responses[commandSubstring] = (output, exitCode);
    }

    public override (string Output, int ExitCode) ExecuteCommand(string command, int timeout = 10)
    {
        foreach (var kvp in _responses)
        {
            if (command.Contains(kvp.Key))
                return kvp.Value;
        }
        return ("", 0);
    }
}

public class SystemInfoTests
{
    [Fact]
    public void GetSystemInfo_ReturnsAllSections()
    {
        var desktop = new SystemInfoDesktopService();
        desktop.SetResponse("Win32_OperatingSystem", "Windows 10 10.0.19045", 0);
        desktop.SetResponse("Get-PSDrive C", "200.5/500.0", 0);

        var result = desktop.GetSystemInfo();

        Assert.Contains("System Information", result);
        Assert.Contains("Windows 10 10.0.19045", result);
        Assert.Contains("CPU Cores", result);
        Assert.Contains(Environment.ProcessorCount.ToString(), result);
        Assert.Contains("Process Memory", result);
        Assert.Contains("Disk C", result);
        Assert.Contains("200.5/500.0", result);
        Assert.Contains(Environment.MachineName, result);
    }

    [Fact]
    public void GetSystemInfo_NoDiskInfo()
    {
        var desktop = new SystemInfoDesktopService();
        desktop.SetResponse("Win32_OperatingSystem", "Windows 11", 0);
        desktop.SetResponse("Get-PSDrive C", "", 1);

        var result = desktop.GetSystemInfo();

        Assert.Contains("Windows 11", result);
        Assert.DoesNotContain("Disk C", result);
    }
}
