using System.Collections.Generic;

namespace WindowsMcp.Uia;

/// <summary>
/// EventId from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-event-ids
/// </summary>
public static class EventId
{
    public const int UIA_ToolTipOpenedEventId = 20000;
    public const int UIA_ToolTipClosedEventId = 20001;
    public const int UIA_StructureChangedEventId = 20002;
    public const int UIA_MenuOpenedEventId = 20003;
    public const int UIA_AutomationPropertyChangedEventId = 20004;
    public const int UIA_AutomationFocusChangedEventId = 20005;
    public const int UIA_AsyncContentLoadedEventId = 20006;
    public const int UIA_MenuClosedEventId = 20007;
    public const int UIA_LayoutInvalidatedEventId = 20008;
    public const int UIA_Invoke_InvokedEventId = 20009;
    public const int UIA_SelectionItem_ElementAddedToSelectionEventId = 20010;
    public const int UIA_SelectionItem_ElementRemovedFromSelectionEventId = 20011;
    public const int UIA_SelectionItem_ElementSelectedEventId = 20012;
    public const int UIA_Selection_InvalidatedEventId = 20013;
    public const int UIA_Text_TextSelectionChangedEventId = 20014;
    public const int UIA_Text_TextChangedEventId = 20015;
    public const int UIA_Window_WindowOpenedEventId = 20016;
    public const int UIA_Window_WindowClosedEventId = 20017;
    public const int UIA_MenuModeStartEventId = 20018;
    public const int UIA_MenuModeEndEventId = 20019;
    public const int UIA_InputReachedTargetEventId = 20020;
    public const int UIA_InputReachedOtherElementEventId = 20021;
    public const int UIA_InputDiscardedEventId = 20022;
    public const int UIA_SystemAlertEventId = 20023;
    public const int UIA_LiveRegionChangedEventId = 20024;
    public const int UIA_HostedFragmentRootsInvalidatedEventId = 20025;
    public const int UIA_Drag_DragStartEventId = 20026;
    public const int UIA_Drag_DragCancelEventId = 20027;
    public const int UIA_Drag_DragCompleteEventId = 20028;
    public const int UIA_DropTarget_DragEnterEventId = 20029;
    public const int UIA_DropTarget_DragLeaveEventId = 20030;
    public const int UIA_DropTarget_DroppedEventId = 20031;
    public const int UIA_TextEdit_TextChangedEventId = 20032;
    public const int UIA_TextEdit_ConversionTargetChangedEventId = 20033;
    public const int UIA_ChangesEventId = 20034;
    public const int UIA_NotificationEventId = 20035;
    public const int UIA_ActiveTextPositionChangedEventId = 20036;
}

public static class EventIdNames
{
    public static readonly Dictionary<int, string> Names = new()
    {
        { EventId.UIA_ToolTipOpenedEventId, nameof(EventId.UIA_ToolTipOpenedEventId) },
        { EventId.UIA_ToolTipClosedEventId, nameof(EventId.UIA_ToolTipClosedEventId) },
        { EventId.UIA_StructureChangedEventId, nameof(EventId.UIA_StructureChangedEventId) },
        { EventId.UIA_MenuOpenedEventId, nameof(EventId.UIA_MenuOpenedEventId) },
        { EventId.UIA_AutomationPropertyChangedEventId, nameof(EventId.UIA_AutomationPropertyChangedEventId) },
        { EventId.UIA_AutomationFocusChangedEventId, nameof(EventId.UIA_AutomationFocusChangedEventId) },
        { EventId.UIA_AsyncContentLoadedEventId, nameof(EventId.UIA_AsyncContentLoadedEventId) },
        { EventId.UIA_MenuClosedEventId, nameof(EventId.UIA_MenuClosedEventId) },
        { EventId.UIA_LayoutInvalidatedEventId, nameof(EventId.UIA_LayoutInvalidatedEventId) },
        { EventId.UIA_Invoke_InvokedEventId, nameof(EventId.UIA_Invoke_InvokedEventId) },
        { EventId.UIA_SelectionItem_ElementAddedToSelectionEventId, nameof(EventId.UIA_SelectionItem_ElementAddedToSelectionEventId) },
        { EventId.UIA_SelectionItem_ElementRemovedFromSelectionEventId, nameof(EventId.UIA_SelectionItem_ElementRemovedFromSelectionEventId) },
        { EventId.UIA_SelectionItem_ElementSelectedEventId, nameof(EventId.UIA_SelectionItem_ElementSelectedEventId) },
        { EventId.UIA_Selection_InvalidatedEventId, nameof(EventId.UIA_Selection_InvalidatedEventId) },
        { EventId.UIA_Text_TextSelectionChangedEventId, nameof(EventId.UIA_Text_TextSelectionChangedEventId) },
        { EventId.UIA_Text_TextChangedEventId, nameof(EventId.UIA_Text_TextChangedEventId) },
        { EventId.UIA_Window_WindowOpenedEventId, nameof(EventId.UIA_Window_WindowOpenedEventId) },
        { EventId.UIA_Window_WindowClosedEventId, nameof(EventId.UIA_Window_WindowClosedEventId) },
        { EventId.UIA_MenuModeStartEventId, nameof(EventId.UIA_MenuModeStartEventId) },
        { EventId.UIA_MenuModeEndEventId, nameof(EventId.UIA_MenuModeEndEventId) },
        { EventId.UIA_InputReachedTargetEventId, nameof(EventId.UIA_InputReachedTargetEventId) },
        { EventId.UIA_InputReachedOtherElementEventId, nameof(EventId.UIA_InputReachedOtherElementEventId) },
        { EventId.UIA_InputDiscardedEventId, nameof(EventId.UIA_InputDiscardedEventId) },
        { EventId.UIA_SystemAlertEventId, nameof(EventId.UIA_SystemAlertEventId) },
        { EventId.UIA_LiveRegionChangedEventId, nameof(EventId.UIA_LiveRegionChangedEventId) },
        { EventId.UIA_HostedFragmentRootsInvalidatedEventId, nameof(EventId.UIA_HostedFragmentRootsInvalidatedEventId) },
        { EventId.UIA_Drag_DragStartEventId, nameof(EventId.UIA_Drag_DragStartEventId) },
        { EventId.UIA_Drag_DragCancelEventId, nameof(EventId.UIA_Drag_DragCancelEventId) },
        { EventId.UIA_Drag_DragCompleteEventId, nameof(EventId.UIA_Drag_DragCompleteEventId) },
        { EventId.UIA_DropTarget_DragEnterEventId, nameof(EventId.UIA_DropTarget_DragEnterEventId) },
        { EventId.UIA_DropTarget_DragLeaveEventId, nameof(EventId.UIA_DropTarget_DragLeaveEventId) },
        { EventId.UIA_DropTarget_DroppedEventId, nameof(EventId.UIA_DropTarget_DroppedEventId) },
        { EventId.UIA_TextEdit_TextChangedEventId, nameof(EventId.UIA_TextEdit_TextChangedEventId) },
        { EventId.UIA_TextEdit_ConversionTargetChangedEventId, nameof(EventId.UIA_TextEdit_ConversionTargetChangedEventId) },
        { EventId.UIA_ChangesEventId, nameof(EventId.UIA_ChangesEventId) },
        { EventId.UIA_NotificationEventId, nameof(EventId.UIA_NotificationEventId) },
        { EventId.UIA_ActiveTextPositionChangedEventId, nameof(EventId.UIA_ActiveTextPositionChangedEventId) },
    };
}
