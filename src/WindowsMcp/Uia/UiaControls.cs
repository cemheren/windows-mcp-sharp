namespace WindowsMcp.Uia;

/// <summary>
/// Typed control wrapper classes for Windows UI Automation control types.
/// Each class inherits from Control and sets the appropriate ControlType automatically.
/// </summary>

public class AppBarControl : Control
{
    public AppBarControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.AppBarControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }
}

public class ButtonControl : Control
{
    public ButtonControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ButtonControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return ExpandCollapsePattern if supported (conditional support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return InvokePattern if supported (conditional support).
    /// </summary>
    public int GetInvokePattern() => GetPattern(PatternId.InvokePattern);

    /// <summary>
    /// Return TogglePattern if supported (conditional support).
    /// </summary>
    public int GetTogglePattern() => GetPattern(PatternId.TogglePattern);
}

public class CalendarControl : Control
{
    public CalendarControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.CalendarControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return GridPattern if supported (must support).
    /// </summary>
    public int GetGridPattern() => GetPattern(PatternId.GridPattern);

    /// <summary>
    /// Return TablePattern if supported (must support).
    /// </summary>
    public int GetTablePattern() => GetPattern(PatternId.TablePattern);

    /// <summary>
    /// Return ScrollPattern if supported (conditional support).
    /// </summary>
    public int GetScrollPattern() => GetPattern(PatternId.ScrollPattern);

    /// <summary>
    /// Return SelectionPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionPattern() => GetPattern(PatternId.SelectionPattern);
}

public class CheckBoxControl : Control
{
    public CheckBoxControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.CheckBoxControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return TogglePattern if supported (must support).
    /// </summary>
    public int GetTogglePattern() => GetPattern(PatternId.TogglePattern);
}

public class ComboBoxControl : Control
{
    public ComboBoxControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ComboBoxControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return ExpandCollapsePattern if supported (must support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return SelectionPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionPattern() => GetPattern(PatternId.SelectionPattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class CustomControl : Control
{
    public CustomControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.CustomControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }
}

public class DataGridControl : Control
{
    public DataGridControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.DataGridControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return GridPattern if supported (must support).
    /// </summary>
    public int GetGridPattern() => GetPattern(PatternId.GridPattern);

    /// <summary>
    /// Return ScrollPattern if supported (conditional support).
    /// </summary>
    public int GetScrollPattern() => GetPattern(PatternId.ScrollPattern);

    /// <summary>
    /// Return SelectionPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionPattern() => GetPattern(PatternId.SelectionPattern);

    /// <summary>
    /// Return TablePattern if supported (conditional support).
    /// </summary>
    public int GetTablePattern() => GetPattern(PatternId.TablePattern);
}

public class DataItemControl : Control
{
    public DataItemControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.DataItemControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return SelectionItemPattern if supported (must support).
    /// </summary>
    public int GetSelectionItemPattern() => GetPattern(PatternId.SelectionItemPattern);

    /// <summary>
    /// Return ExpandCollapsePattern if supported (conditional support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return GridItemPattern if supported (conditional support).
    /// </summary>
    public int GetGridItemPattern() => GetPattern(PatternId.GridItemPattern);

    /// <summary>
    /// Return ScrollItemPattern if supported (conditional support).
    /// </summary>
    public int GetScrollItemPattern() => GetPattern(PatternId.ScrollItemPattern);

    /// <summary>
    /// Return TableItemPattern if supported (conditional support).
    /// </summary>
    public int GetTableItemPattern() => GetPattern(PatternId.TableItemPattern);

    /// <summary>
    /// Return TogglePattern if supported (conditional support).
    /// </summary>
    public int GetTogglePattern() => GetPattern(PatternId.TogglePattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class DocumentControl : Control
{
    public DocumentControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.DocumentControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return TextPattern if supported (must support).
    /// </summary>
    public int GetTextPattern() => GetPattern(PatternId.TextPattern);

    /// <summary>
    /// Return ScrollPattern if supported (conditional support).
    /// </summary>
    public int GetScrollPattern() => GetPattern(PatternId.ScrollPattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class EditControl : Control
{
    public EditControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.EditControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return RangeValuePattern if supported (conditional support).
    /// </summary>
    public int GetRangeValuePattern() => GetPattern(PatternId.RangeValuePattern);

    /// <summary>
    /// Return TextPattern if supported (conditional support).
    /// </summary>
    public int GetTextPattern() => GetPattern(PatternId.TextPattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class GroupControl : Control
{
    public GroupControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.GroupControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return ExpandCollapsePattern if supported (conditional support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);
}

public class HeaderControl : Control
{
    public HeaderControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.HeaderControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return TransformPattern if supported (conditional support).
    /// </summary>
    public int GetTransformPattern() => GetPattern(PatternId.TransformPattern);
}

public class HeaderItemControl : Control
{
    public HeaderItemControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.HeaderItemControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return InvokePattern if supported (conditional support).
    /// </summary>
    public int GetInvokePattern() => GetPattern(PatternId.InvokePattern);

    /// <summary>
    /// Return TransformPattern if supported (conditional support).
    /// </summary>
    public int GetTransformPattern() => GetPattern(PatternId.TransformPattern);
}

public class HyperlinkControl : Control
{
    public HyperlinkControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.HyperlinkControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return InvokePattern if supported (must support).
    /// </summary>
    public int GetInvokePattern() => GetPattern(PatternId.InvokePattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class ImageControl : Control
{
    public ImageControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ImageControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return GridItemPattern if supported (conditional support).
    /// </summary>
    public int GetGridItemPattern() => GetPattern(PatternId.GridItemPattern);

    /// <summary>
    /// Return TableItemPattern if supported (conditional support).
    /// </summary>
    public int GetTableItemPattern() => GetPattern(PatternId.TableItemPattern);
}

public class ListControl : Control
{
    public ListControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ListControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return GridPattern if supported (conditional support).
    /// </summary>
    public int GetGridPattern() => GetPattern(PatternId.GridPattern);

    /// <summary>
    /// Return MultipleViewPattern if supported (conditional support).
    /// </summary>
    public int GetMultipleViewPattern() => GetPattern(PatternId.MultipleViewPattern);

    /// <summary>
    /// Return ScrollPattern if supported (conditional support).
    /// </summary>
    public int GetScrollPattern() => GetPattern(PatternId.ScrollPattern);

    /// <summary>
    /// Return SelectionPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionPattern() => GetPattern(PatternId.SelectionPattern);
}

public class ListItemControl : Control
{
    public ListItemControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ListItemControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return SelectionItemPattern if supported (must support).
    /// </summary>
    public int GetSelectionItemPattern() => GetPattern(PatternId.SelectionItemPattern);

    /// <summary>
    /// Return ExpandCollapsePattern if supported (conditional support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return GridItemPattern if supported (conditional support).
    /// </summary>
    public int GetGridItemPattern() => GetPattern(PatternId.GridItemPattern);

    /// <summary>
    /// Return InvokePattern if supported (conditional support).
    /// </summary>
    public int GetInvokePattern() => GetPattern(PatternId.InvokePattern);

    /// <summary>
    /// Return ScrollItemPattern if supported (conditional support).
    /// </summary>
    public int GetScrollItemPattern() => GetPattern(PatternId.ScrollItemPattern);

    /// <summary>
    /// Return TogglePattern if supported (conditional support).
    /// </summary>
    public int GetTogglePattern() => GetPattern(PatternId.TogglePattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class MenuControl : Control
{
    public MenuControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.MenuControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }
}

public class MenuBarControl : Control
{
    public MenuBarControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.MenuBarControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return DockPattern if supported (conditional support).
    /// </summary>
    public int GetDockPattern() => GetPattern(PatternId.DockPattern);

    /// <summary>
    /// Return ExpandCollapsePattern if supported (conditional support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return TransformPattern if supported (conditional support).
    /// </summary>
    public int GetTransformPattern() => GetPattern(PatternId.TransformPattern);
}

public class MenuItemControl : Control
{
    public MenuItemControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.MenuItemControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return ExpandCollapsePattern if supported (conditional support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return InvokePattern if supported (conditional support).
    /// </summary>
    public int GetInvokePattern() => GetPattern(PatternId.InvokePattern);

    /// <summary>
    /// Return SelectionItemPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionItemPattern() => GetPattern(PatternId.SelectionItemPattern);

    /// <summary>
    /// Return TogglePattern if supported (conditional support).
    /// </summary>
    public int GetTogglePattern() => GetPattern(PatternId.TogglePattern);
}

public class PaneControl : Control
{
    public PaneControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.PaneControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return DockPattern if supported (conditional support).
    /// </summary>
    public int GetDockPattern() => GetPattern(PatternId.DockPattern);

    /// <summary>
    /// Return ScrollPattern if supported (conditional support).
    /// </summary>
    public int GetScrollPattern() => GetPattern(PatternId.ScrollPattern);

    /// <summary>
    /// Return TransformPattern if supported (conditional support).
    /// </summary>
    public int GetTransformPattern() => GetPattern(PatternId.TransformPattern);
}

public class ProgressBarControl : Control
{
    public ProgressBarControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ProgressBarControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return RangeValuePattern if supported (conditional support).
    /// </summary>
    public int GetRangeValuePattern() => GetPattern(PatternId.RangeValuePattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class RadioButtonControl : Control
{
    public RadioButtonControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.RadioButtonControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return SelectionItemPattern if supported (must support).
    /// </summary>
    public int GetSelectionItemPattern() => GetPattern(PatternId.SelectionItemPattern);
}

public class ScrollBarControl : Control
{
    public ScrollBarControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ScrollBarControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return RangeValuePattern if supported (conditional support).
    /// </summary>
    public int GetRangeValuePattern() => GetPattern(PatternId.RangeValuePattern);
}

public class SemanticZoomControl : Control
{
    public SemanticZoomControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.SemanticZoomControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }
}

public class SeparatorControl : Control
{
    public SeparatorControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.SeparatorControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }
}

public class SliderControl : Control
{
    public SliderControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.SliderControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return RangeValuePattern if supported (conditional support).
    /// </summary>
    public int GetRangeValuePattern() => GetPattern(PatternId.RangeValuePattern);

    /// <summary>
    /// Return SelectionPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionPattern() => GetPattern(PatternId.SelectionPattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class SpinnerControl : Control
{
    public SpinnerControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.SpinnerControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return RangeValuePattern if supported (conditional support).
    /// </summary>
    public int GetRangeValuePattern() => GetPattern(PatternId.RangeValuePattern);

    /// <summary>
    /// Return SelectionPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionPattern() => GetPattern(PatternId.SelectionPattern);

    /// <summary>
    /// Return ValuePattern if supported (conditional support).
    /// </summary>
    public int GetValuePattern() => GetPattern(PatternId.ValuePattern);
}

public class SplitButtonControl : Control
{
    public SplitButtonControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.SplitButtonControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return ExpandCollapsePattern if supported (must support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return InvokePattern if supported (must support).
    /// </summary>
    public int GetInvokePattern() => GetPattern(PatternId.InvokePattern);
}

public class StatusBarControl : Control
{
    public StatusBarControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.StatusBarControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return GridPattern if supported (conditional support).
    /// </summary>
    public int GetGridPattern() => GetPattern(PatternId.GridPattern);
}

public class TabControl : Control
{
    public TabControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.TabControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return SelectionPattern if supported (must support).
    /// </summary>
    public int GetSelectionPattern() => GetPattern(PatternId.SelectionPattern);

    /// <summary>
    /// Return ScrollPattern if supported (conditional support).
    /// </summary>
    public int GetScrollPattern() => GetPattern(PatternId.ScrollPattern);
}

public class TabItemControl : Control
{
    public TabItemControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.TabItemControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return SelectionItemPattern if supported (must support).
    /// </summary>
    public int GetSelectionItemPattern() => GetPattern(PatternId.SelectionItemPattern);
}

public class TableControl : Control
{
    public TableControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.TableControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return GridPattern if supported (must support).
    /// </summary>
    public int GetGridPattern() => GetPattern(PatternId.GridPattern);

    /// <summary>
    /// Return GridItemPattern if supported (must support).
    /// </summary>
    public int GetGridItemPattern() => GetPattern(PatternId.GridItemPattern);

    /// <summary>
    /// Return TablePattern if supported (must support).
    /// </summary>
    public int GetTablePattern() => GetPattern(PatternId.TablePattern);

    /// <summary>
    /// Return TableItemPattern if supported (must support).
    /// </summary>
    public int GetTableItemPattern() => GetPattern(PatternId.TableItemPattern);
}

public class TextControl : Control
{
    public TextControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.TextControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return GridItemPattern if supported (conditional support).
    /// </summary>
    public int GetGridItemPattern() => GetPattern(PatternId.GridItemPattern);

    /// <summary>
    /// Return TableItemPattern if supported (conditional support).
    /// </summary>
    public int GetTableItemPattern() => GetPattern(PatternId.TableItemPattern);

    /// <summary>
    /// Return TextPattern if supported (conditional support).
    /// </summary>
    public int GetTextPattern() => GetPattern(PatternId.TextPattern);
}

public class ThumbControl : Control
{
    public ThumbControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ThumbControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return TransformPattern if supported (must support).
    /// </summary>
    public int GetTransformPattern() => GetPattern(PatternId.TransformPattern);
}

public class TitleBarControl : Control
{
    public TitleBarControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.TitleBarControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }
}

public class ToolBarControl : Control
{
    public ToolBarControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ToolBarControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return DockPattern if supported (conditional support).
    /// </summary>
    public int GetDockPattern() => GetPattern(PatternId.DockPattern);

    /// <summary>
    /// Return ExpandCollapsePattern if supported (conditional support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return TransformPattern if supported (conditional support).
    /// </summary>
    public int GetTransformPattern() => GetPattern(PatternId.TransformPattern);
}

public class ToolTipControl : Control
{
    public ToolTipControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.ToolTipControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return TextPattern if supported (conditional support).
    /// </summary>
    public int GetTextPattern() => GetPattern(PatternId.TextPattern);

    /// <summary>
    /// Return WindowPattern if supported (conditional support).
    /// </summary>
    public int GetWindowPattern() => GetPattern(PatternId.WindowPattern);
}

public class TreeControl : Control
{
    public TreeControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.TreeControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return ScrollPattern if supported (conditional support).
    /// </summary>
    public int GetScrollPattern() => GetPattern(PatternId.ScrollPattern);

    /// <summary>
    /// Return SelectionPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionPattern() => GetPattern(PatternId.SelectionPattern);
}

public class TreeItemControl : Control
{
    public TreeItemControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.TreeItemControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return ExpandCollapsePattern if supported (must support).
    /// </summary>
    public int GetExpandCollapsePattern() => GetPattern(PatternId.ExpandCollapsePattern);

    /// <summary>
    /// Return InvokePattern if supported (conditional support).
    /// </summary>
    public int GetInvokePattern() => GetPattern(PatternId.InvokePattern);

    /// <summary>
    /// Return ScrollItemPattern if supported (conditional support).
    /// </summary>
    public int GetScrollItemPattern() => GetPattern(PatternId.ScrollItemPattern);

    /// <summary>
    /// Return SelectionItemPattern if supported (conditional support).
    /// </summary>
    public int GetSelectionItemPattern() => GetPattern(PatternId.SelectionItemPattern);

    /// <summary>
    /// Return TogglePattern if supported (conditional support).
    /// </summary>
    public int GetTogglePattern() => GetPattern(PatternId.TogglePattern);
}

public class WindowControl : Control
{
    public WindowControl(
        string? name = null, string? automationId = null,
        int? processId = null, string? className = null,
        string? regexName = null)
        : base(controlType: ControlType.WindowControl, name: name,
               automationId: automationId, processId: processId,
               className: className, regexName: regexName)
    { }

    /// <summary>
    /// Return TransformPattern if supported (must support).
    /// </summary>
    public int GetTransformPattern() => GetPattern(PatternId.TransformPattern);

    /// <summary>
    /// Return WindowPattern if supported (must support).
    /// </summary>
    public int GetWindowPattern() => GetPattern(PatternId.WindowPattern);

    /// <summary>
    /// Return DockPattern if supported (conditional support).
    /// </summary>
    public int GetDockPattern() => GetPattern(PatternId.DockPattern);
}

/// <summary>
/// Maps ControlType IDs to their corresponding control class constructors.
/// </summary>
public static class ControlConstructors
{
    public static readonly Dictionary<int, Func<Control>> Factories = new()
    {
        { ControlType.AppBarControl, () => new AppBarControl() },
        { ControlType.ButtonControl, () => new ButtonControl() },
        { ControlType.CalendarControl, () => new CalendarControl() },
        { ControlType.CheckBoxControl, () => new CheckBoxControl() },
        { ControlType.ComboBoxControl, () => new ComboBoxControl() },
        { ControlType.CustomControl, () => new CustomControl() },
        { ControlType.DataGridControl, () => new DataGridControl() },
        { ControlType.DataItemControl, () => new DataItemControl() },
        { ControlType.DocumentControl, () => new DocumentControl() },
        { ControlType.EditControl, () => new EditControl() },
        { ControlType.GroupControl, () => new GroupControl() },
        { ControlType.HeaderControl, () => new HeaderControl() },
        { ControlType.HeaderItemControl, () => new HeaderItemControl() },
        { ControlType.HyperlinkControl, () => new HyperlinkControl() },
        { ControlType.ImageControl, () => new ImageControl() },
        { ControlType.ListControl, () => new ListControl() },
        { ControlType.ListItemControl, () => new ListItemControl() },
        { ControlType.MenuBarControl, () => new MenuBarControl() },
        { ControlType.MenuControl, () => new MenuControl() },
        { ControlType.MenuItemControl, () => new MenuItemControl() },
        { ControlType.PaneControl, () => new PaneControl() },
        { ControlType.ProgressBarControl, () => new ProgressBarControl() },
        { ControlType.RadioButtonControl, () => new RadioButtonControl() },
        { ControlType.ScrollBarControl, () => new ScrollBarControl() },
        { ControlType.SemanticZoomControl, () => new SemanticZoomControl() },
        { ControlType.SeparatorControl, () => new SeparatorControl() },
        { ControlType.SliderControl, () => new SliderControl() },
        { ControlType.SpinnerControl, () => new SpinnerControl() },
        { ControlType.SplitButtonControl, () => new SplitButtonControl() },
        { ControlType.StatusBarControl, () => new StatusBarControl() },
        { ControlType.TabControl, () => new TabControl() },
        { ControlType.TabItemControl, () => new TabItemControl() },
        { ControlType.TableControl, () => new TableControl() },
        { ControlType.TextControl, () => new TextControl() },
        { ControlType.ThumbControl, () => new ThumbControl() },
        { ControlType.TitleBarControl, () => new TitleBarControl() },
        { ControlType.ToolBarControl, () => new ToolBarControl() },
        { ControlType.ToolTipControl, () => new ToolTipControl() },
        { ControlType.TreeControl, () => new TreeControl() },
        { ControlType.TreeItemControl, () => new TreeItemControl() },
        { ControlType.WindowControl, () => new WindowControl() },
    };
}
