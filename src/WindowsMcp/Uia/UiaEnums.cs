using System.Collections.Generic;

namespace WindowsMcp.Uia;

/// <summary>
/// ControlType IDs from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-controltype-ids
/// </summary>
public static class ControlType
{
    public const int AppBarControl = 50040;
    public const int ButtonControl = 50000;
    public const int CalendarControl = 50001;
    public const int CheckBoxControl = 50002;
    public const int ComboBoxControl = 50003;
    public const int CustomControl = 50025;
    public const int DataGridControl = 50028;
    public const int DataItemControl = 50029;
    public const int DocumentControl = 50030;
    public const int EditControl = 50004;
    public const int GroupControl = 50026;
    public const int HeaderControl = 50034;
    public const int HeaderItemControl = 50035;
    public const int HyperlinkControl = 50005;
    public const int ImageControl = 50006;
    public const int ListControl = 50008;
    public const int ListItemControl = 50007;
    public const int MenuBarControl = 50010;
    public const int MenuControl = 50009;
    public const int MenuItemControl = 50011;
    public const int PaneControl = 50033;
    public const int ProgressBarControl = 50012;
    public const int RadioButtonControl = 50013;
    public const int ScrollBarControl = 50014;
    public const int SemanticZoomControl = 50039;
    public const int SeparatorControl = 50038;
    public const int SliderControl = 50015;
    public const int SpinnerControl = 50016;
    public const int SplitButtonControl = 50031;
    public const int StatusBarControl = 50017;
    public const int TabControl = 50018;
    public const int TabItemControl = 50019;
    public const int TableControl = 50036;
    public const int TextControl = 50020;
    public const int ThumbControl = 50027;
    public const int TitleBarControl = 50037;
    public const int ToolBarControl = 50021;
    public const int ToolTipControl = 50022;
    public const int TreeControl = 50023;
    public const int TreeItemControl = 50024;
    public const int WindowControl = 50032;
}

public static class ControlTypeNames
{
    public static readonly Dictionary<int, string> Names = new()
    {
        { ControlType.AppBarControl, "AppBarControl" },
        { ControlType.ButtonControl, "ButtonControl" },
        { ControlType.CalendarControl, "CalendarControl" },
        { ControlType.CheckBoxControl, "CheckBoxControl" },
        { ControlType.ComboBoxControl, "ComboBoxControl" },
        { ControlType.CustomControl, "CustomControl" },
        { ControlType.DataGridControl, "DataGridControl" },
        { ControlType.DataItemControl, "DataItemControl" },
        { ControlType.DocumentControl, "DocumentControl" },
        { ControlType.EditControl, "EditControl" },
        { ControlType.GroupControl, "GroupControl" },
        { ControlType.HeaderControl, "HeaderControl" },
        { ControlType.HeaderItemControl, "HeaderItemControl" },
        { ControlType.HyperlinkControl, "HyperlinkControl" },
        { ControlType.ImageControl, "ImageControl" },
        { ControlType.ListControl, "ListControl" },
        { ControlType.ListItemControl, "ListItemControl" },
        { ControlType.MenuBarControl, "MenuBarControl" },
        { ControlType.MenuControl, "MenuControl" },
        { ControlType.MenuItemControl, "MenuItemControl" },
        { ControlType.PaneControl, "PaneControl" },
        { ControlType.ProgressBarControl, "ProgressBarControl" },
        { ControlType.RadioButtonControl, "RadioButtonControl" },
        { ControlType.ScrollBarControl, "ScrollBarControl" },
        { ControlType.SemanticZoomControl, "SemanticZoomControl" },
        { ControlType.SeparatorControl, "SeparatorControl" },
        { ControlType.SliderControl, "SliderControl" },
        { ControlType.SpinnerControl, "SpinnerControl" },
        { ControlType.SplitButtonControl, "SplitButtonControl" },
        { ControlType.StatusBarControl, "StatusBarControl" },
        { ControlType.TabControl, "TabControl" },
        { ControlType.TabItemControl, "TabItemControl" },
        { ControlType.TableControl, "TableControl" },
        { ControlType.TextControl, "TextControl" },
        { ControlType.ThumbControl, "ThumbControl" },
        { ControlType.TitleBarControl, "TitleBarControl" },
        { ControlType.ToolBarControl, "ToolBarControl" },
        { ControlType.ToolTipControl, "ToolTipControl" },
        { ControlType.TreeControl, "TreeControl" },
        { ControlType.TreeItemControl, "TreeItemControl" },
        { ControlType.WindowControl, "WindowControl" },
    };
}

/// <summary>
/// PatternId from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-controlpattern-ids
/// </summary>
public static class PatternId
{
    public const int AnnotationPattern = 10023;
    public const int CustomNavigationPattern = 10033;
    public const int DockPattern = 10011;
    public const int DragPattern = 10030;
    public const int DropTargetPattern = 10031;
    public const int ExpandCollapsePattern = 10005;
    public const int GridItemPattern = 10007;
    public const int GridPattern = 10006;
    public const int InvokePattern = 10000;
    public const int ItemContainerPattern = 10019;
    public const int LegacyIAccessiblePattern = 10018;
    public const int MultipleViewPattern = 10008;
    public const int ObjectModelPattern = 10022;
    public const int RangeValuePattern = 10003;
    public const int ScrollItemPattern = 10017;
    public const int ScrollPattern = 10004;
    public const int SelectionItemPattern = 10010;
    public const int SelectionPattern = 10001;
    public const int SpreadsheetItemPattern = 10027;
    public const int SpreadsheetPattern = 10026;
    public const int StylesPattern = 10025;
    public const int SynchronizedInputPattern = 10021;
    public const int TableItemPattern = 10013;
    public const int TablePattern = 10012;
    public const int TextChildPattern = 10029;
    public const int TextEditPattern = 10032;
    public const int TextPattern = 10014;
    public const int TextPattern2 = 10024;
    public const int TogglePattern = 10015;
    public const int TransformPattern = 10016;
    public const int TransformPattern2 = 10028;
    public const int ValuePattern = 10002;
    public const int VirtualizedItemPattern = 10020;
    public const int WindowPattern = 10009;
    public const int SelectionPattern2 = 10034;
}

public static class PatternIdNames
{
    public static readonly Dictionary<int, string> Names = new()
    {
        { PatternId.AnnotationPattern, "AnnotationPattern" },
        { PatternId.CustomNavigationPattern, "CustomNavigationPattern" },
        { PatternId.DockPattern, "DockPattern" },
        { PatternId.DragPattern, "DragPattern" },
        { PatternId.DropTargetPattern, "DropTargetPattern" },
        { PatternId.ExpandCollapsePattern, "ExpandCollapsePattern" },
        { PatternId.GridItemPattern, "GridItemPattern" },
        { PatternId.GridPattern, "GridPattern" },
        { PatternId.InvokePattern, "InvokePattern" },
        { PatternId.ItemContainerPattern, "ItemContainerPattern" },
        { PatternId.LegacyIAccessiblePattern, "LegacyIAccessiblePattern" },
        { PatternId.MultipleViewPattern, "MultipleViewPattern" },
        { PatternId.ObjectModelPattern, "ObjectModelPattern" },
        { PatternId.RangeValuePattern, "RangeValuePattern" },
        { PatternId.ScrollItemPattern, "ScrollItemPattern" },
        { PatternId.ScrollPattern, "ScrollPattern" },
        { PatternId.SelectionItemPattern, "SelectionItemPattern" },
        { PatternId.SelectionPattern, "SelectionPattern" },
        { PatternId.SpreadsheetItemPattern, "SpreadsheetItemPattern" },
        { PatternId.SpreadsheetPattern, "SpreadsheetPattern" },
        { PatternId.StylesPattern, "StylesPattern" },
        { PatternId.SynchronizedInputPattern, "SynchronizedInputPattern" },
        { PatternId.TableItemPattern, "TableItemPattern" },
        { PatternId.TablePattern, "TablePattern" },
        { PatternId.TextChildPattern, "TextChildPattern" },
        { PatternId.TextEditPattern, "TextEditPattern" },
        { PatternId.TextPattern, "TextPattern" },
        { PatternId.TextPattern2, "TextPattern2" },
        { PatternId.TogglePattern, "TogglePattern" },
        { PatternId.TransformPattern, "TransformPattern" },
        { PatternId.TransformPattern2, "TransformPattern2" },
        { PatternId.ValuePattern, "ValuePattern" },
        { PatternId.VirtualizedItemPattern, "VirtualizedItemPattern" },
        { PatternId.WindowPattern, "WindowPattern" },
        { PatternId.SelectionPattern2, "SelectionPattern2" },
    };
}

/// <summary>
/// PropertyId from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-automation-element-propids
/// Refer https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-control-pattern-propids
/// </summary>
public static class PropertyId
{
    public const int AcceleratorKeyProperty = 30006;
    public const int AccessKeyProperty = 30007;
    public const int AnnotationAnnotationTypeIdProperty = 30113;
    public const int AnnotationAnnotationTypeNameProperty = 30114;
    public const int AnnotationAuthorProperty = 30115;
    public const int AnnotationDateTimeProperty = 30116;
    public const int AnnotationObjectsProperty = 30156;
    public const int AnnotationTargetProperty = 30117;
    public const int AnnotationTypesProperty = 30155;
    public const int AriaPropertiesProperty = 30102;
    public const int AriaRoleProperty = 30101;
    public const int AutomationIdProperty = 30011;
    public const int BoundingRectangleProperty = 30001;
    public const int CenterPointProperty = 30165;
    public const int ClassNameProperty = 30012;
    public const int ClickablePointProperty = 30014;
    public const int ControlTypeProperty = 30003;
    public const int ControllerForProperty = 30104;
    public const int CultureProperty = 30015;
    public const int DescribedByProperty = 30105;
    public const int DockDockPositionProperty = 30069;
    public const int DragDropEffectProperty = 30139;
    public const int DragDropEffectsProperty = 30140;
    public const int DragGrabbedItemsProperty = 30144;
    public const int DragIsGrabbedProperty = 30138;
    public const int DropTargetDropTargetEffectProperty = 30142;
    public const int DropTargetDropTargetEffectsProperty = 30143;
    public const int ExpandCollapseExpandCollapseStateProperty = 30070;
    public const int FillColorProperty = 30160;
    public const int FillTypeProperty = 30162;
    public const int FlowsFromProperty = 30148;
    public const int FlowsToProperty = 30106;
    public const int FrameworkIdProperty = 30024;
    public const int FullDescriptionProperty = 30159;
    public const int GridColumnCountProperty = 30063;
    public const int GridItemColumnProperty = 30065;
    public const int GridItemColumnSpanProperty = 30067;
    public const int GridItemContainingGridProperty = 30068;
    public const int GridItemRowProperty = 30064;
    public const int GridItemRowSpanProperty = 30066;
    public const int GridRowCountProperty = 30062;
    public const int HasKeyboardFocusProperty = 30008;
    public const int HelpTextProperty = 30013;
    public const int IsAnnotationPatternAvailableProperty = 30118;
    public const int IsContentElementProperty = 30017;
    public const int IsControlElementProperty = 30016;
    public const int IsCustomNavigationPatternAvailableProperty = 30151;
    public const int IsDataValidForFormProperty = 30103;
    public const int IsDockPatternAvailableProperty = 30027;
    public const int IsDragPatternAvailableProperty = 30137;
    public const int IsDropTargetPatternAvailableProperty = 30141;
    public const int IsEnabledProperty = 30010;
    public const int IsExpandCollapsePatternAvailableProperty = 30028;
    public const int IsGridItemPatternAvailableProperty = 30029;
    public const int IsGridPatternAvailableProperty = 30030;
    public const int IsInvokePatternAvailableProperty = 30031;
    public const int IsItemContainerPatternAvailableProperty = 30108;
    public const int IsKeyboardFocusableProperty = 30009;
    public const int IsLegacyIAccessiblePatternAvailableProperty = 30090;
    public const int IsMultipleViewPatternAvailableProperty = 30032;
    public const int IsObjectModelPatternAvailableProperty = 30112;
    public const int IsOffscreenProperty = 30022;
    public const int IsPasswordProperty = 30019;
    public const int IsPeripheralProperty = 30150;
    public const int IsRangeValuePatternAvailableProperty = 30033;
    public const int IsRequiredForFormProperty = 30025;
    public const int IsScrollItemPatternAvailableProperty = 30035;
    public const int IsScrollPatternAvailableProperty = 30034;
    public const int IsSelectionItemPatternAvailableProperty = 30036;
    public const int IsSelectionPattern2AvailableProperty = 30168;
    public const int IsSelectionPatternAvailableProperty = 30037;
    public const int IsSpreadsheetItemPatternAvailableProperty = 30132;
    public const int IsSpreadsheetPatternAvailableProperty = 30128;
    public const int IsStylesPatternAvailableProperty = 30127;
    public const int IsSynchronizedInputPatternAvailableProperty = 30110;
    public const int IsTableItemPatternAvailableProperty = 30039;
    public const int IsTablePatternAvailableProperty = 30038;
    public const int IsTextChildPatternAvailableProperty = 30136;
    public const int IsTextEditPatternAvailableProperty = 30149;
    public const int IsTextPattern2AvailableProperty = 30119;
    public const int IsTextPatternAvailableProperty = 30040;
    public const int IsTogglePatternAvailableProperty = 30041;
    public const int IsTransformPattern2AvailableProperty = 30134;
    public const int IsTransformPatternAvailableProperty = 30042;
    public const int IsValuePatternAvailableProperty = 30043;
    public const int IsVirtualizedItemPatternAvailableProperty = 30109;
    public const int IsWindowPatternAvailableProperty = 30044;
    public const int ItemStatusProperty = 30026;
    public const int ItemTypeProperty = 30021;
    public const int LabeledByProperty = 30018;
    public const int LandmarkTypeProperty = 30157;
    public const int LegacyIAccessibleChildIdProperty = 30091;
    public const int LegacyIAccessibleDefaultActionProperty = 30100;
    public const int LegacyIAccessibleDescriptionProperty = 30094;
    public const int LegacyIAccessibleHelpProperty = 30097;
    public const int LegacyIAccessibleKeyboardShortcutProperty = 30098;
    public const int LegacyIAccessibleNameProperty = 30092;
    public const int LegacyIAccessibleRoleProperty = 30095;
    public const int LegacyIAccessibleSelectionProperty = 30099;
    public const int LegacyIAccessibleStateProperty = 30096;
    public const int LegacyIAccessibleValueProperty = 30093;
    public const int LevelProperty = 30154;
    public const int LiveSettingProperty = 30135;
    public const int LocalizedControlTypeProperty = 30004;
    public const int LocalizedLandmarkTypeProperty = 30158;
    public const int MultipleViewCurrentViewProperty = 30071;
    public const int MultipleViewSupportedViewsProperty = 30072;
    public const int NameProperty = 30005;
    public const int NativeWindowHandleProperty = 30020;
    public const int OptimizeForVisualContentProperty = 30111;
    public const int OrientationProperty = 30023;
    public const int OutlineColorProperty = 30161;
    public const int OutlineThicknessProperty = 30164;
    public const int PositionInSetProperty = 30152;
    public const int ProcessIdProperty = 30002;
    public const int ProviderDescriptionProperty = 30107;
    public const int RangeValueIsReadOnlyProperty = 30048;
    public const int RangeValueLargeChangeProperty = 30051;
    public const int RangeValueMaximumProperty = 30050;
    public const int RangeValueMinimumProperty = 30049;
    public const int RangeValueSmallChangeProperty = 30052;
    public const int RangeValueValueProperty = 30047;
    public const int RotationProperty = 30166;
    public const int RuntimeIdProperty = 30000;
    public const int ScrollHorizontalScrollPercentProperty = 30053;
    public const int ScrollHorizontalViewSizeProperty = 30054;
    public const int ScrollHorizontallyScrollableProperty = 30057;
    public const int ScrollVerticalScrollPercentProperty = 30055;
    public const int ScrollVerticalViewSizeProperty = 30056;
    public const int ScrollVerticallyScrollableProperty = 30058;
    public const int Selection2CurrentSelectedItemProperty = 30171;
    public const int Selection2FirstSelectedItemProperty = 30169;
    public const int Selection2ItemCountProperty = 30172;
    public const int Selection2LastSelectedItemProperty = 30170;
    public const int SelectionCanSelectMultipleProperty = 30060;
    public const int SelectionIsSelectionRequiredProperty = 30061;
    public const int SelectionItemIsSelectedProperty = 30079;
    public const int SelectionItemSelectionContainerProperty = 30080;
    public const int SelectionSelectionProperty = 30059;
    public const int SizeOfSetProperty = 30153;
    public const int SizeProperty = 30167;
    public const int SpreadsheetItemAnnotationObjectsProperty = 30130;
    public const int SpreadsheetItemAnnotationTypesProperty = 30131;
    public const int SpreadsheetItemFormulaProperty = 30129;
    public const int StylesExtendedPropertiesProperty = 30126;
    public const int StylesFillColorProperty = 30122;
    public const int StylesFillPatternColorProperty = 30125;
    public const int StylesFillPatternStyleProperty = 30123;
    public const int StylesShapeProperty = 30124;
    public const int StylesStyleIdProperty = 30120;
    public const int StylesStyleNameProperty = 30121;
    public const int TableColumnHeadersProperty = 30082;
    public const int TableItemColumnHeaderItemsProperty = 30085;
    public const int TableItemRowHeaderItemsProperty = 30084;
    public const int TableRowHeadersProperty = 30081;
    public const int TableRowOrColumnMajorProperty = 30083;
    public const int ToggleToggleStateProperty = 30086;
    public const int Transform2CanZoomProperty = 30133;
    public const int Transform2ZoomLevelProperty = 30145;
    public const int Transform2ZoomMaximumProperty = 30147;
    public const int Transform2ZoomMinimumProperty = 30146;
    public const int TransformCanMoveProperty = 30087;
    public const int TransformCanResizeProperty = 30088;
    public const int TransformCanRotateProperty = 30089;
    public const int ValueIsReadOnlyProperty = 30046;
    public const int ValueValueProperty = 30045;
    public const int VisualEffectsProperty = 30163;
    public const int WindowCanMaximizeProperty = 30073;
    public const int WindowCanMinimizeProperty = 30074;
    public const int WindowIsModalProperty = 30077;
    public const int WindowIsTopmostProperty = 30078;
    public const int WindowWindowInteractionStateProperty = 30076;
    public const int WindowWindowVisualStateProperty = 30075;
}

public static class PropertyIdNames
{
    public static readonly Dictionary<int, string> Names = new()
    {
        { PropertyId.AcceleratorKeyProperty, "AcceleratorKeyProperty" },
        { PropertyId.AccessKeyProperty, "AccessKeyProperty" },
        { PropertyId.AnnotationAnnotationTypeIdProperty, "AnnotationAnnotationTypeIdProperty" },
        { PropertyId.AnnotationAnnotationTypeNameProperty, "AnnotationAnnotationTypeNameProperty" },
        { PropertyId.AnnotationAuthorProperty, "AnnotationAuthorProperty" },
        { PropertyId.AnnotationDateTimeProperty, "AnnotationDateTimeProperty" },
        { PropertyId.AnnotationObjectsProperty, "AnnotationObjectsProperty" },
        { PropertyId.AnnotationTargetProperty, "AnnotationTargetProperty" },
        { PropertyId.AnnotationTypesProperty, "AnnotationTypesProperty" },
        { PropertyId.AriaPropertiesProperty, "AriaPropertiesProperty" },
        { PropertyId.AriaRoleProperty, "AriaRoleProperty" },
        { PropertyId.AutomationIdProperty, "AutomationIdProperty" },
        { PropertyId.BoundingRectangleProperty, "BoundingRectangleProperty" },
        { PropertyId.CenterPointProperty, "CenterPointProperty" },
        { PropertyId.ClassNameProperty, "ClassNameProperty" },
        { PropertyId.ClickablePointProperty, "ClickablePointProperty" },
        { PropertyId.ControlTypeProperty, "ControlTypeProperty" },
        { PropertyId.ControllerForProperty, "ControllerForProperty" },
        { PropertyId.CultureProperty, "CultureProperty" },
        { PropertyId.DescribedByProperty, "DescribedByProperty" },
        { PropertyId.DockDockPositionProperty, "DockDockPositionProperty" },
        { PropertyId.DragDropEffectProperty, "DragDropEffectProperty" },
        { PropertyId.DragDropEffectsProperty, "DragDropEffectsProperty" },
        { PropertyId.DragGrabbedItemsProperty, "DragGrabbedItemsProperty" },
        { PropertyId.DragIsGrabbedProperty, "DragIsGrabbedProperty" },
        { PropertyId.DropTargetDropTargetEffectProperty, "DropTargetDropTargetEffectProperty" },
        { PropertyId.DropTargetDropTargetEffectsProperty, "DropTargetDropTargetEffectsProperty" },
        { PropertyId.ExpandCollapseExpandCollapseStateProperty, "ExpandCollapseExpandCollapseStateProperty" },
        { PropertyId.FillColorProperty, "FillColorProperty" },
        { PropertyId.FillTypeProperty, "FillTypeProperty" },
        { PropertyId.FlowsFromProperty, "FlowsFromProperty" },
        { PropertyId.FlowsToProperty, "FlowsToProperty" },
        { PropertyId.FrameworkIdProperty, "FrameworkIdProperty" },
        { PropertyId.FullDescriptionProperty, "FullDescriptionProperty" },
        { PropertyId.GridColumnCountProperty, "GridColumnCountProperty" },
        { PropertyId.GridItemColumnProperty, "GridItemColumnProperty" },
        { PropertyId.GridItemColumnSpanProperty, "GridItemColumnSpanProperty" },
        { PropertyId.GridItemContainingGridProperty, "GridItemContainingGridProperty" },
        { PropertyId.GridItemRowProperty, "GridItemRowProperty" },
        { PropertyId.GridItemRowSpanProperty, "GridItemRowSpanProperty" },
        { PropertyId.GridRowCountProperty, "GridRowCountProperty" },
        { PropertyId.HasKeyboardFocusProperty, "HasKeyboardFocusProperty" },
        { PropertyId.HelpTextProperty, "HelpTextProperty" },
        { PropertyId.IsAnnotationPatternAvailableProperty, "IsAnnotationPatternAvailableProperty" },
        { PropertyId.IsContentElementProperty, "IsContentElementProperty" },
        { PropertyId.IsControlElementProperty, "IsControlElementProperty" },
        { PropertyId.IsCustomNavigationPatternAvailableProperty, "IsCustomNavigationPatternAvailableProperty" },
        { PropertyId.IsDataValidForFormProperty, "IsDataValidForFormProperty" },
        { PropertyId.IsDockPatternAvailableProperty, "IsDockPatternAvailableProperty" },
        { PropertyId.IsDragPatternAvailableProperty, "IsDragPatternAvailableProperty" },
        { PropertyId.IsDropTargetPatternAvailableProperty, "IsDropTargetPatternAvailableProperty" },
        { PropertyId.IsEnabledProperty, "IsEnabledProperty" },
        { PropertyId.IsExpandCollapsePatternAvailableProperty, "IsExpandCollapsePatternAvailableProperty" },
        { PropertyId.IsGridItemPatternAvailableProperty, "IsGridItemPatternAvailableProperty" },
        { PropertyId.IsGridPatternAvailableProperty, "IsGridPatternAvailableProperty" },
        { PropertyId.IsInvokePatternAvailableProperty, "IsInvokePatternAvailableProperty" },
        { PropertyId.IsItemContainerPatternAvailableProperty, "IsItemContainerPatternAvailableProperty" },
        { PropertyId.IsKeyboardFocusableProperty, "IsKeyboardFocusableProperty" },
        { PropertyId.IsLegacyIAccessiblePatternAvailableProperty, "IsLegacyIAccessiblePatternAvailableProperty" },
        { PropertyId.IsMultipleViewPatternAvailableProperty, "IsMultipleViewPatternAvailableProperty" },
        { PropertyId.IsObjectModelPatternAvailableProperty, "IsObjectModelPatternAvailableProperty" },
        { PropertyId.IsOffscreenProperty, "IsOffscreenProperty" },
        { PropertyId.IsPasswordProperty, "IsPasswordProperty" },
        { PropertyId.IsPeripheralProperty, "IsPeripheralProperty" },
        { PropertyId.IsRangeValuePatternAvailableProperty, "IsRangeValuePatternAvailableProperty" },
        { PropertyId.IsRequiredForFormProperty, "IsRequiredForFormProperty" },
        { PropertyId.IsScrollItemPatternAvailableProperty, "IsScrollItemPatternAvailableProperty" },
        { PropertyId.IsScrollPatternAvailableProperty, "IsScrollPatternAvailableProperty" },
        { PropertyId.IsSelectionItemPatternAvailableProperty, "IsSelectionItemPatternAvailableProperty" },
        { PropertyId.IsSelectionPattern2AvailableProperty, "IsSelectionPattern2AvailableProperty" },
        { PropertyId.IsSelectionPatternAvailableProperty, "IsSelectionPatternAvailableProperty" },
        { PropertyId.IsSpreadsheetItemPatternAvailableProperty, "IsSpreadsheetItemPatternAvailableProperty" },
        { PropertyId.IsSpreadsheetPatternAvailableProperty, "IsSpreadsheetPatternAvailableProperty" },
        { PropertyId.IsStylesPatternAvailableProperty, "IsStylesPatternAvailableProperty" },
        { PropertyId.IsSynchronizedInputPatternAvailableProperty, "IsSynchronizedInputPatternAvailableProperty" },
        { PropertyId.IsTableItemPatternAvailableProperty, "IsTableItemPatternAvailableProperty" },
        { PropertyId.IsTablePatternAvailableProperty, "IsTablePatternAvailableProperty" },
        { PropertyId.IsTextChildPatternAvailableProperty, "IsTextChildPatternAvailableProperty" },
        { PropertyId.IsTextEditPatternAvailableProperty, "IsTextEditPatternAvailableProperty" },
        { PropertyId.IsTextPattern2AvailableProperty, "IsTextPattern2AvailableProperty" },
        { PropertyId.IsTextPatternAvailableProperty, "IsTextPatternAvailableProperty" },
        { PropertyId.IsTogglePatternAvailableProperty, "IsTogglePatternAvailableProperty" },
        { PropertyId.IsTransformPattern2AvailableProperty, "IsTransformPattern2AvailableProperty" },
        { PropertyId.IsTransformPatternAvailableProperty, "IsTransformPatternAvailableProperty" },
        { PropertyId.IsValuePatternAvailableProperty, "IsValuePatternAvailableProperty" },
        { PropertyId.IsVirtualizedItemPatternAvailableProperty, "IsVirtualizedItemPatternAvailableProperty" },
        { PropertyId.IsWindowPatternAvailableProperty, "IsWindowPatternAvailableProperty" },
        { PropertyId.ItemStatusProperty, "ItemStatusProperty" },
        { PropertyId.ItemTypeProperty, "ItemTypeProperty" },
        { PropertyId.LabeledByProperty, "LabeledByProperty" },
        { PropertyId.LandmarkTypeProperty, "LandmarkTypeProperty" },
        { PropertyId.LegacyIAccessibleChildIdProperty, "LegacyIAccessibleChildIdProperty" },
        { PropertyId.LegacyIAccessibleDefaultActionProperty, "LegacyIAccessibleDefaultActionProperty" },
        { PropertyId.LegacyIAccessibleDescriptionProperty, "LegacyIAccessibleDescriptionProperty" },
        { PropertyId.LegacyIAccessibleHelpProperty, "LegacyIAccessibleHelpProperty" },
        { PropertyId.LegacyIAccessibleKeyboardShortcutProperty, "LegacyIAccessibleKeyboardShortcutProperty" },
        { PropertyId.LegacyIAccessibleNameProperty, "LegacyIAccessibleNameProperty" },
        { PropertyId.LegacyIAccessibleRoleProperty, "LegacyIAccessibleRoleProperty" },
        { PropertyId.LegacyIAccessibleSelectionProperty, "LegacyIAccessibleSelectionProperty" },
        { PropertyId.LegacyIAccessibleStateProperty, "LegacyIAccessibleStateProperty" },
        { PropertyId.LegacyIAccessibleValueProperty, "LegacyIAccessibleValueProperty" },
        { PropertyId.LevelProperty, "LevelProperty" },
        { PropertyId.LiveSettingProperty, "LiveSettingProperty" },
        { PropertyId.LocalizedControlTypeProperty, "LocalizedControlTypeProperty" },
        { PropertyId.LocalizedLandmarkTypeProperty, "LocalizedLandmarkTypeProperty" },
        { PropertyId.MultipleViewCurrentViewProperty, "MultipleViewCurrentViewProperty" },
        { PropertyId.MultipleViewSupportedViewsProperty, "MultipleViewSupportedViewsProperty" },
        { PropertyId.NameProperty, "NameProperty" },
        { PropertyId.NativeWindowHandleProperty, "NativeWindowHandleProperty" },
        { PropertyId.OptimizeForVisualContentProperty, "OptimizeForVisualContentProperty" },
        { PropertyId.OrientationProperty, "OrientationProperty" },
        { PropertyId.OutlineColorProperty, "OutlineColorProperty" },
        { PropertyId.OutlineThicknessProperty, "OutlineThicknessProperty" },
        { PropertyId.PositionInSetProperty, "PositionInSetProperty" },
        { PropertyId.ProcessIdProperty, "ProcessIdProperty" },
        { PropertyId.ProviderDescriptionProperty, "ProviderDescriptionProperty" },
        { PropertyId.RangeValueIsReadOnlyProperty, "RangeValueIsReadOnlyProperty" },
        { PropertyId.RangeValueLargeChangeProperty, "RangeValueLargeChangeProperty" },
        { PropertyId.RangeValueMaximumProperty, "RangeValueMaximumProperty" },
        { PropertyId.RangeValueMinimumProperty, "RangeValueMinimumProperty" },
        { PropertyId.RangeValueSmallChangeProperty, "RangeValueSmallChangeProperty" },
        { PropertyId.RangeValueValueProperty, "RangeValueValueProperty" },
        { PropertyId.RotationProperty, "RotationProperty" },
        { PropertyId.RuntimeIdProperty, "RuntimeIdProperty" },
        { PropertyId.ScrollHorizontalScrollPercentProperty, "ScrollHorizontalScrollPercentProperty" },
        { PropertyId.ScrollHorizontalViewSizeProperty, "ScrollHorizontalViewSizeProperty" },
        { PropertyId.ScrollHorizontallyScrollableProperty, "ScrollHorizontallyScrollableProperty" },
        { PropertyId.ScrollVerticalScrollPercentProperty, "ScrollVerticalScrollPercentProperty" },
        { PropertyId.ScrollVerticalViewSizeProperty, "ScrollVerticalViewSizeProperty" },
        { PropertyId.ScrollVerticallyScrollableProperty, "ScrollVerticallyScrollableProperty" },
        { PropertyId.Selection2CurrentSelectedItemProperty, "Selection2CurrentSelectedItemProperty" },
        { PropertyId.Selection2FirstSelectedItemProperty, "Selection2FirstSelectedItemProperty" },
        { PropertyId.Selection2ItemCountProperty, "Selection2ItemCountProperty" },
        { PropertyId.Selection2LastSelectedItemProperty, "Selection2LastSelectedItemProperty" },
        { PropertyId.SelectionCanSelectMultipleProperty, "SelectionCanSelectMultipleProperty" },
        { PropertyId.SelectionIsSelectionRequiredProperty, "SelectionIsSelectionRequiredProperty" },
        { PropertyId.SelectionItemIsSelectedProperty, "SelectionItemIsSelectedProperty" },
        { PropertyId.SelectionItemSelectionContainerProperty, "SelectionItemSelectionContainerProperty" },
        { PropertyId.SelectionSelectionProperty, "SelectionSelectionProperty" },
        { PropertyId.SizeOfSetProperty, "SizeOfSetProperty" },
        { PropertyId.SizeProperty, "SizeProperty" },
        { PropertyId.SpreadsheetItemAnnotationObjectsProperty, "SpreadsheetItemAnnotationObjectsProperty" },
        { PropertyId.SpreadsheetItemAnnotationTypesProperty, "SpreadsheetItemAnnotationTypesProperty" },
        { PropertyId.SpreadsheetItemFormulaProperty, "SpreadsheetItemFormulaProperty" },
        { PropertyId.StylesExtendedPropertiesProperty, "StylesExtendedPropertiesProperty" },
        { PropertyId.StylesFillColorProperty, "StylesFillColorProperty" },
        { PropertyId.StylesFillPatternColorProperty, "StylesFillPatternColorProperty" },
        { PropertyId.StylesFillPatternStyleProperty, "StylesFillPatternStyleProperty" },
        { PropertyId.StylesShapeProperty, "StylesShapeProperty" },
        { PropertyId.StylesStyleIdProperty, "StylesStyleIdProperty" },
        { PropertyId.StylesStyleNameProperty, "StylesStyleNameProperty" },
        { PropertyId.TableColumnHeadersProperty, "TableColumnHeadersProperty" },
        { PropertyId.TableItemColumnHeaderItemsProperty, "TableItemColumnHeaderItemsProperty" },
        { PropertyId.TableItemRowHeaderItemsProperty, "TableItemRowHeaderItemsProperty" },
        { PropertyId.TableRowHeadersProperty, "TableRowHeadersProperty" },
        { PropertyId.TableRowOrColumnMajorProperty, "TableRowOrColumnMajorProperty" },
        { PropertyId.ToggleToggleStateProperty, "ToggleToggleStateProperty" },
        { PropertyId.Transform2CanZoomProperty, "Transform2CanZoomProperty" },
        { PropertyId.Transform2ZoomLevelProperty, "Transform2ZoomLevelProperty" },
        { PropertyId.Transform2ZoomMaximumProperty, "Transform2ZoomMaximumProperty" },
        { PropertyId.Transform2ZoomMinimumProperty, "Transform2ZoomMinimumProperty" },
        { PropertyId.TransformCanMoveProperty, "TransformCanMoveProperty" },
        { PropertyId.TransformCanResizeProperty, "TransformCanResizeProperty" },
        { PropertyId.TransformCanRotateProperty, "TransformCanRotateProperty" },
        { PropertyId.ValueIsReadOnlyProperty, "ValueIsReadOnlyProperty" },
        { PropertyId.ValueValueProperty, "ValueValueProperty" },
        { PropertyId.VisualEffectsProperty, "VisualEffectsProperty" },
        { PropertyId.WindowCanMaximizeProperty, "WindowCanMaximizeProperty" },
        { PropertyId.WindowCanMinimizeProperty, "WindowCanMinimizeProperty" },
        { PropertyId.WindowIsModalProperty, "WindowIsModalProperty" },
        { PropertyId.WindowIsTopmostProperty, "WindowIsTopmostProperty" },
        { PropertyId.WindowWindowInteractionStateProperty, "WindowWindowInteractionStateProperty" },
        { PropertyId.WindowWindowVisualStateProperty, "WindowWindowVisualStateProperty" },
    };
}

/// <summary>
/// AccessibleRole from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.accessiblerole
/// </summary>
public static class AccessibleRole
{
    public const int TitleBar = 0x1;
    public const int MenuBar = 0x2;
    public const int ScrollBar = 0x3;
    public const int Grip = 0x4;
    public const int Sound = 0x5;
    public const int Cursor = 0x6;
    public const int Caret = 0x7;
    public const int Alert = 0x8;
    public const int Window = 0x9;
    public const int Client = 0xA;
    public const int MenuPopup = 0xB;
    public const int MenuItem = 0xC;
    public const int ToolTip = 0xD;
    public const int Application = 0xE;
    public const int Document = 0xF;
    public const int Pane = 0x10;
    public const int Chart = 0x11;
    public const int Dialog = 0x12;
    public const int Border = 0x13;
    public const int Grouping = 0x14;
    public const int Separator = 0x15;
    public const int Toolbar = 0x16;
    public const int StatusBar = 0x17;
    public const int Table = 0x18;
    public const int ColumnHeader = 0x19;
    public const int RowHeader = 0x1A;
    public const int Column = 0x1B;
    public const int Row = 0x1C;
    public const int Cell = 0x1D;
    public const int Link = 0x1E;
    public const int HelpBalloon = 0x1F;
    public const int Character = 0x20;
    public const int List = 0x21;
    public const int ListItem = 0x22;
    public const int Outline = 0x23;
    public const int OutlineItem = 0x24;
    public const int PageTab = 0x25;
    public const int PropertyPage = 0x26;
    public const int Indicator = 0x27;
    public const int Graphic = 0x28;
    public const int StaticText = 0x29;
    public const int Text = 0x2A;
    public const int PushButton = 0x2B;
    public const int CheckButton = 0x2C;
    public const int RadioButton = 0x2D;
    public const int ComboBox = 0x2E;
    public const int DropList = 0x2F;
    public const int ProgressBar = 0x30;
    public const int Dial = 0x31;
    public const int HotkeyField = 0x32;
    public const int Slider = 0x33;
    public const int SpinButton = 0x34;
    public const int Diagram = 0x35;
    public const int Animation = 0x36;
    public const int Equation = 0x37;
    public const int ButtonDropDown = 0x38;
    public const int ButtonMenu = 0x39;
    public const int ButtonDropDownGrid = 0x3A;
    public const int WhiteSpace = 0x3B;
    public const int PageTabList = 0x3C;
    public const int Clock = 0x3D;
    public const int SplitButton = 0x3E;
    public const int IpAddress = 0x3F;
    public const int OutlineButton = 0x40;
}

public static class AccessibleRoleNames
{
    public static readonly Dictionary<int, string> Names = new()
    {
        { AccessibleRole.TitleBar, "TitleBar" },
        { AccessibleRole.MenuBar, "MenuBar" },
        { AccessibleRole.ScrollBar, "ScrollBar" },
        { AccessibleRole.Grip, "Grip" },
        { AccessibleRole.Sound, "Sound" },
        { AccessibleRole.Cursor, "Cursor" },
        { AccessibleRole.Caret, "Caret" },
        { AccessibleRole.Alert, "Alert" },
        { AccessibleRole.Window, "Window" },
        { AccessibleRole.Client, "Client" },
        { AccessibleRole.MenuPopup, "MenuPopup" },
        { AccessibleRole.MenuItem, "MenuItem" },
        { AccessibleRole.ToolTip, "ToolTip" },
        { AccessibleRole.Application, "Application" },
        { AccessibleRole.Document, "Document" },
        { AccessibleRole.Pane, "Pane" },
        { AccessibleRole.Chart, "Chart" },
        { AccessibleRole.Dialog, "Dialog" },
        { AccessibleRole.Border, "Border" },
        { AccessibleRole.Grouping, "Grouping" },
        { AccessibleRole.Separator, "Separator" },
        { AccessibleRole.Toolbar, "Toolbar" },
        { AccessibleRole.StatusBar, "StatusBar" },
        { AccessibleRole.Table, "Table" },
        { AccessibleRole.ColumnHeader, "ColumnHeader" },
        { AccessibleRole.RowHeader, "RowHeader" },
        { AccessibleRole.Column, "Column" },
        { AccessibleRole.Row, "Row" },
        { AccessibleRole.Cell, "Cell" },
        { AccessibleRole.Link, "Link" },
        { AccessibleRole.HelpBalloon, "HelpBalloon" },
        { AccessibleRole.Character, "Character" },
        { AccessibleRole.List, "List" },
        { AccessibleRole.ListItem, "ListItem" },
        { AccessibleRole.Outline, "Outline" },
        { AccessibleRole.OutlineItem, "OutlineItem" },
        { AccessibleRole.PageTab, "PageTab" },
        { AccessibleRole.PropertyPage, "PropertyPage" },
        { AccessibleRole.Indicator, "Indicator" },
        { AccessibleRole.Graphic, "Graphic" },
        { AccessibleRole.StaticText, "StaticText" },
        { AccessibleRole.Text, "Text" },
        { AccessibleRole.PushButton, "PushButton" },
        { AccessibleRole.CheckButton, "CheckButton" },
        { AccessibleRole.RadioButton, "RadioButton" },
        { AccessibleRole.ComboBox, "ComboBox" },
        { AccessibleRole.DropList, "DropList" },
        { AccessibleRole.ProgressBar, "ProgressBar" },
        { AccessibleRole.Dial, "Dial" },
        { AccessibleRole.HotkeyField, "HotkeyField" },
        { AccessibleRole.Slider, "Slider" },
        { AccessibleRole.SpinButton, "SpinButton" },
        { AccessibleRole.Diagram, "Diagram" },
        { AccessibleRole.Animation, "Animation" },
        { AccessibleRole.Equation, "Equation" },
        { AccessibleRole.ButtonDropDown, "ButtonDropDown" },
        { AccessibleRole.ButtonMenu, "ButtonMenu" },
        { AccessibleRole.ButtonDropDownGrid, "ButtonDropDownGrid" },
        { AccessibleRole.WhiteSpace, "WhiteSpace" },
        { AccessibleRole.PageTabList, "PageTabList" },
        { AccessibleRole.Clock, "Clock" },
        { AccessibleRole.SplitButton, "SplitButton" },
        { AccessibleRole.IpAddress, "IpAddress" },
        { AccessibleRole.OutlineButton, "OutlineButton" },
    };
}

/// <summary>
/// AccessibleState from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.accessiblestates
/// </summary>
public static class AccessibleState
{
    public const int Normal = 0;
    public const int Unavailable = 0x1;
    public const int Selected = 0x2;
    public const int Focused = 0x4;
    public const int Pressed = 0x8;
    public const int Checked = 0x10;
    public const int Mixed = 0x20;
    public const int Indeterminate = 0x20;
    public const int ReadOnly = 0x40;
    public const int HotTracked = 0x80;
    public const int Default = 0x100;
    public const int Expanded = 0x200;
    public const int Collapsed = 0x400;
    public const int Busy = 0x800;
    public const int Floating = 0x1000;
    public const int Marqueed = 0x2000;
    public const int Animated = 0x4000;
    public const int Invisible = 0x8000;
    public const int Offscreen = 0x10000;
    public const int Sizeable = 0x20000;
    public const int Moveable = 0x40000;
    public const int SelfVoicing = 0x80000;
    public const int Focusable = 0x100000;
    public const int Selectable = 0x200000;
    public const int Linked = 0x400000;
    public const int Traversed = 0x800000;
    public const int MultiSelectable = 0x1000000;
    public const int ExtSelectable = 0x2000000;
    public const int AlertLow = 0x4000000;
    public const int AlertMedium = 0x8000000;
    public const int AlertHigh = 0x10000000;
    public const int Protected = 0x20000000;
    public const int Valid = 0x7FFFFFFF;
    public const int HasPopup = 0x40000000;
}

/// <summary>
/// AccessibleSelection from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.accessibleselection
/// </summary>
public static class AccessibleSelection
{
    public const int None = 0;
    public const int TakeFocus = 0x1;
    public const int TakeSelection = 0x2;
    public const int ExtendSelection = 0x4;
    public const int AddSelection = 0x8;
    public const int RemoveSelection = 0x10;
}

/// <summary>
/// AnnotationType from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-annotation-type-identifiers
/// </summary>
public static class AnnotationType
{
    public const int AdvancedProofingIssue = 60020;
    public const int Author = 60019;
    public const int CircularReferenceError = 60022;
    public const int Comment = 60003;
    public const int ConflictingChange = 60018;
    public const int DataValidationError = 60021;
    public const int DeletionChange = 60012;
    public const int EditingLockedChange = 60016;
    public const int Endnote = 60009;
    public const int ExternalChange = 60017;
    public const int Footer = 60007;
    public const int Footnote = 60010;
    public const int FormatChange = 60014;
    public const int FormulaError = 60004;
    public const int GrammarError = 60002;
    public const int Header = 60006;
    public const int Highlighted = 60008;
    public const int InsertionChange = 60011;
    public const int Mathematics = 60023;
    public const int MoveChange = 60013;
    public const int SpellingError = 60001;
    public const int TrackChanges = 60005;
    public const int Unknown = 60000;
    public const int UnsyncedChange = 60015;
}

/// <summary>
/// NavigateDirection from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-navigatedirection
/// </summary>
public static class NavigateDirection
{
    public const int Parent = 0;
    public const int NextSibling = 1;
    public const int PreviousSibling = 2;
    public const int FirstChild = 3;
    public const int LastChild = 4;
}

/// <summary>
/// DockPosition from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-dockposition
/// </summary>
public static class DockPosition
{
    public const int Top = 0;
    public const int Left = 1;
    public const int Bottom = 2;
    public const int Right = 3;
    public const int Fill = 4;
    public const int None = 5;
}

/// <summary>
/// ScrollAmount from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-scrollamount
/// </summary>
public static class ScrollAmount
{
    public const int LargeDecrement = 0;
    public const int SmallDecrement = 1;
    public const int NoAmount = 2;
    public const int LargeIncrement = 3;
    public const int SmallIncrement = 4;
}

/// <summary>
/// StyleId from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-style-identifiers
/// </summary>
public static class StyleId
{
    public const int Custom = 70000;
    public const int Heading1 = 70001;
    public const int Heading2 = 70002;
    public const int Heading3 = 70003;
    public const int Heading4 = 70004;
    public const int Heading5 = 70005;
    public const int Heading6 = 70006;
    public const int Heading7 = 70007;
    public const int Heading8 = 70008;
    public const int Heading9 = 70009;
    public const int Title = 70010;
    public const int Subtitle = 70011;
    public const int Normal = 70012;
    public const int Emphasis = 70013;
    public const int Quote = 70014;
    public const int BulletedList = 70015;
    public const int NumberedList = 70016;
}

/// <summary>
/// RowOrColumnMajor from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-roworcolumnmajor
/// </summary>
public static class RowOrColumnMajor
{
    public const int RowMajor = 0;
    public const int ColumnMajor = 1;
    public const int Indeterminate = 2;
}

/// <summary>
/// ExpandCollapseState from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-expandcollapsestate
/// </summary>
public static class ExpandCollapseState
{
    public const int Collapsed = 0;
    public const int Expanded = 1;
    public const int PartiallyExpanded = 2;
    public const int LeafNode = 3;
}

/// <summary>
/// OrientationType from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-orientationtype
/// </summary>
public static class OrientationType
{
    public const int None = 0;
    public const int Horizontal = 1;
    public const int Vertical = 2;
}

/// <summary>
/// ToggleState from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-togglestate
/// </summary>
public static class ToggleState
{
    public const int Off = 0;
    public const int On = 1;
    public const int Indeterminate = 2;
}

/// <summary>
/// TextPatternRangeEndpoint from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-textpatternrangeendpoint
/// </summary>
public static class TextPatternRangeEndpoint
{
    public const int Start = 0;
    public const int End = 1;
}

/// <summary>
/// TextAttributeId from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/winauto/uiauto-textattribute-ids
/// </summary>
public static class TextAttributeId
{
    public const int AfterParagraphSpacingAttribute = 40042;
    public const int AnimationStyleAttribute = 40000;
    public const int AnnotationObjectsAttribute = 40032;
    public const int AnnotationTypesAttribute = 40031;
    public const int BackgroundColorAttribute = 40001;
    public const int BeforeParagraphSpacingAttribute = 40041;
    public const int BulletStyleAttribute = 40002;
    public const int CapStyleAttribute = 40003;
    public const int CaretBidiModeAttribute = 40039;
    public const int CaretPositionAttribute = 40038;
    public const int CultureAttribute = 40004;
    public const int FontNameAttribute = 40005;
    public const int FontSizeAttribute = 40006;
    public const int FontWeightAttribute = 40007;
    public const int ForegroundColorAttribute = 40008;
    public const int HorizontalTextAlignmentAttribute = 40009;
    public const int IndentationFirstLineAttribute = 40010;
    public const int IndentationLeadingAttribute = 40011;
    public const int IndentationTrailingAttribute = 40012;
    public const int IsActiveAttribute = 40036;
    public const int IsHiddenAttribute = 40013;
    public const int IsItalicAttribute = 40014;
    public const int IsReadOnlyAttribute = 40015;
    public const int IsSubscriptAttribute = 40016;
    public const int IsSuperscriptAttribute = 40017;
    public const int LineSpacingAttribute = 40040;
    public const int LinkAttribute = 40035;
    public const int MarginBottomAttribute = 40018;
    public const int MarginLeadingAttribute = 40019;
    public const int MarginTopAttribute = 40020;
    public const int MarginTrailingAttribute = 40021;
    public const int OutlineStylesAttribute = 40022;
    public const int OverlineColorAttribute = 40023;
    public const int OverlineStyleAttribute = 40024;
    public const int SayAsInterpretAsAttribute = 40043;
    public const int SelectionActiveEndAttribute = 40037;
    public const int StrikethroughColorAttribute = 40025;
    public const int StrikethroughStyleAttribute = 40026;
    public const int StyleIdAttribute = 40034;
    public const int StyleNameAttribute = 40033;
    public const int TabsAttribute = 40027;
    public const int TextFlowDirectionsAttribute = 40028;
    public const int UnderlineColorAttribute = 40029;
    public const int UnderlineStyleAttribute = 40030;
}

/// <summary>
/// TextUnit from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-textunit
/// </summary>
public static class TextUnit
{
    public const int Character = 0;
    public const int Format = 1;
    public const int Word = 2;
    public const int Line = 3;
    public const int Paragraph = 4;
    public const int Page = 5;
    public const int Document = 6;
}

/// <summary>
/// ZoomUnit from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-zoomunit
/// </summary>
public static class ZoomUnit
{
    public const int NoAmount = 0;
    public const int LargeDecrement = 1;
    public const int SmallDecrement = 2;
    public const int LargeIncrement = 3;
    public const int SmallIncrement = 4;
}

/// <summary>
/// WindowInteractionState from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-windowinteractionstate
/// </summary>
public static class WindowInteractionState
{
    public const int Running = 0;
    public const int Closing = 1;
    public const int ReadyForUserInteraction = 2;
    public const int BlockedByModalWindow = 3;
    public const int NotResponding = 4;
}

/// <summary>
/// WindowVisualState from IUIAutomation.
/// Refer https://docs.microsoft.com/en-us/windows/win32/api/uiautomationcore/ne-uiautomationcore-windowvisualstate
/// </summary>
public static class WindowVisualState
{
    public const int Normal = 0;
    public const int Maximized = 1;
    public const int Minimized = 2;
}

/// <summary>ConsoleColor from Win32.</summary>
public static class ConsoleColor
{
    public const int Default = -1;
    public const int Black = 0;
    public const int DarkBlue = 1;
    public const int DarkGreen = 2;
    public const int DarkCyan = 3;
    public const int DarkRed = 4;
    public const int DarkMagenta = 5;
    public const int DarkYellow = 6;
    public const int Gray = 7;
    public const int DarkGray = 8;
    public const int Blue = 9;
    public const int Green = 10;
    public const int Cyan = 11;
    public const int Red = 12;
    public const int Magenta = 13;
    public const int Yellow = 14;
    public const int White = 15;
}

/// <summary>GAFlag from Win32.</summary>
public static class GAFlag
{
    public const int Parent = 1;
    public const int Root = 2;
    public const int RootOwner = 3;
}

/// <summary>MouseEventFlag from Win32.</summary>
public static class MouseEventFlag
{
    public const int Move = 0x0001;
    public const int LeftDown = 0x0002;
    public const int LeftUp = 0x0004;
    public const int RightDown = 0x0008;
    public const int RightUp = 0x0010;
    public const int MiddleDown = 0x0020;
    public const int MiddleUp = 0x0040;
    public const int XDown = 0x0080;
    public const int XUp = 0x0100;
    public const int Wheel = 0x0800;
    public const int HWheel = 0x1000;
    public const int MoveNoCoalesce = 0x2000;
    public const int VirtualDesk = 0x4000;
    public const int Absolute = 0x8000;
}

/// <summary>KeyboardEventFlag from Win32.</summary>
public static class KeyboardEventFlag
{
    public const int KeyDown = 0x0000;
    public const int ExtendedKey = 0x0001;
    public const int KeyUp = 0x0002;
    public const int KeyUnicode = 0x0004;
    public const int KeyScanCode = 0x0008;
}

/// <summary>InputType from Win32.</summary>
public static class InputType
{
    public const int Mouse = 0;
    public const int Keyboard = 1;
    public const int Hardware = 2;
}

/// <summary>ModifierKey from Win32.</summary>
public static class ModifierKey
{
    public const int Alt = 0x0001;
    public const int Control = 0x0002;
    public const int Shift = 0x0004;
    public const int Win = 0x0008;
    public const int NoRepeat = 0x4000;
}

/// <summary>ShowWindow params from Win32.</summary>
public static class SW
{
    public const int Hide = 0;
    public const int ShowNormal = 1;
    public const int Normal = 1;
    public const int ShowMinimized = 2;
    public const int ShowMaximized = 3;
    public const int Maximize = 3;
    public const int ShowNoActivate = 4;
    public const int Show = 5;
    public const int Minimize = 6;
    public const int ShowMinNoActive = 7;
    public const int ShowNA = 8;
    public const int Restore = 9;
    public const int ShowDefault = 10;
    public const int ForceMinimize = 11;
    public const int Max = 11;
}

/// <summary>SetWindowPos params from Win32.</summary>
public static class SWP
{
    public const int HWND_Top = 0;
    public const int HWND_Bottom = 1;
    public const int HWND_Topmost = -1;
    public const int HWND_NoTopmost = -2;
    public const int SWP_NoSize = 0x0001;
    public const int SWP_NoMove = 0x0002;
    public const int SWP_NoZOrder = 0x0004;
    public const int SWP_NoRedraw = 0x0008;
    public const int SWP_NoActivate = 0x0010;
    public const int SWP_FrameChanged = 0x0020;
    public const int SWP_ShowWindow = 0x0040;
    public const int SWP_HideWindow = 0x0080;
    public const int SWP_NoCopyBits = 0x0100;
    public const int SWP_NoOwnerZOrder = 0x0200;
    public const int SWP_NoSendChanging = 0x0400;
    public const int SWP_DrawFrame = SWP_FrameChanged;
    public const int SWP_NoReposition = SWP_NoOwnerZOrder;
    public const int SWP_DeferErase = 0x2000;
    public const int SWP_AsyncWindowPos = 0x4000;
}

/// <summary>MessageBox flags from Win32.</summary>
public static class MB
{
    public const int Ok = 0x00000000;
    public const int OkCancel = 0x00000001;
    public const int AbortRetryIgnore = 0x00000002;
    public const int YesNoCancel = 0x00000003;
    public const int YesNo = 0x00000004;
    public const int RetryCancel = 0x00000005;
    public const int CancelTryContinue = 0x00000006;
    public const int IconHand = 0x00000010;
    public const int IconQuestion = 0x00000020;
    public const int IconExclamation = 0x00000030;
    public const int IconAsterisk = 0x00000040;
    public const int UserIcon = 0x00000080;
    public const int IconWarning = 0x00000030;
    public const int IconError = 0x00000010;
    public const int IconInformation = 0x00000040;
    public const int IconStop = 0x00000010;
    public const int DefButton1 = 0x00000000;
    public const int DefButton2 = 0x00000100;
    public const int DefButton3 = 0x00000200;
    public const int DefButton4 = 0x00000300;
    public const int ApplModal = 0x00000000;
    public const int SystemModal = 0x00001000;
    public const int TaskModal = 0x00002000;
    public const int Help = 0x00004000;
    public const int NoFocus = 0x00008000;
    public const int SetForeground = 0x00010000;
    public const int DefaultDesktopOnly = 0x00020000;
    public const int Topmost = 0x00040000;
    public const int Right = 0x00080000;
    public const int RtlReading = 0x00100000;
    public const int ServiceNotification = 0x00200000;
    public const int ServiceNotificationNT3X = 0x00040000;

    public const int TypeMask = 0x0000000F;
    public const int IconMask = 0x000000F0;
    public const int DefMask = 0x00000F00;
    public const int ModeMask = 0x00003000;
    public const int MiscMask = 0x0000C000;

    public const int IdOk = 1;
    public const int IdCancel = 2;
    public const int IdAbort = 3;
    public const int IdRetry = 4;
    public const int IdIgnore = 5;
    public const int IdYes = 6;
    public const int IdNo = 7;
    public const int IdClose = 8;
    public const int IdHelp = 9;
    public const int IdTryAgain = 10;
    public const int IdContinue = 11;
    public const int IdTimeout = 32000;
}

public static class GWL
{
    public const int ExStyle = -20;
    public const int HInstance = -6;
    public const int HwndParent = -8;
    public const int ID = -12;
    public const int Style = -16;
    public const int UserData = -21;
    public const int WndProc = -4;
}

public static class ProcessDpiAwareness
{
    public const int DpiUnaware = 0;
    public const int SystemDpiAware = 1;
    public const int PerMonitorDpiAware = 2;
}

public static class DpiAwarenessContext
{
    public const int Unaware = -1;
    public const int SystemAware = -2;
    public const int PerMonitorAware = -3;
    public const int PerMonitorAwareV2 = -4;
    public const int UnawareGdiScaled = -5;
}

/// <summary>Key codes from Win32.</summary>
public static class Keys
{
    public const int VK_LBUTTON = 0x01;
    public const int VK_RBUTTON = 0x02;
    public const int VK_CANCEL = 0x03;
    public const int VK_MBUTTON = 0x04;
    public const int VK_XBUTTON1 = 0x05;
    public const int VK_XBUTTON2 = 0x06;
    public const int VK_BACK = 0x08;
    public const int VK_TAB = 0x09;
    public const int VK_CLEAR = 0x0C;
    public const int VK_RETURN = 0x0D;
    public const int VK_ENTER = 0x0D;
    public const int VK_SHIFT = 0x10;
    public const int VK_CONTROL = 0x11;
    public const int VK_MENU = 0x12;
    public const int VK_PAUSE = 0x13;
    public const int VK_CAPITAL = 0x14;
    public const int VK_KANA = 0x15;
    public const int VK_HANGUEL = 0x15;
    public const int VK_HANGUL = 0x15;
    public const int VK_JUNJA = 0x17;
    public const int VK_FINAL = 0x18;
    public const int VK_HANJA = 0x19;
    public const int VK_KANJI = 0x19;
    public const int VK_ESCAPE = 0x1B;
    public const int VK_CONVERT = 0x1C;
    public const int VK_NONCONVERT = 0x1D;
    public const int VK_ACCEPT = 0x1E;
    public const int VK_MODECHANGE = 0x1F;
    public const int VK_SPACE = 0x20;
    public const int VK_PRIOR = 0x21;
    public const int VK_PAGEUP = 0x21;
    public const int VK_NEXT = 0x22;
    public const int VK_PAGEDOWN = 0x22;
    public const int VK_END = 0x23;
    public const int VK_HOME = 0x24;
    public const int VK_LEFT = 0x25;
    public const int VK_UP = 0x26;
    public const int VK_RIGHT = 0x27;
    public const int VK_DOWN = 0x28;
    public const int VK_SELECT = 0x29;
    public const int VK_PRINT = 0x2A;
    public const int VK_EXECUTE = 0x2B;
    public const int VK_SNAPSHOT = 0x2C;
    public const int VK_INSERT = 0x2D;
    public const int VK_DELETE = 0x2E;
    public const int VK_HELP = 0x2F;
    public const int VK_0 = 0x30;
    public const int VK_1 = 0x31;
    public const int VK_2 = 0x32;
    public const int VK_3 = 0x33;
    public const int VK_4 = 0x34;
    public const int VK_5 = 0x35;
    public const int VK_6 = 0x36;
    public const int VK_7 = 0x37;
    public const int VK_8 = 0x38;
    public const int VK_9 = 0x39;
    public const int VK_A = 0x41;
    public const int VK_B = 0x42;
    public const int VK_C = 0x43;
    public const int VK_D = 0x44;
    public const int VK_E = 0x45;
    public const int VK_F = 0x46;
    public const int VK_G = 0x47;
    public const int VK_H = 0x48;
    public const int VK_I = 0x49;
    public const int VK_J = 0x4A;
    public const int VK_K = 0x4B;
    public const int VK_L = 0x4C;
    public const int VK_M = 0x4D;
    public const int VK_N = 0x4E;
    public const int VK_O = 0x4F;
    public const int VK_P = 0x50;
    public const int VK_Q = 0x51;
    public const int VK_R = 0x52;
    public const int VK_S = 0x53;
    public const int VK_T = 0x54;
    public const int VK_U = 0x55;
    public const int VK_V = 0x56;
    public const int VK_W = 0x57;
    public const int VK_X = 0x58;
    public const int VK_Y = 0x59;
    public const int VK_Z = 0x5A;
    public const int VK_LWIN = 0x5B;
    public const int VK_RWIN = 0x5C;
    public const int VK_APPS = 0x5D;
    public const int VK_SLEEP = 0x5F;
    public const int VK_NUMPAD0 = 0x60;
    public const int VK_NUMPAD1 = 0x61;
    public const int VK_NUMPAD2 = 0x62;
    public const int VK_NUMPAD3 = 0x63;
    public const int VK_NUMPAD4 = 0x64;
    public const int VK_NUMPAD5 = 0x65;
    public const int VK_NUMPAD6 = 0x66;
    public const int VK_NUMPAD7 = 0x67;
    public const int VK_NUMPAD8 = 0x68;
    public const int VK_NUMPAD9 = 0x69;
    public const int VK_MULTIPLY = 0x6A;
    public const int VK_ADD = 0x6B;
    public const int VK_SEPARATOR = 0x6C;
    public const int VK_SUBTRACT = 0x6D;
    public const int VK_DECIMAL = 0x6E;
    public const int VK_DIVIDE = 0x6F;
    public const int VK_F1 = 0x70;
    public const int VK_F2 = 0x71;
    public const int VK_F3 = 0x72;
    public const int VK_F4 = 0x73;
    public const int VK_F5 = 0x74;
    public const int VK_F6 = 0x75;
    public const int VK_F7 = 0x76;
    public const int VK_F8 = 0x77;
    public const int VK_F9 = 0x78;
    public const int VK_F10 = 0x79;
    public const int VK_F11 = 0x7A;
    public const int VK_F12 = 0x7B;
    public const int VK_F13 = 0x7C;
    public const int VK_F14 = 0x7D;
    public const int VK_F15 = 0x7E;
    public const int VK_F16 = 0x7F;
    public const int VK_F17 = 0x80;
    public const int VK_F18 = 0x81;
    public const int VK_F19 = 0x82;
    public const int VK_F20 = 0x83;
    public const int VK_F21 = 0x84;
    public const int VK_F22 = 0x85;
    public const int VK_F23 = 0x86;
    public const int VK_F24 = 0x87;
    public const int VK_NUMLOCK = 0x90;
    public const int VK_SCROLL = 0x91;
    public const int VK_LSHIFT = 0xA0;
    public const int VK_RSHIFT = 0xA1;
    public const int VK_LCONTROL = 0xA2;
    public const int VK_RCONTROL = 0xA3;
    public const int VK_LMENU = 0xA4;
    public const int VK_RMENU = 0xA5;
    public const int VK_BROWSER_BACK = 0xA6;
    public const int VK_BROWSER_FORWARD = 0xA7;
    public const int VK_BROWSER_REFRESH = 0xA8;
    public const int VK_BROWSER_STOP = 0xA9;
    public const int VK_BROWSER_SEARCH = 0xAA;
    public const int VK_BROWSER_FAVORITES = 0xAB;
    public const int VK_BROWSER_HOME = 0xAC;
    public const int VK_VOLUME_MUTE = 0xAD;
    public const int VK_VOLUME_DOWN = 0xAE;
    public const int VK_VOLUME_UP = 0xAF;
    public const int VK_MEDIA_NEXT_TRACK = 0xB0;
    public const int VK_MEDIA_PREV_TRACK = 0xB1;
    public const int VK_MEDIA_STOP = 0xB2;
    public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
    public const int VK_LAUNCH_MAIL = 0xB4;
    public const int VK_LAUNCH_MEDIA_SELECT = 0xB5;
    public const int VK_LAUNCH_APP1 = 0xB6;
    public const int VK_LAUNCH_APP2 = 0xB7;
    public const int VK_OEM_1 = 0xBA;
    public const int VK_OEM_PLUS = 0xBB;
    public const int VK_OEM_COMMA = 0xBC;
    public const int VK_OEM_MINUS = 0xBD;
    public const int VK_OEM_PERIOD = 0xBE;
    public const int VK_OEM_2 = 0xBF;
    public const int VK_OEM_3 = 0xC0;
    public const int VK_OEM_4 = 0xDB;
    public const int VK_OEM_5 = 0xDC;
    public const int VK_OEM_6 = 0xDD;
    public const int VK_OEM_7 = 0xDE;
    public const int VK_OEM_8 = 0xDF;
    public const int VK_OEM_102 = 0xE2;
    public const int VK_PROCESSKEY = 0xE5;
    public const int VK_PACKET = 0xE7;
    public const int VK_ATTN = 0xF6;
    public const int VK_CRSEL = 0xF7;
    public const int VK_EXSEL = 0xF8;
    public const int VK_EREOF = 0xF9;
    public const int VK_PLAY = 0xFA;
    public const int VK_ZOOM = 0xFB;
    public const int VK_NONAME = 0xFC;
    public const int VK_PA1 = 0xFD;
    public const int VK_OEM_CLEAR = 0xFE;
}

public static class SpecialKeyNames
{
    public static readonly Dictionary<string, int> Names = new()
    {
        { "LBUTTON", Keys.VK_LBUTTON },
        { "RBUTTON", Keys.VK_RBUTTON },
        { "CANCEL", Keys.VK_CANCEL },
        { "MBUTTON", Keys.VK_MBUTTON },
        { "XBUTTON1", Keys.VK_XBUTTON1 },
        { "XBUTTON2", Keys.VK_XBUTTON2 },
        { "BACK", Keys.VK_BACK },
        { "TAB", Keys.VK_TAB },
        { "CLEAR", Keys.VK_CLEAR },
        { "RETURN", Keys.VK_RETURN },
        { "ENTER", Keys.VK_RETURN },
        { "SHIFT", Keys.VK_SHIFT },
        { "CTRL", Keys.VK_CONTROL },
        { "CONTROL", Keys.VK_CONTROL },
        { "ALT", Keys.VK_MENU },
        { "PAUSE", Keys.VK_PAUSE },
        { "CAPITAL", Keys.VK_CAPITAL },
        { "KANA", Keys.VK_KANA },
        { "HANGUEL", Keys.VK_HANGUEL },
        { "HANGUL", Keys.VK_HANGUL },
        { "JUNJA", Keys.VK_JUNJA },
        { "FINAL", Keys.VK_FINAL },
        { "HANJA", Keys.VK_HANJA },
        { "KANJI", Keys.VK_KANJI },
        { "ESC", Keys.VK_ESCAPE },
        { "ESCAPE", Keys.VK_ESCAPE },
        { "CONVERT", Keys.VK_CONVERT },
        { "NONCONVERT", Keys.VK_NONCONVERT },
        { "ACCEPT", Keys.VK_ACCEPT },
        { "MODECHANGE", Keys.VK_MODECHANGE },
        { "SPACE", Keys.VK_SPACE },
        { "PRIOR", Keys.VK_PRIOR },
        { "PAGEUP", Keys.VK_PRIOR },
        { "NEXT", Keys.VK_NEXT },
        { "PAGEDOWN", Keys.VK_NEXT },
        { "END", Keys.VK_END },
        { "HOME", Keys.VK_HOME },
        { "LEFT", Keys.VK_LEFT },
        { "UP", Keys.VK_UP },
        { "RIGHT", Keys.VK_RIGHT },
        { "DOWN", Keys.VK_DOWN },
        { "SELECT", Keys.VK_SELECT },
        { "PRINT", Keys.VK_PRINT },
        { "EXECUTE", Keys.VK_EXECUTE },
        { "SNAPSHOT", Keys.VK_SNAPSHOT },
        { "PRINTSCREEN", Keys.VK_SNAPSHOT },
        { "INSERT", Keys.VK_INSERT },
        { "INS", Keys.VK_INSERT },
        { "DELETE", Keys.VK_DELETE },
        { "DEL", Keys.VK_DELETE },
        { "HELP", Keys.VK_HELP },
        { "WIN", Keys.VK_LWIN },
        { "LWIN", Keys.VK_LWIN },
        { "RWIN", Keys.VK_RWIN },
        { "APPS", Keys.VK_APPS },
        { "SLEEP", Keys.VK_SLEEP },
        { "NUMPAD0", Keys.VK_NUMPAD0 },
        { "NUMPAD1", Keys.VK_NUMPAD1 },
        { "NUMPAD2", Keys.VK_NUMPAD2 },
        { "NUMPAD3", Keys.VK_NUMPAD3 },
        { "NUMPAD4", Keys.VK_NUMPAD4 },
        { "NUMPAD5", Keys.VK_NUMPAD5 },
        { "NUMPAD6", Keys.VK_NUMPAD6 },
        { "NUMPAD7", Keys.VK_NUMPAD7 },
        { "NUMPAD8", Keys.VK_NUMPAD8 },
        { "NUMPAD9", Keys.VK_NUMPAD9 },
        { "MULTIPLY", Keys.VK_MULTIPLY },
        { "ADD", Keys.VK_ADD },
        { "SEPARATOR", Keys.VK_SEPARATOR },
        { "SUBTRACT", Keys.VK_SUBTRACT },
        { "DECIMAL", Keys.VK_DECIMAL },
        { "DIVIDE", Keys.VK_DIVIDE },
        { "F1", Keys.VK_F1 },
        { "F2", Keys.VK_F2 },
        { "F3", Keys.VK_F3 },
        { "F4", Keys.VK_F4 },
        { "F5", Keys.VK_F5 },
        { "F6", Keys.VK_F6 },
        { "F7", Keys.VK_F7 },
        { "F8", Keys.VK_F8 },
        { "F9", Keys.VK_F9 },
        { "F10", Keys.VK_F10 },
        { "F11", Keys.VK_F11 },
        { "F12", Keys.VK_F12 },
        { "F13", Keys.VK_F13 },
        { "F14", Keys.VK_F14 },
        { "F15", Keys.VK_F15 },
        { "F16", Keys.VK_F16 },
        { "F17", Keys.VK_F17 },
        { "F18", Keys.VK_F18 },
        { "F19", Keys.VK_F19 },
        { "F20", Keys.VK_F20 },
        { "F21", Keys.VK_F21 },
        { "F22", Keys.VK_F22 },
        { "F23", Keys.VK_F23 },
        { "F24", Keys.VK_F24 },
        { "NUMLOCK", Keys.VK_NUMLOCK },
        { "SCROLL", Keys.VK_SCROLL },
        { "LSHIFT", Keys.VK_LSHIFT },
        { "RSHIFT", Keys.VK_RSHIFT },
        { "LCONTROL", Keys.VK_LCONTROL },
        { "LCTRL", Keys.VK_LCONTROL },
        { "RCONTROL", Keys.VK_RCONTROL },
        { "RCTRL", Keys.VK_RCONTROL },
        { "LALT", Keys.VK_LMENU },
        { "RALT", Keys.VK_RMENU },
        { "BROWSER_BACK", Keys.VK_BROWSER_BACK },
        { "BROWSER_FORWARD", Keys.VK_BROWSER_FORWARD },
        { "BROWSER_REFRESH", Keys.VK_BROWSER_REFRESH },
        { "BROWSER_STOP", Keys.VK_BROWSER_STOP },
        { "BROWSER_SEARCH", Keys.VK_BROWSER_SEARCH },
        { "BROWSER_FAVORITES", Keys.VK_BROWSER_FAVORITES },
        { "BROWSER_HOME", Keys.VK_BROWSER_HOME },
        { "VOLUME_MUTE", Keys.VK_VOLUME_MUTE },
        { "VOLUME_DOWN", Keys.VK_VOLUME_DOWN },
        { "VOLUME_UP", Keys.VK_VOLUME_UP },
        { "MEDIA_NEXT_TRACK", Keys.VK_MEDIA_NEXT_TRACK },
        { "MEDIA_PREV_TRACK", Keys.VK_MEDIA_PREV_TRACK },
        { "MEDIA_STOP", Keys.VK_MEDIA_STOP },
        { "MEDIA_PLAY_PAUSE", Keys.VK_MEDIA_PLAY_PAUSE },
        { "LAUNCH_MAIL", Keys.VK_LAUNCH_MAIL },
        { "LAUNCH_MEDIA_SELECT", Keys.VK_LAUNCH_MEDIA_SELECT },
        { "LAUNCH_APP1", Keys.VK_LAUNCH_APP1 },
        { "LAUNCH_APP2", Keys.VK_LAUNCH_APP2 },
        { "OEM_1", Keys.VK_OEM_1 },
        { "OEM_PLUS", Keys.VK_OEM_PLUS },
        { "OEM_COMMA", Keys.VK_OEM_COMMA },
        { "OEM_MINUS", Keys.VK_OEM_MINUS },
        { "OEM_PERIOD", Keys.VK_OEM_PERIOD },
        { "OEM_2", Keys.VK_OEM_2 },
        { "OEM_3", Keys.VK_OEM_3 },
        { "OEM_4", Keys.VK_OEM_4 },
        { "OEM_5", Keys.VK_OEM_5 },
        { "OEM_6", Keys.VK_OEM_6 },
        { "OEM_7", Keys.VK_OEM_7 },
        { "OEM_8", Keys.VK_OEM_8 },
        { "OEM_102", Keys.VK_OEM_102 },
        { "PROCESSKEY", Keys.VK_PROCESSKEY },
        { "PACKET", Keys.VK_PACKET },
        { "ATTN", Keys.VK_ATTN },
        { "CRSEL", Keys.VK_CRSEL },
        { "EXSEL", Keys.VK_EXSEL },
        { "EREOF", Keys.VK_EREOF },
        { "PLAY", Keys.VK_PLAY },
        { "ZOOM", Keys.VK_ZOOM },
        { "NONAME", Keys.VK_NONAME },
        { "PA1", Keys.VK_PA1 },
        { "OEM_CLEAR", Keys.VK_OEM_CLEAR },
    };
}

public static class CharacterCodes
{
    public static readonly Dictionary<char, int> Codes = new()
    {
        { '0', Keys.VK_0 },
        { '1', Keys.VK_1 },
        { '2', Keys.VK_2 },
        { '3', Keys.VK_3 },
        { '4', Keys.VK_4 },
        { '5', Keys.VK_5 },
        { '6', Keys.VK_6 },
        { '7', Keys.VK_7 },
        { '8', Keys.VK_8 },
        { '9', Keys.VK_9 },
        { 'a', Keys.VK_A },
        { 'A', Keys.VK_A },
        { 'b', Keys.VK_B },
        { 'B', Keys.VK_B },
        { 'c', Keys.VK_C },
        { 'C', Keys.VK_C },
        { 'd', Keys.VK_D },
        { 'D', Keys.VK_D },
        { 'e', Keys.VK_E },
        { 'E', Keys.VK_E },
        { 'f', Keys.VK_F },
        { 'F', Keys.VK_F },
        { 'g', Keys.VK_G },
        { 'G', Keys.VK_G },
        { 'h', Keys.VK_H },
        { 'H', Keys.VK_H },
        { 'i', Keys.VK_I },
        { 'I', Keys.VK_I },
        { 'j', Keys.VK_J },
        { 'J', Keys.VK_J },
        { 'k', Keys.VK_K },
        { 'K', Keys.VK_K },
        { 'l', Keys.VK_L },
        { 'L', Keys.VK_L },
        { 'm', Keys.VK_M },
        { 'M', Keys.VK_M },
        { 'n', Keys.VK_N },
        { 'N', Keys.VK_N },
        { 'o', Keys.VK_O },
        { 'O', Keys.VK_O },
        { 'p', Keys.VK_P },
        { 'P', Keys.VK_P },
        { 'q', Keys.VK_Q },
        { 'Q', Keys.VK_Q },
        { 'r', Keys.VK_R },
        { 'R', Keys.VK_R },
        { 's', Keys.VK_S },
        { 'S', Keys.VK_S },
        { 't', Keys.VK_T },
        { 'T', Keys.VK_T },
        { 'u', Keys.VK_U },
        { 'U', Keys.VK_U },
        { 'v', Keys.VK_V },
        { 'V', Keys.VK_V },
        { 'w', Keys.VK_W },
        { 'W', Keys.VK_W },
        { 'x', Keys.VK_X },
        { 'X', Keys.VK_X },
        { 'y', Keys.VK_Y },
        { 'Y', Keys.VK_Y },
        { 'z', Keys.VK_Z },
        { 'Z', Keys.VK_Z },
        { ' ', Keys.VK_SPACE },
        { '`', Keys.VK_OEM_3 },
        { '-', Keys.VK_OEM_MINUS },
        { '=', Keys.VK_OEM_PLUS },
        { '[', Keys.VK_OEM_4 },
        { ']', Keys.VK_OEM_6 },
        { '\\', Keys.VK_OEM_5 },
        { ';', Keys.VK_OEM_1 },
        { '\'', Keys.VK_OEM_7 },
        { ',', Keys.VK_OEM_COMMA },
        { '.', Keys.VK_OEM_PERIOD },
        { '/', Keys.VK_OEM_2 },
    };
}

/// <summary>Clipboard format constants from Win32.</summary>
public static class ClipboardFormat
{
    public const int CF_TEXT = 1;
    public const int CF_BITMAP = 2;
    public const int CF_METAFILEPICT = 3;
    public const int CF_SYLK = 4;
    public const int CF_DIF = 5;
    public const int CF_TIFF = 6;
    public const int CF_OEMTEXT = 7;
    public const int CF_DIB = 8;
    public const int CF_PALETTE = 9;
    public const int CF_PENDATA = 10;
    public const int CF_RIFF = 11;
    public const int CF_WAVE = 12;
    public const int CF_UNICODETEXT = 13;
    public const int CF_ENHMETAFILE = 14;
    public const int CF_HDROP = 15;
    public const int CF_LOCALE = 16;
    public const int CF_DIBV5 = 17;
    public const int CF_MAX = 18;
}

public static class ActiveEnd
{
    public const int ActiveEnd_None = 0;
    public const int ActiveEnd_Start = 1;
    public const int ActiveEnd_End = 2;
}

public static class AnimationStyle
{
    public const int AnimationStyle_None = 0;
    public const int AnimationStyle_LasVegasLights = 1;
    public const int AnimationStyle_BlinkingBackground = 2;
    public const int AnimationStyle_SparkleText = 3;
    public const int AnimationStyle_MarchingBlackAnts = 4;
    public const int AnimationStyle_MarchingRedAnts = 5;
    public const int AnimationStyle_Shimmer = 6;
    public const int AnimationStyle_Other = -1;
}

public static class AsyncContentLoadedState
{
    public const int AsyncContentLoadedState_Beginning = 0;
    public const int AsyncContentLoadedState_Progress = 1;
    public const int AsyncContentLoadedState_Completed = 2;
}

public static class AutomationElementMode
{
    public const int AutomationElementMode_None = 0;
    public const int AutomationElementMode_Full = 1;
}

public static class AutomationIdentifierType
{
    public const int AutomationIdentifierType_Property = 0;
    public const int AutomationIdentifierType_Pattern = 1;
    public const int AutomationIdentifierType_Event = 2;
    public const int AutomationIdentifierType_ControlType = 3;
    public const int AutomationIdentifierType_TextAttribute = 4;
    public const int AutomationIdentifierType_LandmarkType = 5;
    public const int AutomationIdentifierType_Annotation = 6;
    public const int AutomationIdentifierType_Changes = 7;
    public const int AutomationIdentifierType_Style = 8;
}

public static class BulletStyle
{
    public const int BulletStyle_None = 0;
    public const int BulletStyle_HollowRoundBullet = 1;
    public const int BulletStyle_FilledRoundBullet = 2;
    public const int BulletStyle_HollowSquareBullet = 3;
    public const int BulletStyle_FilledSquareBullet = 4;
    public const int BulletStyle_DashBullet = 5;
    public const int BulletStyle_Other = -1;
}

public static class CapStyle
{
    public const int CapStyle_None = 0;
    public const int CapStyle_SmallCap = 1;
    public const int CapStyle_AllCap = 2;
    public const int CapStyle_AllPetiteCaps = 3;
    public const int CapStyle_PetiteCaps = 4;
    public const int CapStyle_Unicase = 5;
    public const int CapStyle_Titling = 6;
    public const int CapStyle_Other = -1;
}

public static class CaretBidiMode
{
    public const int CaretBidiMode_LTR = 0;
    public const int CaretBidiMode_RTL = 1;
}

public static class CaretPosition
{
    public const int CaretPosition_Unknown = 0;
    public const int CaretPosition_EndOfLine = 1;
    public const int CaretPosition_BeginningOfLine = 2;
}

public static class CoalesceEventsOptions
{
    public const int CoalesceEventsOptions_Disabled = 0;
    public const int CoalesceEventsOptions_Enabled = 1;
}

public static class ConditionType
{
    public const int ConditionType_True = 0;
    public const int ConditionType_False = 1;
    public const int ConditionType_Property = 2;
    public const int ConditionType_And = 3;
    public const int ConditionType_Or = 4;
    public const int ConditionType_Not = 5;
}

public static class ConnectionRecoveryBehaviorOptions
{
    public const int ConnectionRecoveryBehaviorOptions_Disabled = 0;
    public const int ConnectionRecoveryBehaviorOptions_Enabled = 1;
}

public static class EventArgsType
{
    public const int EventArgsType_Simple = 0;
    public const int EventArgsType_PropertyChanged = 1;
    public const int EventArgsType_StructureChanged = 2;
    public const int EventArgsType_AsyncContentLoaded = 3;
    public const int EventArgsType_WindowClosed = 4;
    public const int EventArgsType_TextEditTextChanged = 5;
    public const int EventArgsType_Changes = 6;
    public const int EventArgsType_Notification = 7;
    public const int EventArgsType_ActiveTextPositionChanged = 8;
    public const int EventArgsType_StructuredMarkup = 9;
}

public static class FillType
{
    public const int FillType_None = 0;
    public const int FillType_Color = 1;
    public const int FillType_Gradient = 2;
    public const int FillType_Picture = 3;
    public const int FillType_Pattern = 4;
}

public static class FlowDirections
{
    public const int FlowDirections_Default = 0;
    public const int FlowDirections_RightToLeft = 1;
    public const int FlowDirections_BottomToTop = 2;
    public const int FlowDirections_Vertical = 4;
}

public static class LiveSetting
{
    public const int Off = 0;
    public const int Polite = 1;
    public const int Assertive = 2;
}

public static class NormalizeState
{
    public const int NormalizeState_None = 0;
    public const int NormalizeState_View = 1;
    public const int NormalizeState_Custom = 2;
}

public static class NotificationKind
{
    public const int NotificationKind_ItemAdded = 0;
    public const int NotificationKind_ItemRemoved = 1;
    public const int NotificationKind_ActionCompleted = 2;
    public const int NotificationKind_ActionAborted = 3;
    public const int NotificationKind_Other = 4;
}

public static class NotificationProcessing
{
    public const int NotificationProcessing_ImportantAll = 0;
    public const int NotificationProcessing_ImportantMostRecent = 1;
    public const int NotificationProcessing_All = 2;
    public const int NotificationProcessing_MostRecent = 3;
    public const int NotificationProcessing_CurrentThenMostRecent = 4;
    public const int NotificationProcessing_ImportantCurrentThenMostRecent = 5;
}

public static class OutlineStyles
{
    public const int OutlineStyles_None = 0;
    public const int OutlineStyles_Outline = 1;
    public const int OutlineStyles_Shadow = 2;
    public const int OutlineStyles_Engraved = 4;
    public const int OutlineStyles_Embossed = 8;
}

public static class PropertyConditionFlags
{
    public const int PropertyConditionFlags_None = 0;
    public const int PropertyConditionFlags_IgnoreCase = 1;
    public const int PropertyConditionFlags_MatchSubstring = 2;
}

public static class ProviderOptions
{
    public const int ProviderOptions_ClientSideProvider = 1;
    public const int ProviderOptions_ServerSideProvider = 2;
    public const int ProviderOptions_NonClientAreaProvider = 4;
    public const int ProviderOptions_OverrideProvider = 8;
    public const int ProviderOptions_ProviderOwnsSetFocus = 16;
    public const int ProviderOptions_UseComThreading = 32;
    public const int ProviderOptions_RefuseNonClientSupport = 64;
    public const int ProviderOptions_HasNativeIAccessible = 128;
    public const int ProviderOptions_UseClientCoordinates = 256;
}

public static class ProviderType
{
    public const int ProviderType_BaseHwnd = 0;
    public const int ProviderType_Proxy = 1;
    public const int ProviderType_NonClientArea = 2;
}

public static class SayAsInterpretAs
{
    public const int SayAsInterpretAs_None = 0;
    public const int SayAsInterpretAs_Spell = 1;
    public const int SayAsInterpretAs_Cardinal = 2;
    public const int SayAsInterpretAs_Ordinal = 3;
    public const int SayAsInterpretAs_Number = 4;
    public const int SayAsInterpretAs_Date = 5;
    public const int SayAsInterpretAs_Time = 6;
    public const int SayAsInterpretAs_Telephone = 7;
    public const int SayAsInterpretAs_Currency = 8;
    public const int SayAsInterpretAs_Net = 9;
    public const int SayAsInterpretAs_Url = 10;
    public const int SayAsInterpretAs_Address = 11;
    public const int SayAsInterpretAs_Alphanumeric = 12;
    public const int SayAsInterpretAs_Name = 13;
    public const int SayAsInterpretAs_Media = 14;
    public const int SayAsInterpretAs_Date_MonthDayYear = 15;
    public const int SayAsInterpretAs_Date_DayMonthYear = 16;
    public const int SayAsInterpretAs_Date_YearMonthDay = 17;
    public const int SayAsInterpretAs_Date_YearMonth = 18;
    public const int SayAsInterpretAs_Date_MonthYear = 19;
    public const int SayAsInterpretAs_Date_DayMonth = 20;
    public const int SayAsInterpretAs_Date_MonthDay = 21;
    public const int SayAsInterpretAs_Date_Year = 22;
    public const int SayAsInterpretAs_Time_HoursMinutesSeconds12 = 23;
    public const int SayAsInterpretAs_Time_HoursMinutes12 = 24;
    public const int SayAsInterpretAs_Time_HoursMinutesSeconds24 = 25;
    public const int SayAsInterpretAs_Time_HoursMinutes24 = 26;
}

public static class StructureChangeType
{
    public const int StructureChangeType_ChildAdded = 0;
    public const int StructureChangeType_ChildRemoved = 1;
    public const int StructureChangeType_ChildrenInvalidated = 2;
    public const int StructureChangeType_ChildrenBulkAdded = 3;
    public const int StructureChangeType_ChildrenBulkRemoved = 4;
    public const int StructureChangeType_ChildrenReordered = 5;
}

public static class SupportedTextSelection
{
    public const int SupportedTextSelection_None = 0;
    public const int SupportedTextSelection_Single = 1;
    public const int SupportedTextSelection_Multiple = 2;
}

public static class SynchronizedInputType
{
    public const int SynchronizedInputType_KeyUp = 1;
    public const int SynchronizedInputType_KeyDown = 2;
    public const int SynchronizedInputType_LeftMouseUp = 4;
    public const int SynchronizedInputType_LeftMouseDown = 8;
    public const int SynchronizedInputType_RightMouseUp = 16;
    public const int SynchronizedInputType_RightMouseDown = 32;
}

public static class TextDecorationLineStyle
{
    public const int TextDecorationLineStyle_None = 0;
    public const int TextDecorationLineStyle_Single = 1;
    public const int TextDecorationLineStyle_WordsOnly = 2;
    public const int TextDecorationLineStyle_Double = 3;
    public const int TextDecorationLineStyle_Dot = 4;
    public const int TextDecorationLineStyle_Dash = 5;
    public const int TextDecorationLineStyle_DashDot = 6;
    public const int TextDecorationLineStyle_DashDotDot = 7;
    public const int TextDecorationLineStyle_Wavy = 8;
    public const int TextDecorationLineStyle_ThickSingle = 9;
    public const int TextDecorationLineStyle_DoubleWavy = 11;
    public const int TextDecorationLineStyle_ThickWavy = 12;
    public const int TextDecorationLineStyle_LongDash = 13;
    public const int TextDecorationLineStyle_ThickDash = 14;
    public const int TextDecorationLineStyle_ThickDashDot = 15;
    public const int TextDecorationLineStyle_ThickDashDotDot = 16;
    public const int TextDecorationLineStyle_ThickDot = 17;
    public const int TextDecorationLineStyle_ThickLongDash = 18;
    public const int TextDecorationLineStyle_Other = -1;
}

public static class TextEditChangeType
{
    public const int TextEditChangeType_None = 0;
    public const int TextEditChangeType_AutoCorrect = 1;
    public const int TextEditChangeType_Composition = 2;
    public const int TextEditChangeType_CompositionFinalized = 3;
    public const int TextEditChangeType_AutoComplete = 4;
}

public static class TreeScope
{
    public const int TreeScope_None = 0;
    public const int TreeScope_Element = 1;
    public const int TreeScope_Children = 2;
    public const int TreeScope_Descendants = 4;
    public const int TreeScope_Parent = 8;
    public const int TreeScope_Ancestors = 16;
    public const int TreeScope_Subtree = 7;
}

public static class TreeTraversalOptions
{
    public const int TreeTraversalOptions_Default = 0;
    public const int TreeTraversalOptions_PostOrder = 1;
    public const int TreeTraversalOptions_LastToFirstOrder = 2;
}

public static class UIAutomationType
{
    public const int UIAutomationType_Int = 1;
    public const int UIAutomationType_Bool = 2;
    public const int UIAutomationType_String = 3;
    public const int UIAutomationType_Double = 4;
    public const int UIAutomationType_Point = 5;
    public const int UIAutomationType_Rect = 6;
    public const int UIAutomationType_Element = 7;
    public const int UIAutomationType_Array = 65536;
    public const int UIAutomationType_Out = 131072;
    public const int UIAutomationType_IntArray = 131073;
    public const int UIAutomationType_BoolArray = 131074;
    public const int UIAutomationType_StringArray = 131075;
    public const int UIAutomationType_DoubleArray = 131076;
    public const int UIAutomationType_PointArray = 131077;
    public const int UIAutomationType_RectArray = 131078;
    public const int UIAutomationType_ElementArray = 131079;
    public const int UIAutomationType_OutInt = 131080;
    public const int UIAutomationType_OutBool = 131081;
    public const int UIAutomationType_OutString = 131082;
    public const int UIAutomationType_OutDouble = 131083;
    public const int UIAutomationType_OutPoint = 131084;
    public const int UIAutomationType_OutRect = 131085;
    public const int UIAutomationType_OutElement = 131086;
    public const int UIAutomationType_OutIntArray = 131087;
    public const int UIAutomationType_OutBoolArray = 131088;
    public const int UIAutomationType_OutStringArray = 131089;
    public const int UIAutomationType_OutDoubleArray = 131090;
    public const int UIAutomationType_OutPointArray = 131091;
    public const int UIAutomationType_OutRectArray = 131092;
    public const int UIAutomationType_OutElementArray = 131093;
}

public static class VisualEffects
{
    public const int VisualEffects_None = 0;
    public const int VisualEffects_Shadow = 1;
    public const int VisualEffects_Reflection = 2;
    public const int VisualEffects_Glow = 3;
    public const int VisualEffects_SoftEdges = 4;
    public const int VisualEffects_Bevel = 5;
}
