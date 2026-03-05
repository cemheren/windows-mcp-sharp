using System.Security;

namespace WindowsMcp.Desktop;

/// <summary>
/// Centralized input sanitization for PowerShell commands.
/// </summary>
public static class DesktopUtils
{
    /// <summary>
    /// Wrap value in PowerShell single-quoted string literal (escapes ' as '').
    /// </summary>
    public static string PsQuote(string value)
    {
        return "'" + value.Replace("'", "''") + "'";
    }

    /// <summary>
    /// XML-escape then PsQuote. Use for values in XML passed to PowerShell.
    /// </summary>
    public static string PsQuoteForXml(string value)
    {
        var escaped = SecurityElement.Escape(value);
        return PsQuote(escaped);
    }
}
