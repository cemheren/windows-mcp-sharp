using WindowsMcp.Desktop;
using Xunit;

namespace WindowsMcp.Tests;

/// <summary>
/// Testable subclass of DesktopService that allows mocking ExecuteCommand
/// without native interop dependencies.
/// </summary>
file class TestableDesktopService : DesktopService
{
    private Func<string, (string Output, int ExitCode)> _executeFunc;

    public string? LastCommand { get; private set; }

    public TestableDesktopService(Func<string, (string, int)>? executeFunc = null)
        : base("utf-8")
    {
        _executeFunc = executeFunc ?? (_ => ("", 0));
    }

    public void SetExecuteResult(string output, int exitCode)
    {
        _executeFunc = _ => (output, exitCode);
    }

    public override (string Output, int ExitCode) ExecuteCommand(string command, int timeout = 10)
    {
        LastCommand = command;
        return _executeFunc(command);
    }
}

public class PsQuoteTests
{
    [Fact]
    public void SimpleString()
    {
        Assert.Equal("'hello'", DesktopUtils.PsQuote("hello"));
    }

    [Fact]
    public void SingleQuoteEscaping()
    {
        Assert.Equal("'it''s'", DesktopUtils.PsQuote("it's"));
    }

    [Fact]
    public void DoubleQuotesNotEscaped()
    {
        Assert.Equal("'say \"hi\"'", DesktopUtils.PsQuote("say \"hi\""));
    }

    [Fact]
    public void DollarSignNotExpanded()
    {
        Assert.Equal("'$env:PATH'", DesktopUtils.PsQuote("$env:PATH"));
    }

    [Fact]
    public void EmptyString()
    {
        Assert.Equal("''", DesktopUtils.PsQuote(""));
    }

    [Fact]
    public void RegistryPath()
    {
        var result = DesktopUtils.PsQuote(@"HKCU:\Software\Test");
        Assert.Equal(@"'HKCU:\Software\Test'", result);
    }
}

public class RegistryGetTests
{
    [Fact]
    public void Success()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("42\n", 0);

        var result = desktop.RegistryGet(path: @"HKCU:\Software\Test", name: "MyValue");

        Assert.Contains("MyValue", result);
        Assert.Contains("42", result);
        Assert.DoesNotContain("Error", result);
    }

    [Fact]
    public void Failure()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("Property not found", 1);

        var result = desktop.RegistryGet(path: @"HKCU:\Software\Test", name: "Missing");

        Assert.Contains("Error reading registry", result);
        Assert.Contains("Property not found", result);
    }

    [Fact]
    public void CommandUsesPsQuote()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("val", 0);

        desktop.RegistryGet(path: @"HKCU:\Software\O'Reilly", name: "key's");

        Assert.Contains("HKCU:\\Software\\O''Reilly", desktop.LastCommand);
        Assert.Contains("key''s", desktop.LastCommand);
    }
}

public class RegistrySetTests
{
    [Fact]
    public void Success()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("", 0);

        var result = desktop.RegistrySet(path: @"HKCU:\Software\Test", name: "MyKey", value: "hello");

        Assert.Contains("set to", result);
        Assert.Contains("\"hello\"", result);
    }

    [Fact]
    public void Failure()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("Access denied", 1);

        var result = desktop.RegistrySet(path: @"HKLM:\Software\Test", name: "Key", value: "val");

        Assert.Contains("Error writing registry", result);
    }

    [Fact]
    public void InvalidType()
    {
        var desktop = new TestableDesktopService();

        var result = desktop.RegistrySet(path: @"HKCU:\Test", name: "Key", value: "val", regType: "Invalid");

        Assert.Contains("Error: invalid registry type", result);
        Assert.Contains("Invalid", result);
        Assert.Null(desktop.LastCommand);
    }

    [Theory]
    [InlineData("String")]
    [InlineData("ExpandString")]
    [InlineData("Binary")]
    [InlineData("DWord")]
    [InlineData("MultiString")]
    [InlineData("QWord")]
    public void AllValidTypes(string regType)
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("", 0);

        var result = desktop.RegistrySet(path: @"HKCU:\Test", name: "K", value: "V", regType: regType);

        Assert.DoesNotContain("Error", result);
    }

    [Fact]
    public void CreatesKeyIfMissing()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("", 0);

        desktop.RegistrySet(path: @"HKCU:\Software\NewKey", name: "Val", value: "1");

        Assert.Contains("New-Item", desktop.LastCommand);
        Assert.Contains("Test-Path", desktop.LastCommand);
    }
}

public class RegistryDeleteTests
{
    [Fact]
    public void DeleteValue()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("", 0);

        var result = desktop.RegistryDelete(path: @"HKCU:\Software\Test", name: "MyValue");

        Assert.Contains("deleted", result);
        Assert.Contains("\"MyValue\"", result);
        Assert.Contains("Remove-ItemProperty", desktop.LastCommand);
    }

    [Fact]
    public void DeleteKey()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("", 0);

        var result = desktop.RegistryDelete(path: @"HKCU:\Software\Test", name: null);

        Assert.Contains("key", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("deleted", result);
        Assert.Contains("Remove-Item", desktop.LastCommand);
        Assert.Contains("-Recurse", desktop.LastCommand);
    }

    [Fact]
    public void DeleteValueFailure()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("Not found", 1);

        var result = desktop.RegistryDelete(path: @"HKCU:\Software\Test", name: "Missing");

        Assert.Contains("Error deleting registry value", result);
    }

    [Fact]
    public void DeleteKeyFailure()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("Access denied", 1);

        var result = desktop.RegistryDelete(path: @"HKCU:\Software\Protected");

        Assert.Contains("Error deleting registry key", result);
    }
}

public class RegistryListTests
{
    [Fact]
    public void Success()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("Values:\nMyKey : hello\n\nSub-Keys:\nChild1", 0);

        var result = desktop.RegistryList(path: @"HKCU:\Software\Test");

        Assert.Contains("MyKey", result);
        Assert.Contains("hello", result);
        Assert.Contains("Child1", result);
    }

    [Fact]
    public void Failure()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("Path not found", 1);

        var result = desktop.RegistryList(path: @"HKCU:\Software\Missing");

        Assert.Contains("Error listing registry", result);
    }

    [Fact]
    public void Empty()
    {
        var desktop = new TestableDesktopService();
        desktop.SetExecuteResult("No values or sub-keys found.", 0);

        var result = desktop.RegistryList(path: @"HKCU:\Software\Empty");

        Assert.Contains("No values or sub-keys found", result);
    }
}
