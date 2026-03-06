using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsMcp.Uia;

namespace WindowsMcp.Hints;

/// <summary>
/// Enumerates interactive UI elements using UIA, ported from hunt-and-peck's
/// UiAutomationHintProviderService. Uses BFS traversal with FindAll + compound
/// conditions for faster, more reliable element discovery than manual tree walking.
/// </summary>
public class HintService
{
    private static readonly string[] HintChars = [
        "s","a","d","q","t","l","e","w","c","m","p","g","h"
    ];

    /// <summary>
    /// Enumerates all interactive elements across all visible windows on the current desktop.
    /// </summary>
    public List<HintElement> EnumAllHints()
    {
        var sw = Stopwatch.StartNew();
        var allHints = new List<HintElement>();

        var windowHandles = GetVisibleWindowHandles();
        Trace.TraceInformation($"[HintService] Found {windowHandles.Count} visible windows");

        foreach (var hwnd in windowHandles)
        {
            try
            {
                var windowHints = EnumWindowHints(hwnd);
                allHints.AddRange(windowHints);
            }
            catch (Exception ex)
            {
                Trace.TraceWarning($"[HintService] Failed to enumerate window {hwnd}: {ex.Message}");
            }
        }

        // Assign labels
        var labels = GenerateLabels(allHints.Count);
        for (int i = 0; i < allHints.Count; i++)
        {
            allHints[i] = allHints[i] with { Label = labels[i] };
        }

        sw.Stop();
        Trace.TraceInformation($"[HintService] Enumerated {allHints.Count} hints in {sw.ElapsedMilliseconds}ms");
        return allHints;
    }

    /// <summary>
    /// Enumerates interactive elements for a single window handle.
    /// </summary>
    public List<HintElement> EnumWindowHints(IntPtr hWnd)
    {
        var automation = AutomationClient.Instance.Automation;
        var results = new List<HintElement>();

        IUIAutomationElement? rootElement;
        try
        {
            rootElement = automation.ElementFromHandle(hWnd);
        }
        catch
        {
            return results;
        }

        if (rootElement == null) return results;

        string windowName;
        try { windowName = rootElement.CurrentName ?? ""; }
        catch { windowName = ""; }

        // Build compound condition: ControlView AND Enabled AND OnScreen
        var condControlView = automation.GetControlViewCondition();
        var condEnabled = automation.CreatePropertyCondition(PropertyId.IsEnabledProperty, true);
        var conditions = automation.CreateAndCondition(condControlView, condEnabled);

        var condOnScreen = automation.CreatePropertyCondition(PropertyId.IsOffscreenProperty, false);
        conditions = automation.CreateAndCondition(conditions, condOnScreen);

        // BFS traversal (from hunt-and-peck)
        var elements = EnumElements(rootElement, conditions);

        foreach (var element in elements)
        {
            try
            {
                var rect = element.CurrentBoundingRectangle;
                if (rect.right <= rect.left || rect.bottom <= rect.top)
                    continue;

                string name;
                try { name = element.CurrentName ?? ""; }
                catch { name = ""; }

                string controlType;
                try { controlType = element.CurrentLocalizedControlType ?? ""; }
                catch { controlType = ""; }

                string automationId;
                try { automationId = element.CurrentAutomationId ?? ""; }
                catch { automationId = ""; }

                var patterns = DetectPatterns(element);

                // Only include elements that have at least one interaction pattern
                if (patterns.Count == 0)
                    continue;

                results.Add(new HintElement
                {
                    Label = "", // assigned later
                    Name = name,
                    ControlType = controlType,
                    AutomationId = automationId,
                    Left = rect.left,
                    Top = rect.top,
                    Right = rect.right,
                    Bottom = rect.bottom,
                    Patterns = patterns,
                    WindowName = windowName,
                });
            }
            catch
            {
                // Element may have gone stale
            }
        }

        return results;
    }

    /// <summary>
    /// BFS traversal of UIA tree using FindAll with conditions.
    /// Ported from hunt-and-peck's EnumElements method.
    /// Single-child nodes are skipped (drilled into) for better leaf-element discovery.
    /// </summary>
    private static List<IUIAutomationElement> EnumElements(
        IUIAutomationElement root,
        IUIAutomationCondition conditions)
    {
        var results = new List<IUIAutomationElement>();
        var queue = new Queue<IUIAutomationElement>();

        // Seed with direct children
        var initialChildren = root.FindAll(
            TreeScope.TreeScope_Children, conditions);
        if (initialChildren != null)
        {
            for (int i = 0; i < initialChildren.Length; i++)
            {
                try { queue.Enqueue(initialChildren.GetElement(i)); }
                catch { /* stale element */ }
            }
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            IUIAutomationElementArray? children;
            try
            {
                children = current.FindAll(TreeScope.TreeScope_Children, conditions);
            }
            catch
            {
                results.Add(current);
                continue;
            }

            if (children != null && children.Length > 0)
            {
                // Single-child optimization from hunt-and-peck:
                // If only one child, skip this node and drill into the child
                if (children.Length == 1)
                {
                    try { queue.Enqueue(children.GetElement(0)); }
                    catch { results.Add(current); }
                    continue;
                }

                for (int i = 0; i < children.Length; i++)
                {
                    try { queue.Enqueue(children.GetElement(i)); }
                    catch { /* stale element */ }
                }
            }

            results.Add(current);
        }

        return results;
    }

    /// <summary>
    /// Detects which interaction patterns an element supports.
    /// </summary>
    private static List<string> DetectPatterns(IUIAutomationElement element)
    {
        var patterns = new List<string>();

        TryAddPattern(element, PatternId.InvokePattern, "Invoke", patterns);
        TryAddPattern(element, PatternId.TogglePattern, "Toggle", patterns);
        TryAddPattern(element, PatternId.SelectionItemPattern, "Select", patterns);
        TryAddPattern(element, PatternId.ExpandCollapsePattern, "ExpandCollapse", patterns);
        TryAddPattern(element, PatternId.ValuePattern, "Value", patterns);
        TryAddPattern(element, PatternId.RangeValuePattern, "RangeValue", patterns);
        TryAddPattern(element, PatternId.ScrollPattern, "Scroll", patterns);

        return patterns;
    }

    private static void TryAddPattern(
        IUIAutomationElement element, int patternId, string name, List<string> list)
    {
        try
        {
            var pattern = element.GetCurrentPattern(patternId);
            if (pattern != null)
                list.Add(name);
        }
        catch { /* not supported */ }
    }

    /// <summary>
    /// Gets all visible top-level window handles on the current desktop.
    /// </summary>
    public static List<IntPtr> GetVisibleWindowHandles()
    {
        var handles = new List<IntPtr>();
        Win32.EnumWindows((hwnd, _) =>
        {
            try
            {
                if (UiaHelpers.IsWindowVisible(hwnd))
                {
                    handles.Add(hwnd);
                }
            }
            catch { }
            return true;
        }, IntPtr.Zero);
        return handles;
    }

    /// <summary>
    /// Generates unique labels for hints (vimium-style, from hunt-and-peck).
    /// Uses numeric labels for MCP (simpler for AI agents than letter codes).
    /// </summary>
    private static List<string> GenerateLabels(int count)
    {
        var labels = new List<string>(count);
        for (int i = 0; i < count; i++)
            labels.Add(i.ToString());
        return labels;
    }

    /// <summary>
    /// Formats hint elements as a table string for the Snapshot output.
    /// </summary>
    public static string FormatHints(List<HintElement> hints)
    {
        if (hints.Count == 0)
            return "No interactive elements found.";

        var lines = new List<string>
        {
            "# label|name|control_type|center|patterns|window"
        };

        foreach (var h in hints)
        {
            var nameDisplay = string.IsNullOrEmpty(h.Name) ? h.AutomationId : h.Name;
            if (string.IsNullOrEmpty(nameDisplay)) nameDisplay = "(unnamed)";
            var patternsStr = string.Join(",", h.Patterns);
            lines.Add($"{h.Label}|{nameDisplay}|{h.ControlType}|({h.CenterX},{h.CenterY})|{patternsStr}|{h.WindowName}");
        }

        return string.Join("\n", lines);
    }
}
