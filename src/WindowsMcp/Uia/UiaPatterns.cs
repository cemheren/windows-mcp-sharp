using System;
using System.Collections.Generic;
using System.Threading;

namespace WindowsMcp.Uia;

/// <summary>
/// Default operation wait time in milliseconds.
/// </summary>
internal static class PatternConstants
{
    public const int OperationWaitTimeMs = 500;
    public const int S_OK = 0;
}

/// <summary>
/// Wraps IUIAutomationAnnotationPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationannotationpattern
/// </summary>
public class AnnotationPattern
{
    private readonly dynamic _pattern;

    public AnnotationPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationAnnotationPattern::get_CurrentAnnotationTypeId.
    /// Returns a value in class AnnotationType.
    /// </summary>
    public int AnnotationTypeId => (int)_pattern.CurrentAnnotationTypeId;

    /// <summary>
    /// Call IUIAutomationAnnotationPattern::get_CurrentAnnotationTypeName.
    /// </summary>
    public string AnnotationTypeName => (string)_pattern.CurrentAnnotationTypeName;

    /// <summary>
    /// Call IUIAutomationAnnotationPattern::get_CurrentAuthor.
    /// </summary>
    public string Author => (string)_pattern.CurrentAuthor;

    /// <summary>
    /// Call IUIAutomationAnnotationPattern::get_CurrentDateTime.
    /// </summary>
    public string DateTime => (string)_pattern.CurrentDateTime;

    /// <summary>
    /// Call IUIAutomationAnnotationPattern::get_CurrentTarget.
    /// Returns the element that is being annotated.
    /// </summary>
    public object? Target => _pattern.CurrentTarget;
}

/// <summary>
/// Wraps IUIAutomationCustomNavigationPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationcustomnavigationpattern
/// </summary>
public class CustomNavigationPattern
{
    private readonly dynamic _pattern;

    public CustomNavigationPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationCustomNavigationPattern::Navigate.
    /// Get the next control in the specified direction within the logical UI tree.
    /// direction: a value in class NavigateDirection.
    /// </summary>
    public object? Navigate(int direction) => _pattern.Navigate(direction);
}

/// <summary>
/// Wraps IUIAutomationDockPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationdockpattern
/// </summary>
public class DockPattern
{
    private readonly dynamic _pattern;

    public DockPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationDockPattern::get_CurrentDockPosition.
    /// Returns a value in class DockPosition.
    /// </summary>
    public int DockPosition => (int)_pattern.CurrentDockPosition;

    /// <summary>
    /// Call IUIAutomationDockPattern::SetDockPosition.
    /// dockPosition: a value in class DockPosition.
    /// </summary>
    public int SetDockPosition(int dockPosition, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        int ret = (int)_pattern.SetDockPosition(dockPosition);
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationDragPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationdragpattern
/// </summary>
public class DragPattern
{
    private readonly dynamic _pattern;

    public DragPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationDragPattern::get_CurrentDropEffect.
    /// Returns a localized string that indicates what happens when the user drops this element.
    /// </summary>
    public string DropEffect => (string)_pattern.CurrentDropEffect;

    /// <summary>
    /// Call IUIAutomationDragPattern::get_CurrentDropEffects.
    /// Returns a list of localized strings enumerating the full set of drop effects.
    /// </summary>
    public object DropEffects => _pattern.CurrentDropEffects;

    /// <summary>
    /// Call IUIAutomationDragPattern::get_CurrentIsGrabbed.
    /// Indicates whether the user has grabbed this element as part of a drag-and-drop operation.
    /// </summary>
    public bool IsGrabbed => (bool)_pattern.CurrentIsGrabbed;

    /// <summary>
    /// Call IUIAutomationDragPattern::GetCurrentGrabbedItems.
    /// Returns the full set of items that the user is dragging.
    /// </summary>
    public object? GetGrabbedItems() => _pattern.GetCurrentGrabbedItems();
}

/// <summary>
/// Wraps IUIAutomationDropTargetPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationdroptargetpattern
/// </summary>
public class DropTargetPattern
{
    private readonly dynamic _pattern;

    public DropTargetPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationDropTargetPattern::get_CurrentDropTargetEffect.
    /// Returns a localized string describing what happens when the user drops the grabbed element on this drop target.
    /// </summary>
    public string DropTargetEffect => (string)_pattern.CurrentDropTargetEffect;

    /// <summary>
    /// Call IUIAutomationDropTargetPattern::get_CurrentDropTargetEffects.
    /// Returns a list of localized strings enumerating the full set of effects.
    /// </summary>
    public object DropTargetEffects => _pattern.CurrentDropTargetEffects;
}

/// <summary>
/// Wraps IUIAutomationExpandCollapsePattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationexpandcollapsepattern
/// </summary>
public class ExpandCollapsePattern
{
    private readonly dynamic _pattern;

    public ExpandCollapsePattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationExpandCollapsePattern::get_CurrentExpandCollapseState.
    /// Returns a value in class ExpandCollapseState.
    /// </summary>
    public int ExpandCollapseState => (int)_pattern.CurrentExpandCollapseState;

    /// <summary>
    /// Call IUIAutomationExpandCollapsePattern::Collapse.
    /// Returns true if succeed otherwise false.
    /// </summary>
    public bool Collapse(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        try
        {
            bool ret = (int)_pattern.Collapse() == PatternConstants.S_OK;
            Thread.Sleep(waitTimeMs);
            return ret;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Call IUIAutomationExpandCollapsePattern::Expand.
    /// Returns true if succeed otherwise false.
    /// </summary>
    public bool Expand(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        try
        {
            bool ret = (int)_pattern.Expand() == PatternConstants.S_OK;
            Thread.Sleep(waitTimeMs);
            return ret;
        }
        catch
        {
            return false;
        }
    }
}

/// <summary>
/// Wraps IUIAutomationGridItemPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationgriditempattern
/// </summary>
public class GridItemPattern
{
    private readonly dynamic _pattern;

    public GridItemPattern(object pattern) => _pattern = pattern;

    /// <summary>The zero-based index of the column that contains the item.</summary>
    public int Column => (int)_pattern.CurrentColumn;

    /// <summary>The number of columns spanned by the grid item.</summary>
    public int ColumnSpan => (int)_pattern.CurrentColumnSpan;

    /// <summary>The element that contains the grid item.</summary>
    public object? ContainingGrid => _pattern.CurrentContainingGrid;

    /// <summary>The zero-based index of the row that contains the grid item.</summary>
    public int Row => (int)_pattern.CurrentRow;

    /// <summary>The number of rows spanned by the grid item.</summary>
    public int RowSpan => (int)_pattern.CurrentRowSpan;
}

/// <summary>
/// Wraps IUIAutomationGridPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationgridpattern
/// </summary>
public class GridPattern
{
    private readonly dynamic _pattern;

    public GridPattern(object pattern) => _pattern = pattern;

    /// <summary>The number of columns in the grid.</summary>
    public int ColumnCount => (int)_pattern.CurrentColumnCount;

    /// <summary>The number of rows in the grid.</summary>
    public int RowCount => (int)_pattern.CurrentRowCount;

    /// <summary>
    /// Call IUIAutomationGridPattern::GetItem.
    /// Returns a control representing an item in the grid.
    /// </summary>
    public object? GetItem(int row, int column) => _pattern.GetItem(row, column);
}

/// <summary>
/// Wraps IUIAutomationInvokePattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationinvokepattern
/// </summary>
public class InvokePattern
{
    private readonly dynamic _pattern;

    public InvokePattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationInvokePattern::Invoke.
    /// Invoke the action of a control, such as a button click.
    /// Returns true if succeed otherwise false.
    /// </summary>
    public bool Invoke(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Invoke() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationItemContainerPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationitemcontainerpattern
/// </summary>
public class ItemContainerPattern
{
    private readonly dynamic _pattern;

    public ItemContainerPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationItemContainerPattern::FindItemByProperty.
    /// Returns a control within a containing element, based on a specified property value.
    /// </summary>
    public object? FindItemByProperty(object element, int propertyId, object propertyValue)
        => _pattern.FindItemByProperty(element, propertyId, propertyValue);
}

/// <summary>
/// Wraps IUIAutomationLegacyIAccessiblePattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationlegacyiaccessiblepattern
/// </summary>
public class LegacyIAccessiblePattern
{
    private readonly dynamic _pattern;

    public LegacyIAccessiblePattern(object pattern) => _pattern = pattern;

    /// <summary>The Microsoft Active Accessibility child identifier for the element.</summary>
    public int ChildId => (int)_pattern.CurrentChildId;

    /// <summary>The Microsoft Active Accessibility current default action for the element.</summary>
    public string DefaultAction => (string)_pattern.CurrentDefaultAction;

    /// <summary>The Microsoft Active Accessibility description of the element.</summary>
    public string Description => (string)_pattern.CurrentDescription;

    /// <summary>The Microsoft Active Accessibility help string for the element.</summary>
    public string Help => (string)_pattern.CurrentHelp;

    /// <summary>The Microsoft Active Accessibility keyboard shortcut property for the element.</summary>
    public string KeyboardShortcut => (string)_pattern.CurrentKeyboardShortcut;

    /// <summary>The Microsoft Active Accessibility name property of the element.</summary>
    public string Name => (string)(_pattern.CurrentName ?? "");

    /// <summary>The Microsoft Active Accessibility role identifier (a value in class AccessibleRole).</summary>
    public int Role => (int)_pattern.CurrentRole;

    /// <summary>The Microsoft Active Accessibility state identifier (a value in class AccessibleState).</summary>
    public int State => (int)_pattern.CurrentState;

    /// <summary>The Microsoft Active Accessibility value property.</summary>
    public string Value => (string)_pattern.CurrentValue;

    /// <summary>
    /// Call IUIAutomationLegacyIAccessiblePattern::DoDefaultAction.
    /// Perform the Microsoft Active Accessibility default action for the element.
    /// </summary>
    public bool DoDefaultAction(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.DoDefaultAction() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationLegacyIAccessiblePattern::GetCurrentSelection.
    /// Returns the selected children of this element.
    /// </summary>
    public object? GetSelection() => _pattern.GetCurrentSelection();

    /// <summary>
    /// Call IUIAutomationLegacyIAccessiblePattern::GetIAccessible.
    /// Returns an IAccessible object that corresponds to the UI Automation element.
    /// </summary>
    public object? GetIAccessible() => _pattern.GetIAccessible();

    /// <summary>
    /// Call IUIAutomationLegacyIAccessiblePattern::Select.
    /// Perform a Microsoft Active Accessibility selection.
    /// flagsSelect: a value in AccessibleSelection.
    /// </summary>
    public bool Select(int flagsSelect, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Select(flagsSelect) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationLegacyIAccessiblePattern::SetValue.
    /// Set the Microsoft Active Accessibility value property for the element.
    /// </summary>
    public bool SetValue(string value, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.SetValue(value) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationMultipleViewPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationmultipleviewpattern
/// </summary>
public class MultipleViewPattern
{
    private readonly dynamic _pattern;

    public MultipleViewPattern(object pattern) => _pattern = pattern;

    /// <summary>The control-specific identifier of the current view of the control.</summary>
    public int CurrentView => (int)_pattern.CurrentCurrentView;

    /// <summary>
    /// Call IUIAutomationMultipleViewPattern::GetCurrentSupportedViews.
    /// Returns control-specific view identifiers.
    /// </summary>
    public object GetSupportedViews() => _pattern.GetCurrentSupportedViews();

    /// <summary>
    /// Call IUIAutomationMultipleViewPattern::GetViewName.
    /// Returns the name of a control-specific view.
    /// </summary>
    public string GetViewName(int view) => (string)_pattern.GetViewName(view);

    /// <summary>
    /// Call IUIAutomationMultipleViewPattern::SetCurrentView.
    /// Set the view of the control.
    /// </summary>
    public bool SetView(int view) => (int)_pattern.SetCurrentView(view) == PatternConstants.S_OK;
}

/// <summary>
/// Wraps IUIAutomationObjectModelPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationobjectmodelpattern
/// </summary>
public class ObjectModelPattern
{
    private readonly dynamic _pattern;

    public ObjectModelPattern(object pattern) => _pattern = pattern;
}

/// <summary>
/// Wraps IUIAutomationRangeValuePattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationrangevaluepattern
/// </summary>
public class RangeValuePattern
{
    private readonly dynamic _pattern;

    public RangeValuePattern(object pattern) => _pattern = pattern;

    /// <summary>Indicates whether the value of the element can be changed.</summary>
    public bool IsReadOnly => (bool)_pattern.CurrentIsReadOnly;

    /// <summary>The value added/subtracted when a large change is made (e.g. PAGE DOWN).</summary>
    public double LargeChange => (double)_pattern.CurrentLargeChange;

    /// <summary>The maximum value of the control.</summary>
    public double Maximum => (double)_pattern.CurrentMaximum;

    /// <summary>The minimum value of the control.</summary>
    public double Minimum => (double)_pattern.CurrentMinimum;

    /// <summary>The value added/subtracted when a small change is made (e.g. arrow key).</summary>
    public double SmallChange => (double)_pattern.CurrentSmallChange;

    /// <summary>The value of the control.</summary>
    public double Value => (double)_pattern.CurrentValue;

    /// <summary>
    /// Call IUIAutomationRangeValuePattern::SetValue.
    /// Set the value of the control.
    /// </summary>
    public bool SetValue(double value, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.SetValue(value) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationScrollItemPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationscrollitempattern
/// </summary>
public class ScrollItemPattern
{
    private readonly dynamic _pattern;

    public ScrollItemPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationScrollItemPattern::ScrollIntoView.
    /// Returns true if succeed otherwise false.
    /// </summary>
    public bool ScrollIntoView(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.ScrollIntoView() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationScrollPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationscrollpattern
/// </summary>
public class ScrollPattern
{
    public const double NoScrollValue = -1;

    private readonly IUIAutomationScrollPattern _pattern;

    public ScrollPattern(object pattern) => _pattern = (IUIAutomationScrollPattern)pattern;

    /// <summary>Indicates whether the element can scroll horizontally.</summary>
    public bool HorizontallyScrollable => _pattern.CurrentHorizontallyScrollable != 0;

    /// <summary>The horizontal scroll position.</summary>
    public double HorizontalScrollPercent => _pattern.CurrentHorizontalScrollPercent;

    /// <summary>The horizontal size of the viewable region of a scrollable element.</summary>
    public double HorizontalViewSize => _pattern.CurrentHorizontalViewSize;

    /// <summary>Indicates whether the element can scroll vertically.</summary>
    public bool VerticallyScrollable => _pattern.CurrentVerticallyScrollable != 0;

    /// <summary>The vertical scroll position.</summary>
    public double VerticalScrollPercent => _pattern.CurrentVerticalScrollPercent;

    /// <summary>The vertical size of the viewable region of a scrollable element.</summary>
    public double VerticalViewSize => _pattern.CurrentVerticalViewSize;

    /// <summary>
    /// Call IUIAutomationScrollPattern::Scroll.
    /// Scroll the visible region of the content area horizontally and vertically.
    /// horizontalAmount/verticalAmount: values in ScrollAmount.
    /// </summary>
    public bool Scroll(int horizontalAmount, int verticalAmount, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        try { _pattern.Scroll(horizontalAmount, verticalAmount); }
        catch { return false; }
        Thread.Sleep(waitTimeMs);
        return true;
    }

    /// <summary>
    /// Call IUIAutomationScrollPattern::SetScrollPercent.
    /// Set the horizontal and vertical scroll positions as a percentage of the total content area.
    /// Use NoScrollValue (-1) if no scroll is desired for a direction.
    /// </summary>
    public bool SetScrollPercent(double horizontalPercent, double verticalPercent, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        try { _pattern.SetScrollPercent(horizontalPercent, verticalPercent); }
        catch { return false; }
        Thread.Sleep(waitTimeMs);
        return true;
    }
}

/// <summary>
/// Wraps IUIAutomationSelectionItemPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationselectionitempattern
/// </summary>
public class SelectionItemPattern
{
    private readonly dynamic _pattern;

    public SelectionItemPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationSelectionItemPattern::AddToSelection.
    /// Add the current element to the collection of selected items.
    /// </summary>
    public bool AddToSelection(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.AddToSelection() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>Indicates whether this item is selected.</summary>
    public bool IsSelected => (bool)_pattern.CurrentIsSelected;

    /// <summary>The element that supports IUIAutomationSelectionPattern and acts as the container for this item.</summary>
    public object? SelectionContainer => _pattern.CurrentSelectionContainer;

    /// <summary>
    /// Call IUIAutomationSelectionItemPattern::RemoveFromSelection.
    /// Remove this element from the selection.
    /// </summary>
    public bool RemoveFromSelection(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.RemoveFromSelection() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationSelectionItemPattern::Select.
    /// Clear any selected items and then select the current element.
    /// </summary>
    public bool Select(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Select() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationSelectionPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationselectionpattern
/// </summary>
public class SelectionPattern
{
    private readonly dynamic _pattern;

    public SelectionPattern(object pattern) => _pattern = pattern;

    /// <summary>Indicates whether more than one item in the container can be selected at one time.</summary>
    public bool CanSelectMultiple => (bool)_pattern.CurrentCanSelectMultiple;

    /// <summary>Indicates whether at least one item must be selected at all times.</summary>
    public bool IsSelectionRequired => (bool)_pattern.CurrentIsSelectionRequired;

    /// <summary>
    /// Call IUIAutomationSelectionPattern::GetCurrentSelection.
    /// Returns the selected elements in the container.
    /// </summary>
    public object? GetSelection() => _pattern.GetCurrentSelection();
}

/// <summary>
/// Wraps IUIAutomationSelectionPattern2.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationselectionpattern2
/// </summary>
public class SelectionPattern2 : SelectionPattern
{
    private readonly dynamic _pattern2;

    public SelectionPattern2(object pattern) : base(pattern) => _pattern2 = pattern;

    /// <summary>The currently selected element.</summary>
    public object? CurrentSelectedItem => _pattern2.CurrentCurrentSelectedItem;

    /// <summary>The first selected element.</summary>
    public object? FirstSelectedItem => _pattern2.CurrentFirstSelectedItem;

    /// <summary>The last selected element.</summary>
    public object? LastSelectedItem => _pattern2.CurrentLastSelectedItem;

    /// <summary>The item count.</summary>
    public int ItemCount => (int)_pattern2.CurrentItemCount;
}

/// <summary>
/// Wraps IUIAutomationSpreadsheetItemPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationspreadsheetitempattern
/// </summary>
public class SpreadsheetItemPattern
{
    private readonly dynamic _pattern;

    public SpreadsheetItemPattern(object pattern) => _pattern = pattern;

    /// <summary>The formula for this cell.</summary>
    public string Formula => (string)_pattern.CurrentFormula;

    /// <summary>
    /// Call IUIAutomationSpreadsheetItemPattern::GetCurrentAnnotationObjects.
    /// Returns the annotations associated with this spreadsheet cell.
    /// </summary>
    public object? GetAnnotationObjects() => _pattern.GetCurrentAnnotationObjects();

    /// <summary>
    /// Call IUIAutomationSpreadsheetItemPattern::GetCurrentAnnotationTypes.
    /// Returns annotation type values associated with this spreadsheet cell.
    /// </summary>
    public object? GetAnnotationTypes() => _pattern.GetCurrentAnnotationTypes();
}

/// <summary>
/// Wraps IUIAutomationSpreadsheetPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationspreadsheetpattern
/// </summary>
public class SpreadsheetPattern
{
    private readonly dynamic _pattern;

    public SpreadsheetPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationSpreadsheetPattern::GetItemByName.
    /// Returns the spreadsheet cell that has the specified name.
    /// </summary>
    public object? GetItemByName(string name) => _pattern.GetItemByName(name);
}

/// <summary>
/// Wraps IUIAutomationStylesPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationstylespattern
/// </summary>
public class StylesPattern
{
    private readonly dynamic _pattern;

    public StylesPattern(object pattern) => _pattern = pattern;

    /// <summary>A localized string containing the list of extended properties for an element in a document.</summary>
    public string ExtendedProperties => (string)_pattern.CurrentExtendedProperties;

    /// <summary>The fill color of an element in a document.</summary>
    public int FillColor => (int)_pattern.CurrentFillColor;

    /// <summary>The color of the pattern used to fill an element in a document.</summary>
    public int FillPatternColor => (int)_pattern.CurrentFillPatternColor;

    /// <summary>The shape of an element in a document.</summary>
    public string Shape => (string)_pattern.CurrentShape;

    /// <summary>A value in class StyleId, the visual style associated with an element in a document.</summary>
    public int StyleId => (int)_pattern.CurrentStyleId;

    /// <summary>The name of the visual style associated with an element in a document.</summary>
    public string StyleName => (string)_pattern.CurrentStyleName;
}

/// <summary>
/// Wraps IUIAutomationSynchronizedInputPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationsynchronizedinputpattern
/// </summary>
public class SynchronizedInputPattern
{
    private readonly dynamic _pattern;

    public SynchronizedInputPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationSynchronizedInputPattern::Cancel.
    /// Cause the provider to stop listening for mouse or keyboard input.
    /// </summary>
    public bool Cancel() => (int)_pattern.Cancel() == PatternConstants.S_OK;

    /// <summary>
    /// Call IUIAutomationSynchronizedInputPattern::StartListening.
    /// Cause the provider to start listening for mouse or keyboard input.
    /// </summary>
    public bool StartListening() => (int)_pattern.StartListening() == PatternConstants.S_OK;
}

/// <summary>
/// Wraps IUIAutomationTableItemPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtableitempattern
/// </summary>
public class TableItemPattern
{
    private readonly dynamic _pattern;

    public TableItemPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationTableItemPattern::GetCurrentColumnHeaderItems.
    /// Returns the column headers associated with a table item or cell.
    /// </summary>
    public object? GetColumnHeaderItems() => _pattern.GetCurrentColumnHeaderItems();

    /// <summary>
    /// Call IUIAutomationTableItemPattern::GetCurrentRowHeaderItems.
    /// Returns the row headers associated with a table item or cell.
    /// </summary>
    public object? GetRowHeaderItems() => _pattern.GetCurrentRowHeaderItems();
}

/// <summary>
/// Wraps IUIAutomationTablePattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtablepattern
/// </summary>
public class TablePattern
{
    private readonly dynamic _pattern;

    public TablePattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// A value in class RowOrColumnMajor, the primary direction of traversal for the table.
    /// </summary>
    public int RowOrColumnMajor => (int)_pattern.CurrentRowOrColumnMajor;

    /// <summary>
    /// Call IUIAutomationTablePattern::GetCurrentColumnHeaders.
    /// Returns all the column headers in a table.
    /// </summary>
    public object? GetColumnHeaders() => _pattern.GetCurrentColumnHeaders();

    /// <summary>
    /// Call IUIAutomationTablePattern::GetCurrentRowHeaders.
    /// Returns all the row headers in a table.
    /// </summary>
    public object? GetRowHeaders() => _pattern.GetCurrentRowHeaders();
}

/// <summary>
/// Wraps IUIAutomationTextRange.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtextrange
/// </summary>
public class TextRange
{
    private readonly dynamic _textRange;

    public TextRange(object textRange) => _textRange = textRange;

    /// <summary>The underlying COM text range object.</summary>
    public object RawTextRange => _textRange;

    /// <summary>
    /// Call IUIAutomationTextRange::AddToSelection.
    /// Add the text range to the collection of selected text ranges.
    /// </summary>
    public bool AddToSelection(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_textRange.AddToSelection() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::Clone.
    /// Returns a TextRange identical to the original.
    /// </summary>
    public TextRange Clone() => new TextRange(_textRange.Clone());

    /// <summary>
    /// Call IUIAutomationTextRange::Compare.
    /// Returns whether this text range has the same endpoints as another text range.
    /// </summary>
    public bool Compare(TextRange textRange) => (bool)_textRange.Compare(textRange._textRange);

    /// <summary>
    /// Call IUIAutomationTextRange::CompareEndpoints.
    /// srcEndPoint/targetEndPoint: values in class TextPatternRangeEndpoint.
    /// Returns negative if src occurs earlier, 0 if same, positive if src occurs later.
    /// </summary>
    public int CompareEndpoints(int srcEndPoint, TextRange textRange, int targetEndPoint)
        => (int)_textRange.CompareEndpoints(srcEndPoint, textRange._textRange, targetEndPoint);

    /// <summary>
    /// Call IUIAutomationTextRange::ExpandToEnclosingUnit.
    /// Normalize the text range by the specified text unit.
    /// unit: a value in class TextUnit.
    /// </summary>
    public bool ExpandToEnclosingUnit(int unit, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_textRange.ExpandToEnclosingUnit(unit) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::FindAttribute.
    /// textAttributeId: a value in class TextAttributeId.
    /// Returns a text range subset that has the specified text attribute value, or null.
    /// </summary>
    public TextRange? FindAttribute(int textAttributeId, object val, bool backward)
    {
        object? textRange = _textRange.FindAttribute(textAttributeId, val, backward ? 1 : 0);
        return textRange != null ? new TextRange(textRange) : null;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::FindText.
    /// Returns a text range subset that contains the specified text, or null.
    /// </summary>
    public TextRange? FindText(string text, bool backward, bool ignoreCase)
    {
        object? textRange = _textRange.FindText(text, backward ? 1 : 0, ignoreCase ? 1 : 0);
        return textRange != null ? new TextRange(textRange) : null;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::GetAttributeValue.
    /// textAttributeId: a value in class TextAttributeId.
    /// Returns the value of the specified text attribute.
    /// </summary>
    public object? GetAttributeValue(int textAttributeId) => _textRange.GetAttributeValue(textAttributeId);

    /// <summary>
    /// Call IUIAutomationTextRange::GetBoundingRectangles.
    /// Returns bounding rectangles for each fully or partially visible line of text in a text range.
    /// </summary>
    public object? GetBoundingRectangles() => _textRange.GetBoundingRectangles();

    /// <summary>
    /// Call IUIAutomationTextRange::GetChildren.
    /// Returns embedded objects that fall within the text range.
    /// </summary>
    public object? GetChildren() => _textRange.GetChildren();

    /// <summary>
    /// Call IUIAutomationTextRange::GetEnclosingElement.
    /// Returns the innermost UI Automation element that encloses the text range.
    /// </summary>
    public object? GetEnclosingControl() => _textRange.GetEnclosingElement();

    /// <summary>
    /// Call IUIAutomationTextRange::GetText.
    /// maxLength: the maximum length of the string to return, or -1 if no limit.
    /// Returns the plain text of the text range.
    /// </summary>
    public string GetText(int maxLength = -1) => (string)_textRange.GetText(maxLength);

    /// <summary>
    /// Call IUIAutomationTextRange::Move.
    /// Move the text range forward or backward by the specified number of text units.
    /// unit: a value in class TextUnit.
    /// count: positive moves forward, negative moves backward.
    /// Returns the number of text units actually moved.
    /// </summary>
    public int Move(int unit, int count, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        int ret = (int)_textRange.Move(unit, count);
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::MoveEndpointByRange.
    /// Move one endpoint of the current text range to the specified endpoint of a second text range.
    /// srcEndPoint/targetEndPoint: values in class TextPatternRangeEndpoint.
    /// </summary>
    public bool MoveEndpointByRange(int srcEndPoint, TextRange textRange, int targetEndPoint, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_textRange.MoveEndpointByRange(srcEndPoint, textRange._textRange, targetEndPoint) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::MoveEndpointByUnit.
    /// Move one endpoint of the text range the specified number of text units.
    /// endPoint: a value in class TextPatternRangeEndpoint.
    /// unit: a value in class TextUnit.
    /// count: positive moves forward, negative moves backward.
    /// Returns the count of units actually moved.
    /// </summary>
    public int MoveEndpointByUnit(int endPoint, int unit, int count, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        int ret = (int)_textRange.MoveEndpointByUnit(endPoint, unit, count);
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::RemoveFromSelection.
    /// Remove the text range from an existing collection of selected text.
    /// </summary>
    public bool RemoveFromSelection(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_textRange.RemoveFromSelection() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::ScrollIntoView.
    /// Cause the text control to scroll until the text range is visible in the viewport.
    /// </summary>
    public bool ScrollIntoView(bool alignTop = true, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_textRange.ScrollIntoView(alignTop ? 1 : 0) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTextRange::Select.
    /// Select the span of text that corresponds to this text range, and remove any previous selection.
    /// </summary>
    public bool Select(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_textRange.Select() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationTextChildPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtextchildpattern
/// </summary>
public class TextChildPattern
{
    private readonly dynamic _pattern;

    public TextChildPattern(object pattern) => _pattern = pattern;

    /// <summary>The nearest ancestor element that supports the Text control pattern.</summary>
    public object? TextContainer => _pattern.TextContainer;

    /// <summary>A text range that encloses this child element.</summary>
    public TextRange TextRangeValue => new TextRange(_pattern.TextRange);
}

/// <summary>
/// Wraps IUIAutomationTextEditPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtexteditpattern
/// </summary>
public class TextEditPattern
{
    private readonly dynamic _pattern;

    public TextEditPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationTextEditPattern::GetActiveComposition.
    /// Returns the active composition, or null.
    /// </summary>
    public TextRange? GetActiveComposition()
    {
        object? textRange = _pattern.GetActiveComposition();
        return textRange != null ? new TextRange(textRange) : null;
    }

    /// <summary>
    /// Call IUIAutomationTextEditPattern::GetConversionTarget.
    /// Returns the current conversion target range, or null.
    /// </summary>
    public TextRange? GetConversionTarget()
    {
        object? textRange = _pattern.GetConversionTarget();
        return textRange != null ? new TextRange(textRange) : null;
    }
}

/// <summary>
/// Wraps IUIAutomationTextPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtextpattern
/// </summary>
public class TextPattern
{
    private readonly dynamic _pattern;

    public TextPattern(object pattern) => _pattern = pattern;

    /// <summary>A text range that encloses the main text of a document.</summary>
    public TextRange DocumentRange => new TextRange(_pattern.DocumentRange);

    /// <summary>Specifies the type of text selection that is supported by the control.</summary>
    public bool SupportedTextSelection => (bool)_pattern.SupportedTextSelection;

    /// <summary>
    /// Call IUIAutomationTextPattern::GetSelection.
    /// Returns the currently selected text ranges.
    /// </summary>
    public object? GetSelection() => _pattern.GetSelection();

    /// <summary>
    /// Call IUIAutomationTextPattern::GetVisibleRanges.
    /// Returns disjoint text ranges representing contiguous spans of visible text.
    /// </summary>
    public object? GetVisibleRanges() => _pattern.GetVisibleRanges();

    /// <summary>
    /// Call IUIAutomationTextPattern::RangeFromChild.
    /// Returns a text range enclosing a child element, or null.
    /// </summary>
    public TextRange? RangeFromChild(object childElement)
    {
        object? textRange = _pattern.RangeFromChild(childElement);
        return textRange != null ? new TextRange(textRange) : null;
    }

    /// <summary>
    /// Call IUIAutomationTextPattern::RangeFromPoint.
    /// Returns the degenerate (empty) text range nearest to the specified screen coordinates, or null.
    /// </summary>
    public TextRange? RangeFromPoint(int x, int y)
    {
        object? textRange = _pattern.RangeFromPoint(new { x, y });
        return textRange != null ? new TextRange(textRange) : null;
    }
}

/// <summary>
/// Wraps IUIAutomationTextPattern2.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtextpattern2
/// </summary>
public class TextPattern2
{
    private readonly dynamic _pattern;

    public TextPattern2(object pattern) => _pattern = pattern;
}

/// <summary>
/// Wraps IUIAutomationTogglePattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtogglepattern
/// </summary>
public class TogglePattern
{
    private readonly dynamic _pattern;

    public TogglePattern(object pattern) => _pattern = pattern;

    /// <summary>A value in class ToggleState, the state of the control.</summary>
    public int ToggleState => (int)_pattern.CurrentToggleState;

    /// <summary>
    /// Call IUIAutomationTogglePattern::Toggle.
    /// Cycle through the toggle states of the control.
    /// </summary>
    public bool Toggle(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Toggle() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Set the toggle state by cycling through states until the desired state is reached.
    /// </summary>
    public bool SetToggleState(int toggleState, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        for (int i = 0; i < 6; i++)
        {
            if (ToggleState == toggleState)
                return true;
            Toggle(waitTimeMs);
        }
        return false;
    }
}

/// <summary>
/// Wraps IUIAutomationTransformPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtransformpattern
/// </summary>
public class TransformPattern
{
    private readonly dynamic _pattern;

    public TransformPattern(object pattern) => _pattern = pattern;

    /// <summary>Indicates whether the element can be moved.</summary>
    public bool CanMove => (bool)_pattern.CurrentCanMove;

    /// <summary>Indicates whether the element can be resized.</summary>
    public bool CanResize => (bool)_pattern.CurrentCanResize;

    /// <summary>Indicates whether the element can be rotated.</summary>
    public bool CanRotate => (bool)_pattern.CurrentCanRotate;

    /// <summary>
    /// Call IUIAutomationTransformPattern::Move.
    /// Move the UI Automation element.
    /// </summary>
    public bool Move(int x, int y, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Move(x, y) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTransformPattern::Resize.
    /// Resize the UI Automation element.
    /// </summary>
    public bool Resize(int width, int height, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Resize(width, height) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTransformPattern::Rotate.
    /// Rotate the UI Automation element.
    /// </summary>
    public bool Rotate(int degrees, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Rotate(degrees) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationTransformPattern2.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationtransformpattern2
/// </summary>
public class TransformPattern2
{
    private readonly dynamic _pattern;

    public TransformPattern2(object pattern) => _pattern = pattern;

    /// <summary>Indicates whether the control supports zooming of its viewport.</summary>
    public bool CanZoom => (bool)_pattern.CurrentCanZoom;

    /// <summary>The zoom level of the control's viewport.</summary>
    public double ZoomLevel => (double)_pattern.CurrentZoomLevel;

    /// <summary>The maximum zoom level of the control's viewport.</summary>
    public double ZoomMaximum => (double)_pattern.CurrentZoomMaximum;

    /// <summary>The minimum zoom level of the control's viewport.</summary>
    public double ZoomMinimum => (double)_pattern.CurrentZoomMinimum;

    /// <summary>
    /// Call IUIAutomationTransformPattern2::Zoom.
    /// Zoom the viewport of the control.
    /// </summary>
    public bool Zoom(double zoomLevel, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Zoom(zoomLevel) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationTransformPattern2::ZoomByUnit.
    /// Zoom the viewport of the control by the specified unit.
    /// zoomUnit: a value in class ZoomUnit.
    /// </summary>
    public bool ZoomByUnit(int zoomUnit, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.ZoomByUnit(zoomUnit) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationValuePattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationvaluepattern
/// </summary>
public class ValuePattern
{
    private readonly dynamic _pattern;

    public ValuePattern(object pattern) => _pattern = pattern;

    /// <summary>Indicates whether the value of the element is read-only.</summary>
    public bool IsReadOnly => (bool)_pattern.CurrentIsReadOnly;

    /// <summary>The value of the element.</summary>
    public string Value => (string)_pattern.CurrentValue;

    /// <summary>
    /// Call IUIAutomationValuePattern::SetValue.
    /// Set the value of the element.
    /// </summary>
    public bool SetValue(string value, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.SetValue(value) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationVirtualizedItemPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationvirtualizeditempattern
/// </summary>
public class VirtualizedItemPattern
{
    private readonly dynamic _pattern;

    public VirtualizedItemPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationVirtualizedItemPattern::Realize.
    /// Create a full UI Automation element for a virtualized item.
    /// </summary>
    public bool Realize(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Realize() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }
}

/// <summary>
/// Wraps IUIAutomationWindowPattern.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationclient/nn-uiautomationclient-iuiautomationwindowpattern
/// </summary>
public class WindowPattern
{
    private readonly dynamic _pattern;

    public WindowPattern(object pattern) => _pattern = pattern;

    /// <summary>
    /// Call IUIAutomationWindowPattern::Close.
    /// Close the window.
    /// </summary>
    public bool Close(int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.Close() == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>Indicates whether the window can be maximized.</summary>
    public bool CanMaximize => (bool)_pattern.CurrentCanMaximize;

    /// <summary>Indicates whether the window can be minimized.</summary>
    public bool CanMinimize => (bool)_pattern.CurrentCanMinimize;

    /// <summary>Indicates whether the window is modal.</summary>
    public bool IsModal => (bool)_pattern.CurrentIsModal;

    /// <summary>Indicates whether the window is the topmost element in the z-order.</summary>
    public bool IsTopmost => (bool)_pattern.CurrentIsTopmost;

    /// <summary>A value in class WindowInteractionState, the current state of the window for user interaction.</summary>
    public int WindowInteractionState => (int)_pattern.CurrentWindowInteractionState;

    /// <summary>A value in class WindowVisualState, whether the window is normal, maximized, or minimized.</summary>
    public int WindowVisualState => (int)_pattern.CurrentWindowVisualState;

    /// <summary>
    /// Call IUIAutomationWindowPattern::SetWindowVisualState.
    /// Minimize, maximize, or restore the window.
    /// state: a value in class WindowVisualState.
    /// </summary>
    public bool SetWindowVisualState(int state, int waitTimeMs = PatternConstants.OperationWaitTimeMs)
    {
        bool ret = (int)_pattern.SetWindowVisualState(state) == PatternConstants.S_OK;
        Thread.Sleep(waitTimeMs);
        return ret;
    }

    /// <summary>
    /// Call IUIAutomationWindowPattern::WaitForInputIdle.
    /// Block for the specified time or until the associated process enters an idle state.
    /// </summary>
    public bool WaitForInputIdle(int milliseconds) => (int)_pattern.WaitForInputIdle(milliseconds) == PatternConstants.S_OK;
}
