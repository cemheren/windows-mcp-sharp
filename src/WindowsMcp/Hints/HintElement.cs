namespace WindowsMcp.Hints;

/// <summary>
/// Represents a discoverable interactive UI element, ported from hunt-and-peck.
/// </summary>
public record HintElement
{
    public required string Label { get; init; }
    public required string Name { get; init; }
    public required string ControlType { get; init; }
    public required string AutomationId { get; init; }

    /// <summary>Bounding rectangle in screen coordinates.</summary>
    public required int Left { get; init; }
    public required int Top { get; init; }
    public required int Right { get; init; }
    public required int Bottom { get; init; }

    /// <summary>Center point for clicking.</summary>
    public int CenterX => (Left + Right) / 2;
    public int CenterY => (Top + Bottom) / 2;

    /// <summary>Supported interaction patterns.</summary>
    public required List<string> Patterns { get; init; }

    /// <summary>The window this element belongs to.</summary>
    public required string WindowName { get; init; }

    public override string ToString()
    {
        var patternsStr = Patterns.Count > 0 ? string.Join(",", Patterns) : "none";
        var nameDisplay = string.IsNullOrEmpty(Name) ? AutomationId : Name;
        return $"{Label}|{nameDisplay}|{ControlType}|({CenterX},{CenterY})|{patternsStr}";
    }
}
