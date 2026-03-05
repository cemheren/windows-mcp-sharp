namespace WindowsMcp.Tree;

public static class TreeConfig
{
    public static readonly HashSet<string> InteractiveControlTypeNames =
    [
        "ButtonControl",
        "ListItemControl",
        "MenuItemControl",
        "EditControl",
        "CheckBoxControl",
        "RadioButtonControl",
        "ComboBoxControl",
        "HyperlinkControl",
        "SplitButtonControl",
        "TabItemControl",
        "TreeItemControl",
        "DataItemControl",
        "HeaderItemControl",
        "TextBoxControl",
        "SpinnerControl",
        "ScrollBarControl",
    ];

    public static readonly HashSet<string> InteractiveRoles =
    [
        // Buttons
        "PushButton",
        "SplitButton",
        "ButtonDropDown",
        "ButtonMenu",
        "ButtonDropDownGrid",
        "OutlineButton",
        // Links
        "Link",
        // Inputs & Selection
        "Text",
        "IpAddress",
        "HotkeyField",
        "ComboBox",
        "DropList",
        "CheckButton",
        "RadioButton",
        // Menus & Tabs
        "MenuItem",
        "ListItem",
        "PageTab",
        // Trees
        "OutlineItem",
        // Values
        "Slider",
        "SpinButton",
        "Dial",
        "ScrollBar",
        "Grip",
        // Grids
        "ColumnHeader",
        "RowHeader",
        "Cell",
    ];

    public static readonly HashSet<string> DocumentControlTypeNames =
    [
        "DocumentControl",
    ];

    public static readonly HashSet<string> StructuralControlTypeNames =
    [
        "PaneControl",
        "GroupControl",
        "CustomControl",
    ];

    public static readonly HashSet<string> InformativeControlTypeNames =
    [
        "TextControl",
        "ImageControl",
        "StatusBarControl",
        // "ProgressBarControl",
        // "ToolTipControl",
        // "TitleBarControl",
        // "SeparatorControl",
        // "HeaderControl",
        // "HeaderItemControl",
    ];

    public static readonly HashSet<string> DefaultActions =
    [
        "Click",
        "Press",
        "Jump",
        "Check",
        "Uncheck",
        "Double Click",
    ];

    public const int ThreadMaxRetries = 3;
}
