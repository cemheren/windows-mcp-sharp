#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable SYSLIB1054 // Use LibraryImport instead of DllImport
#pragma warning disable CA1069 // Enums should not have duplicate values
#pragma warning disable CS0649 // Field is never assigned to

using System.Runtime.InteropServices;
using static WindowsMcp.Uia.ControlType;
using static WindowsMcp.Uia.MouseEventFlag;
using static WindowsMcp.Uia.KeyboardEventFlag;
using static WindowsMcp.Uia.InputType;
using static WindowsMcp.Uia.Keys;
using static WindowsMcp.Uia.PatternId;
using static WindowsMcp.Uia.PropertyId;
using static WindowsMcp.Uia.TreeScope;
using static WindowsMcp.Uia.SW;
using static WindowsMcp.Uia.SWP;
using static WindowsMcp.Uia.ProcessDpiAwareness;

namespace WindowsMcp.Uia;

// ──────────────────────────────────────────────
// Constants
// ──────────────────────────────────────────────

public static class UiaConstants
{
    public const string MetroWindowClassName = "Windows.UI.Core.CoreWindow";
    public const double SearchInterval = 0.5;
    public const double MaxMoveSecond = 1.0;
    public const double TimeOutSecond = 10.0;
    public const double OperationWaitTime = 0.5;
    public const int MaxPath = 260;
    public const int S_OK = 0;
}

// ──────────────────────────────────────────────
// COM Interface Declarations
// ──────────────────────────────────────────────

[ComImport]
[Guid("30cbe57d-d9d0-452a-ab13-7ac5ac4825ee")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomation
{
    int CompareElements(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement el1,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement el2);

    int CompareRuntimeIds(
        [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] int[] runtimeId1,
        [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] int[] runtimeId2);

    IUIAutomationElement GetRootElement();

    IUIAutomationElement ElementFromHandle(IntPtr hwnd);

    IUIAutomationElement ElementFromPoint(tagPOINT pt);

    IUIAutomationElement GetFocusedElement();

    // BuildCache variants (must be present to maintain vtable order)
    void GetRootElementBuildCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] out IUIAutomationElement root);

    void ElementFromHandleBuildCache(
        IntPtr hwnd,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] out IUIAutomationElement element);

    void ElementFromPointBuildCache(
        tagPOINT pt,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] out IUIAutomationElement element);

    void GetFocusedElementBuildCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] out IUIAutomationElement element);

    IUIAutomationTreeWalker CreateTreeWalker(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition pCondition);

    IUIAutomationTreeWalker GetControlViewWalker();

    IUIAutomationTreeWalker GetContentViewWalker();

    IUIAutomationTreeWalker GetRawViewWalker();

    IUIAutomationCondition GetRawViewCondition();

    IUIAutomationCondition GetControlViewCondition();

    IUIAutomationCondition GetContentViewCondition();

    IUIAutomationCacheRequest CreateCacheRequest();

    IUIAutomationCondition CreateTrueCondition();

    IUIAutomationCondition CreateFalseCondition();

    IUIAutomationCondition CreatePropertyCondition(
        int propertyId,
        [MarshalAs(UnmanagedType.Struct)] object value);

    IUIAutomationCondition CreatePropertyConditionEx(
        int propertyId,
        [MarshalAs(UnmanagedType.Struct)] object value,
        int flags);

    IUIAutomationCondition CreateAndCondition(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition1,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition2);

    void CreateAndConditionFromArray(
        [MarshalAs(UnmanagedType.SafeArray)] IUIAutomationCondition[] conditions,
        [MarshalAs(UnmanagedType.Interface)] out IUIAutomationCondition newCondition);

    void CreateAndConditionFromNativeArray(
        IntPtr conditions,
        int conditionCount,
        [MarshalAs(UnmanagedType.Interface)] out IUIAutomationCondition newCondition);

    IUIAutomationCondition CreateOrCondition(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition1,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition2);

    void CreateOrConditionFromArray(
        [MarshalAs(UnmanagedType.SafeArray)] IUIAutomationCondition[] conditions,
        [MarshalAs(UnmanagedType.Interface)] out IUIAutomationCondition newCondition);

    void CreateOrConditionFromNativeArray(
        IntPtr conditions,
        int conditionCount,
        [MarshalAs(UnmanagedType.Interface)] out IUIAutomationCondition newCondition);

    IUIAutomationCondition CreateNotCondition(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition);

    void AddAutomationEventHandler(
        int eventId,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        int scope,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] object handler);

    void RemoveAutomationEventHandler(
        int eventId,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] object handler);

    void AddPropertyChangedEventHandlerNativeArray(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        int scope,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] object handler,
        IntPtr propertyArray,
        int propertyCount);

    void AddPropertyChangedEventHandler(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        int scope,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] object handler,
        [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] int[] propertyArray);

    void RemovePropertyChangedEventHandler(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] object handler);

    void AddStructureChangedEventHandler(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        int scope,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] object handler);

    void RemoveStructureChangedEventHandler(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] object handler);

    void AddFocusChangedEventHandler(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest,
        [MarshalAs(UnmanagedType.Interface)] object handler);

    void RemoveFocusChangedEventHandler(
        [MarshalAs(UnmanagedType.Interface)] object handler);

    void RemoveAllEventHandlers();
}

[ComImport]
[Guid("c270f6b5-5c69-4290-9745-7a7f97169468")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationFocusChangedEventHandler
{
    [PreserveSig]
    int HandleFocusChangedEvent(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement sender);
}

[ComImport]
[Guid("e81d1b4e-11c5-42f8-9754-e7036c79f054")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationStructureChangedEventHandler
{
    [PreserveSig]
    int HandleStructureChangedEvent(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement sender,
        int changeType,
        [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)] int[] runtimeId);
}

[ComImport]
[Guid("40cd37d4-c756-4b0c-8c6f-bddfeeb13b50")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationPropertyChangedEventHandler
{
    [PreserveSig]
    int HandlePropertyChangedEvent(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement sender,
        int propertyId,
        object newValue);
}

[ComImport]
[Guid("d22108aa-8ac5-49a5-837b-37bbb3d7591e")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationElement
{
    void SetFocus();

    [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_I4)]
    int[] GetRuntimeId();

    IUIAutomationElement? FindFirst(
        int scope,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition);

    IUIAutomationElementArray? FindAll(
        int scope,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition);

    IUIAutomationElement? FindFirstBuildCache(
        int scope,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    IUIAutomationElementArray? FindAllBuildCache(
        int scope,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCondition condition,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    IUIAutomationElement? BuildUpdatedCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    [return: MarshalAs(UnmanagedType.Struct)]
    object GetCurrentPropertyValue(int propertyId);

    [return: MarshalAs(UnmanagedType.Struct)]
    object GetCurrentPropertyValueEx(int propertyId, int ignoreDefaultValue);

    [return: MarshalAs(UnmanagedType.Struct)]
    object GetCachedPropertyValue(int propertyId);

    [return: MarshalAs(UnmanagedType.Struct)]
    object GetCachedPropertyValueEx(int propertyId, int ignoreDefaultValue);

    IntPtr GetCurrentPatternAs(int patternId, ref Guid riid);

    IntPtr GetCachedPatternAs(int patternId, ref Guid riid);

    [return: MarshalAs(UnmanagedType.IUnknown)]
    object? GetCurrentPattern(int patternId);

    [return: MarshalAs(UnmanagedType.IUnknown)]
    object? GetCachedPattern(int patternId);

    IUIAutomationElement? GetCachedParent();

    IUIAutomationElementArray? GetCachedChildren();

    int CurrentProcessId { get; }
    int CurrentControlType { get; }
    string CurrentLocalizedControlType { get; }
    string CurrentName { get; }
    string CurrentAcceleratorKey { get; }
    string CurrentAccessKey { get; }
    int CurrentHasKeyboardFocus { get; }
    int CurrentIsKeyboardFocusable { get; }
    int CurrentIsEnabled { get; }
    string CurrentAutomationId { get; }
    string CurrentClassName { get; }
    string CurrentHelpText { get; }
    int CurrentCulture { get; }
    int CurrentIsControlElement { get; }
    int CurrentIsContentElement { get; }
    int CurrentIsPassword { get; }
    IntPtr CurrentNativeWindowHandle { get; }
    string CurrentItemType { get; }
    int CurrentIsOffscreen { get; }
    int CurrentOrientation { get; }
    string CurrentFrameworkId { get; }
    int CurrentIsRequiredForForm { get; }
    string CurrentItemStatus { get; }
    tagRECT CurrentBoundingRectangle { get; }
    IUIAutomationElement? CurrentLabeledBy { get; }
    string CurrentAriaRole { get; }
    string CurrentAriaProperties { get; }
    int CurrentIsDataValidForForm { get; }
    IUIAutomationElementArray? CurrentControllerFor { get; }
    IUIAutomationElementArray? CurrentDescribedBy { get; }
    IUIAutomationElementArray? CurrentFlowsTo { get; }
    string CurrentProviderDescription { get; }
    int CachedProcessId { get; }
    int CachedControlType { get; }
    string CachedLocalizedControlType { get; }
    string CachedName { get; }
    string CachedAcceleratorKey { get; }
    string CachedAccessKey { get; }
    int CachedHasKeyboardFocus { get; }
    int CachedIsKeyboardFocusable { get; }
    int CachedIsEnabled { get; }
    string CachedAutomationId { get; }
    string CachedClassName { get; }
    string CachedHelpText { get; }
    int CachedCulture { get; }
    int CachedIsControlElement { get; }
    int CachedIsContentElement { get; }
    int CachedIsPassword { get; }
    IntPtr CachedNativeWindowHandle { get; }
    string CachedItemType { get; }
    int CachedIsOffscreen { get; }
    int CachedOrientation { get; }
    string CachedFrameworkId { get; }
    int CachedIsRequiredForForm { get; }
    string CachedItemStatus { get; }
    tagRECT CachedBoundingRectangle { get; }
    IUIAutomationElement? CachedLabeledBy { get; }
    string CachedAriaRole { get; }
    string CachedAriaProperties { get; }
    int CachedIsDataValidForForm { get; }
    IUIAutomationElementArray? CachedControllerFor { get; }
    IUIAutomationElementArray? CachedDescribedBy { get; }
    IUIAutomationElementArray? CachedFlowsTo { get; }
    string CachedProviderDescription { get; }

    void GetClickablePoint(out tagPOINT clickable, out int gotClickable);
}

[ComImport]
[Guid("14314595-b4bc-4055-95f2-58f2e42c9855")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationElementArray
{
    int Length { get; }
    IUIAutomationElement GetElement(int index);
}

[ComImport]
[Guid("352ffba8-0973-437c-a61f-f64cafd81df9")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationCondition
{
}

[ComImport]
[Guid("b32a92b5-bc25-4078-9c08-d7ee95c48e03")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationCacheRequest
{
    void AddProperty(int propertyId);
    void AddPattern(int patternId);
    IUIAutomationCacheRequest Clone();
    int TreeScope { get; set; }
    IUIAutomationCondition TreeFilter { get; set; }
    int AutomationElementMode { get; set; }
}

[ComImport]
[Guid("88f4d42a-e881-459d-a77c-73bbbb7e02dc")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationScrollPattern
{
    void Scroll(int horizontalAmount, int verticalAmount);
    void SetScrollPercent(double horizontalPercent, double verticalPercent);
    double CurrentHorizontalScrollPercent { get; }
    double CurrentVerticalScrollPercent { get; }
    double CurrentHorizontalViewSize { get; }
    double CurrentVerticalViewSize { get; }
    int CurrentHorizontallyScrollable { get; }
    int CurrentVerticallyScrollable { get; }
    double CachedHorizontalScrollPercent { get; }
    double CachedVerticalScrollPercent { get; }
    double CachedHorizontalViewSize { get; }
    double CachedVerticalViewSize { get; }
    int CachedHorizontallyScrollable { get; }
    int CachedVerticallyScrollable { get; }
}

[ComImport]
[Guid("4042c624-389c-4afc-a630-9df854a541fc")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IUIAutomationTreeWalker
{
    IUIAutomationElement? GetParentElement(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element);

    IUIAutomationElement? GetFirstChildElement(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element);

    IUIAutomationElement? GetLastChildElement(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element);

    IUIAutomationElement? GetNextSiblingElement(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element);

    IUIAutomationElement? GetPreviousSiblingElement(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element);

    IUIAutomationElement? NormalizeElement(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element);

    IUIAutomationElement? GetParentElementBuildCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    IUIAutomationElement? GetFirstChildElementBuildCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    IUIAutomationElement? GetLastChildElementBuildCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    IUIAutomationElement? GetNextSiblingElementBuildCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    IUIAutomationElement? GetPreviousSiblingElementBuildCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    IUIAutomationElement? NormalizeElementBuildCache(
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationElement element,
        [MarshalAs(UnmanagedType.Interface)] IUIAutomationCacheRequest cacheRequest);

    IUIAutomationCondition Condition { get; }
}

// ──────────────────────────────────────────────
// P/Invoke Structures
// ──────────────────────────────────────────────

[StructLayout(LayoutKind.Sequential)]
public struct tagPOINT
{
    public int x;
    public int y;

    public tagPOINT(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct tagRECT
{
    public int left;
    public int top;
    public int right;
    public int bottom;
}

[StructLayout(LayoutKind.Sequential)]
public struct BITMAPINFOHEADER
{
    public uint biSize;
    public int biWidth;
    public int biHeight;
    public ushort biPlanes;
    public ushort biBitCount;
    public uint biCompression;
    public uint biSizeImage;
    public int biXPelsPerMeter;
    public int biYPelsPerMeter;
    public uint biClrUsed;
    public uint biClrImportant;
}

[StructLayout(LayoutKind.Sequential)]
public struct MOUSEINPUT
{
    public int dx;
    public int dy;
    public uint mouseData;
    public uint dwFlags;
    public uint time;
    public UIntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
public struct KEYBDINPUT
{
    public ushort wVk;
    public ushort wScan;
    public uint dwFlags;
    public uint time;
    public UIntPtr dwExtraInfo;
}

[StructLayout(LayoutKind.Sequential)]
public struct HARDWAREINPUT
{
    public uint uMsg;
    public ushort wParamL;
    public ushort wParamH;
}

[StructLayout(LayoutKind.Explicit)]
public struct InputUnion
{
    [FieldOffset(0)] public MOUSEINPUT mi;
    [FieldOffset(0)] public KEYBDINPUT ki;
    [FieldOffset(0)] public HARDWAREINPUT hi;
}

[StructLayout(LayoutKind.Sequential)]
public struct INPUT
{
    public uint type;
    public InputUnion union;

    public static INPUT FromMouse(MOUSEINPUT mi) =>
        new() { type = (uint)Mouse, union = new InputUnion { mi = mi } };

    public static INPUT FromKeyboard(KEYBDINPUT ki) =>
        new() { type = (uint)Keyboard, union = new InputUnion { ki = ki } };

    public static INPUT FromHardware(HARDWAREINPUT hi) =>
        new() { type = (uint)Hardware, union = new InputUnion { hi = hi } };
}

// ──────────────────────────────────────────────
// Rect helper class
// ──────────────────────────────────────────────

public class Rect
{
    public int Left { get; set; }
    public int Top { get; set; }
    public int Right { get; set; }
    public int Bottom { get; set; }

    public Rect(int left = 0, int top = 0, int right = 0, int bottom = 0)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public int Width() => Right - Left;
    public int Height() => Bottom - Top;
    public int XCenter() => Left + Width() / 2;
    public int YCenter() => Top + Height() / 2;
    public bool IsEmpty() => Width() == 0 || Height() == 0;
    public bool Contains(int x, int y) => Left <= x && x < Right && Top <= y && y < Bottom;

    public Rect Intersect(Rect other) =>
        new(Math.Max(Left, other.Left), Math.Max(Top, other.Top),
            Math.Min(Right, other.Right), Math.Min(Bottom, other.Bottom));

    public void Offset(int x, int y)
    {
        Left += x; Right += x; Top += y; Bottom += y;
    }

    public override bool Equals(object? obj) =>
        obj is Rect r && Left == r.Left && Top == r.Top && Right == r.Right && Bottom == r.Bottom;

    public override int GetHashCode() => HashCode.Combine(Left, Top, Right, Bottom);

    public override string ToString() =>
        $"({Left},{Top},{Right},{Bottom})[{Width()}x{Height()}]";
}

// ──────────────────────────────────────────────
// ProcessInfo
// ──────────────────────────────────────────────

public class ProcessInfo
{
    public int Pid { get; set; }
    public int Ppid { get; set; } = -1;
    public string ExeName { get; set; } = "";
    public bool? Is64Bit { get; set; }
    public string ExePath { get; set; } = "";
    public string CmdLine { get; set; } = "";

    public ProcessInfo(string exeName, int pid, int ppid = -1,
        string exePath = "", string cmdLine = "")
    {
        ExeName = exeName; Pid = pid; Ppid = ppid;
        ExePath = exePath; CmdLine = cmdLine;
    }

    public override string ToString() =>
        $"ProcessInfo(pid={Pid}, ppid={Ppid}, exeName='{ExeName}', " +
        $"is64Bit={Is64Bit}, exePath='{ExePath}', cmdLine='{CmdLine}')";
}

// ──────────────────────────────────────────────
// CacheRequest wrapper
// ──────────────────────────────────────────────

public class CacheRequest
{
    public IUIAutomationCacheRequest ComCacheRequest { get; }

    public CacheRequest(IUIAutomationCacheRequest? cacheRequest = null)
    {
        ComCacheRequest = cacheRequest
            ?? AutomationClient.Instance.Automation.CreateCacheRequest();
    }

    public int TreeScopeValue
    {
        get => ComCacheRequest.TreeScope;
        set => ComCacheRequest.TreeScope = value;
    }

    public int AutomationElementMode
    {
        get => ComCacheRequest.AutomationElementMode;
        set => ComCacheRequest.AutomationElementMode = value;
    }

    public IUIAutomationCondition TreeFilter
    {
        get => ComCacheRequest.TreeFilter;
        set => ComCacheRequest.TreeFilter = value;
    }

    public void AddProperty(int propertyId) => ComCacheRequest.AddProperty(propertyId);
    public void AddPattern(int patternId) => ComCacheRequest.AddPattern(patternId);
    public CacheRequest Clone() => new(ComCacheRequest.Clone());
}

// ──────────────────────────────────────────────
// Win32 P/Invoke Declarations
// ──────────────────────────────────────────────

public static class Win32
{
    [DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(tagPOINT point);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out tagPOINT lpPoint);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetPhysicalCursorPos(out tagPOINT lpPoint);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    public static extern uint GetDoubleClickTime();

    [DllImport("user32.dll")]
    public static extern void mouse_event(
        uint dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    public static extern void keybd_event(
        byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessageW(
        IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int SendMessageW(
        IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool BringWindowToTop(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern void SwitchToThisWindow(
        IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool fAltTab);

    [DllImport("user32.dll")]
    public static extern IntPtr GetAncestor(IntPtr hWnd, int gaFlags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsTopLevelWindow(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowLongW(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int SetWindowLongW(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsIconic(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsZoomed(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MoveWindow(
        IntPtr hWnd, int x, int y, int nWidth, int nHeight,
        [MarshalAs(UnmanagedType.Bool)] bool bRepaint);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(
        IntPtr hWnd, IntPtr hWndInsertAfter,
        int x, int y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowTextW(
        IntPtr hWnd, [Out] char[] lpString, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowTextW(IntPtr hWnd, string lpString);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, out tagRECT lpRect);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    public static extern uint SendInput(
        uint nInputs, INPUT[] pInputs, int cbSize);

    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll")]
    public static extern int GetWindowThreadProcessId(
        IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBoxW(
        IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr OpenDesktopW(
        string lpszDesktop, uint dwFlags,
        [MarshalAs(UnmanagedType.Bool)] bool fInherit, uint dwDesiredAccess);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SwitchDesktop(IntPtr hDesktop);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseDesktop(IntPtr hDesktop);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumDisplayMonitors(
        IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

    public delegate bool MonitorEnumProc(
        IntPtr hMonitor, IntPtr hdcMonitor, ref tagRECT lprcMonitor, IntPtr dwData);

    [DllImport("user32.dll")]
    public static extern uint MapVirtualKeyA(uint uCode, uint uMapType);

    [DllImport("user32.dll")]
    public static extern short VkKeyScanW(char ch);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EmptyClipboard();

    [DllImport("user32.dll")]
    public static extern IntPtr GetClipboardData(uint uFormat);

    [DllImport("user32.dll")]
    public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsClipboardFormatAvailable(uint format);

    [DllImport("user32.dll")]
    public static extern uint EnumClipboardFormats(uint format);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern uint RegisterClipboardFormatW(string lpszFormat);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetClipboardFormatNameW(
        uint format, [Out] char[] lpszFormatName, int cchMaxCount);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [DllImport("gdi32.dll")]
    public static extern uint GetPixel(IntPtr hdc, int x, int y);

    [DllImport("gdi32.dll")]
    public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool BitBlt(
        IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest,
        IntPtr hdcSrc, int xSrc, int ySrc, uint rop);

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    public static extern int GetDIBits(
        IntPtr hdc, IntPtr hbmp, uint uStartScan, uint cScanLines,
        IntPtr lpvBits, ref BITMAPINFOHEADER lpbi, uint uUsage);

    public const uint SRCCOPY = 0x00CC0020;

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetConsoleWindow();

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GlobalFree(IntPtr hMem);

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(
        uint dwDesiredAccess,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
        int dwProcessId);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool TerminateProcess(IntPtr hProcess, int uExitCode);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetConsoleTitleW(string lpConsoleTitle);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetConsoleTitleW(
        [Out] char[] lpConsoleTitle, int nSize);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetConsoleTextAttribute(
        IntPtr hConsoleOutput, ushort wAttributes);

    [DllImport("shell32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsUserAnAdmin();

    [DllImport("shcore.dll")]
    public static extern int SetProcessDpiAwareness(int awareness);

    [DllImport("dwmapi.dll")]
    public static extern int DwmIsCompositionEnabled(
        [MarshalAs(UnmanagedType.Bool)] out bool pfEnabled);

    [DllImport("dwmapi.dll")]
    public static extern int DwmGetWindowAttribute(
        IntPtr hWnd, int dwAttribute, out tagRECT pvAttribute, int cbAttribute);

    [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PlaySoundW(
        string? pszSound, IntPtr hmod, uint fdwSound);
}

// ──────────────────────────────────────────────
// AutomationClient Singleton
// ──────────────────────────────────────────────

public sealed class AutomationClient
{
    private static readonly Lazy<AutomationClient> _lazy = new(() => new AutomationClient());
    public static AutomationClient Instance => _lazy.Value;

    public IUIAutomation Automation { get; }
    public IUIAutomationTreeWalker ViewWalker { get; }

    private static readonly Guid CUIAutomationClsid =
        new("ff48dba4-60ef-4201-aa87-54103eef594e");

    private AutomationClient()
    {
        const int tryCount = 3;
        Exception? lastEx = null;
        for (int retry = 0; retry < tryCount; retry++)
        {
            try
            {
                var uiaType = Type.GetTypeFromCLSID(CUIAutomationClsid)
                    ?? throw new InvalidOperationException(
                        "Failed to get CUIAutomation type from CLSID.");
                var obj = Activator.CreateInstance(uiaType)
                    ?? throw new InvalidOperationException(
                        "Failed to create CUIAutomation instance.");
                Automation = (IUIAutomation)obj;
                ViewWalker = Automation.GetRawViewWalker();
                return;
            }
            catch (Exception ex)
            {
                lastEx = ex;
            }
        }
        throw lastEx!;
    }
}

// ──────────────────────────────────────────────
// Global Win32 Helper Functions
// ──────────────────────────────────────────────

public static class UiaHelpers
{
    public static (int x, int y) GetCursorPos()
    {
        Win32.GetCursorPos(out tagPOINT pt);
        return (pt.x, pt.y);
    }

    public static (int x, int y) GetPhysicalCursorPos()
    {
        Win32.GetPhysicalCursorPos(out tagPOINT pt);
        return (pt.x, pt.y);
    }

    public static bool SetCursorPos(int x, int y) => Win32.SetCursorPos(x, y);

    public static IntPtr WindowFromPoint(int x, int y) =>
        Win32.WindowFromPoint(new tagPOINT(x, y));

    public static (int width, int height) GetScreenSize()
    {
        const int SM_CXSCREEN = 0;
        const int SM_CYSCREEN = 1;
        return (Win32.GetSystemMetrics(SM_CXSCREEN),
                Win32.GetSystemMetrics(SM_CYSCREEN));
    }

    public static (int width, int height) GetVirtualScreenSize() =>
        (Win32.GetSystemMetrics(78), Win32.GetSystemMetrics(79));

    public static (int left, int top, int width, int height) GetVirtualScreenRect() =>
        (Win32.GetSystemMetrics(76), Win32.GetSystemMetrics(77),
         Win32.GetSystemMetrics(78), Win32.GetSystemMetrics(79));

    public static List<Rect> GetMonitorsRect()
    {
        var rects = new List<Rect>();
        Win32.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
            (IntPtr hMon, IntPtr hdc, ref tagRECT rc, IntPtr data) =>
            {
                rects.Add(new Rect(rc.left, rc.top, rc.right, rc.bottom));
                return true;
            }, IntPtr.Zero);
        return rects;
    }

    public static void Click(int x, int y,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        SetCursorPos(x, y);
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(LeftDown | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep(50);
        Win32.mouse_event(LeftUp | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void RightClick(int x, int y,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        SetCursorPos(x, y);
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(RightDown | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep(50);
        Win32.mouse_event(RightUp | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void MiddleClick(int x, int y,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        SetCursorPos(x, y);
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(MiddleDown | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep(50);
        Win32.mouse_event(MiddleUp | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void PressMouse(int x, int y,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        SetCursorPos(x, y);
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(LeftDown | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void ReleaseMouse(
        double waitTime = UiaConstants.OperationWaitTime)
    {
        var (x, y) = GetCursorPos();
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(LeftUp | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void RightPressMouse(int x, int y,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        SetCursorPos(x, y);
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(RightDown | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void RightReleaseMouse(
        double waitTime = UiaConstants.OperationWaitTime)
    {
        var (x, y) = GetCursorPos();
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(RightUp | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void MiddlePressMouse(int x, int y,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        SetCursorPos(x, y);
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(MiddleDown | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void MiddleReleaseMouse(
        double waitTime = UiaConstants.OperationWaitTime)
    {
        var (x, y) = GetCursorPos();
        var (sw, sh) = GetScreenSize();
        Win32.mouse_event(MiddleUp | Absolute,
            x * 65535 / sw, y * 65535 / sh, 0, UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void MoveTo(int x, int y, double moveSpeed = 1.0,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        double moveTime = moveSpeed <= 0 ? 0.0 : UiaConstants.MaxMoveSecond / moveSpeed;
        var (curX, curY) = GetCursorPos();
        var (sw, sh) = GetScreenSize();
        int xCount = Math.Abs(x - curX);
        int yCount = Math.Abs(y - curY);
        int maxPoint = Math.Max(xCount, yCount);
        int maxSide = Math.Max(sw, sh);
        int minSide = Math.Min(sw, sh);
        if (maxPoint > minSide) maxPoint = minSide;
        if (maxPoint < maxSide)
        {
            maxPoint = 100 + (int)((maxSide - 100.0) / maxSide * maxPoint);
            moveTime = moveTime * maxPoint / maxSide;
        }
        int stepCount = maxPoint / 20;
        if (stepCount > 1)
        {
            double xStep = (x - curX) * 1.0 / stepCount;
            double yStep = (y - curY) * 1.0 / stepCount;
            double interval = moveTime / stepCount;
            for (int i = 0; i < stepCount; i++)
            {
                SetCursorPos(curX + (int)(xStep * i), curY + (int)(yStep * i));
                Thread.Sleep((int)(interval * 1000));
            }
        }
        SetCursorPos(x, y);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void DragDrop(int x1, int y1, int x2, int y2,
        double moveSpeed = 1.0, double waitTime = UiaConstants.OperationWaitTime)
    {
        PressMouse(x1, y1, 0.05);
        MoveTo(x2, y2, moveSpeed, 0.05);
        ReleaseMouse(waitTime);
    }

    public static void RightDragDrop(int x1, int y1, int x2, int y2,
        double moveSpeed = 1.0, double waitTime = UiaConstants.OperationWaitTime)
    {
        RightPressMouse(x1, y1, 0.05);
        MoveTo(x2, y2, moveSpeed, 0.05);
        RightReleaseMouse(waitTime);
    }

    public static void MiddleDragDrop(int x1, int y1, int x2, int y2,
        double moveSpeed = 1.0, double waitTime = UiaConstants.OperationWaitTime)
    {
        MiddlePressMouse(x1, y1, 0.05);
        MoveTo(x2, y2, moveSpeed, 0.05);
        MiddleReleaseMouse(waitTime);
    }

    public static void WheelDown(int wheelTimes = 1, double interval = 0.05,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        for (int i = 0; i < wheelTimes; i++)
        {
            Win32.mouse_event(Wheel, 0, 0, -120, UIntPtr.Zero);
            Thread.Sleep((int)(interval * 1000));
        }
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void WheelUp(int wheelTimes = 1, double interval = 0.05,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        for (int i = 0; i < wheelTimes; i++)
        {
            Win32.mouse_event(Wheel, 0, 0, 120, UIntPtr.Zero);
            Thread.Sleep((int)(interval * 1000));
        }
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static uint GetPixelColor(int x, int y, IntPtr handle = default)
    {
        IntPtr hdc = Win32.GetWindowDC(handle);
        uint bgr = Win32.GetPixel(hdc, x, y);
        Win32.ReleaseDC(handle, hdc);
        return bgr;
    }

    public static IntPtr GetForegroundWindow() => Win32.GetForegroundWindow();

    public static bool SetForegroundWindow(IntPtr handle) =>
        Win32.SetForegroundWindow(handle);

    public static bool BringWindowToTop(IntPtr handle) =>
        Win32.BringWindowToTop(handle);

    public static void SwitchToThisWindow(IntPtr handle) =>
        Win32.SwitchToThisWindow(handle, true);

    public static IntPtr GetAncestor(IntPtr handle, int flag) =>
        Win32.GetAncestor(handle, flag);

    public static bool IsTopLevelWindow(IntPtr handle) =>
        Win32.IsTopLevelWindow(handle);

    public static int GetWindowLong(IntPtr handle, int index) =>
        Win32.GetWindowLongW(handle, index);

    public static int SetWindowLong(IntPtr handle, int index, int value) =>
        Win32.SetWindowLongW(handle, index, value);

    public static bool IsIconic(IntPtr handle) => Win32.IsIconic(handle);
    public static bool IsZoomed(IntPtr handle) => Win32.IsZoomed(handle);

    public static bool IsWindowVisible(IntPtr handle) =>
        Win32.IsWindowVisible(handle);

    public static bool ShowWindow(IntPtr handle, int cmdShow) =>
        Win32.ShowWindow(handle, cmdShow);

    public static bool MoveWindow(IntPtr handle, int x, int y,
        int width, int height, bool repaint = true) =>
        Win32.MoveWindow(handle, x, y, width, height, repaint);

    public static bool SetWindowPos(IntPtr handle, IntPtr hWndInsertAfter,
        int x, int y, int width, int height, uint flags) =>
        Win32.SetWindowPos(handle, hWndInsertAfter, x, y, width, height, flags);

    public static bool SetWindowTopmost(IntPtr handle, bool isTopmost)
    {
        IntPtr topValue = new(isTopmost ? SWP.HWND_Topmost : SWP.HWND_NoTopmost);
        return SetWindowPos(handle, topValue, 0, 0, 0, 0,
            (uint)(SWP.SWP_NoSize | SWP.SWP_NoMove));
    }

    public static string GetWindowText(IntPtr handle)
    {
        var buf = new char[UiaConstants.MaxPath];
        Win32.GetWindowTextW(handle, buf, UiaConstants.MaxPath);
        int len = Array.IndexOf(buf, '\0');
        return len < 0 ? new string(buf) : new string(buf, 0, len);
    }

    public static bool SetWindowText(IntPtr handle, string text) =>
        Win32.SetWindowTextW(handle, text);

    public static Rect? GetWindowRect(IntPtr handle)
    {
        if (Win32.GetWindowRect(handle, out tagRECT rc))
            return new Rect(rc.left, rc.top, rc.right, rc.bottom);
        return null;
    }

    public static Rect? DwmGetWindowExtendFrameBounds(IntPtr handle)
    {
        const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
        int hr = Win32.DwmGetWindowAttribute(handle,
            DWMWA_EXTENDED_FRAME_BOUNDS, out tagRECT rect, Marshal.SizeOf<tagRECT>());
        return hr == UiaConstants.S_OK
            ? new Rect(rect.left, rect.top, rect.right, rect.bottom) : null;
    }

    public static bool DwmIsCompositionEnabled()
    {
        try
        {
            int hr = Win32.DwmIsCompositionEnabled(out bool enabled);
            return hr == UiaConstants.S_OK && enabled;
        }
        catch { return false; }
    }

    public static bool IsDesktopLocked()
    {
        const uint DESKTOP_SWITCHDESKTOP = 0x0100;
        IntPtr desk = Win32.OpenDesktopW("Default", 0, false, DESKTOP_SWITCHDESKTOP);
        if (desk == IntPtr.Zero) return false;
        bool isLocked = !Win32.SwitchDesktop(desk);
        Win32.CloseDesktop(desk);
        return isLocked;
    }

    public static int MessageBox(string content, string title, uint flags = 0) =>
        Win32.MessageBoxW(IntPtr.Zero, content, title, flags);

    public static void SendKey(int key,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        Win32.keybd_event((byte)key, 0, (uint)(KeyDown | ExtendedKey), UIntPtr.Zero);
        Win32.keybd_event((byte)key, 0, (uint)(KeyUp | ExtendedKey), UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void PressKey(int key,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        Win32.keybd_event((byte)key, 0, (uint)(KeyDown | ExtendedKey), UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static void ReleaseKey(int key,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        Win32.keybd_event((byte)key, 0, (uint)(KeyUp | ExtendedKey), UIntPtr.Zero);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static bool IsKeyPressed(int key) =>
        (Win32.GetAsyncKeyState(key) & 0x8000) != 0;

    public static INPUT MouseInput(int dx, int dy, int mouseData = 0,
        uint dwFlags = LeftDown, uint time = 0) =>
        INPUT.FromMouse(new MOUSEINPUT
        {
            dx = dx, dy = dy, mouseData = (uint)mouseData,
            dwFlags = dwFlags, time = time
        });

    public static INPUT KeyboardInput(int wVk, int wScan,
        uint dwFlags = (uint)KeyDown, uint time = 0) =>
        INPUT.FromKeyboard(new KEYBDINPUT
        {
            wVk = (ushort)wVk, wScan = (ushort)wScan,
            dwFlags = dwFlags, time = time
        });

    public static INPUT HardwareInput(int uMsg, int param = 0) =>
        INPUT.FromHardware(new HARDWAREINPUT
        {
            uMsg = (uint)uMsg,
            wParamL = (ushort)(param & 0xFFFF),
            wParamH = (ushort)((param >> 16) & 0xFFFF)
        });

    public static uint SendInput(params INPUT[] inputs) =>
        Win32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());

    public static uint SendUnicodeChar(char ch, bool charMode = true)
    {
        ushort vk, scan;
        uint flag;
        if (charMode)
        {
            vk = 0; scan = (ushort)ch; flag = KeyUnicode;
        }
        else
        {
            short res = Win32.VkKeyScanW(ch);
            if (((res >> 8) & 0xFF) == 0)
            { vk = (ushort)(res & 0xFF); scan = 0; flag = 0; }
            else
            { vk = 0; scan = (ushort)ch; flag = KeyUnicode; }
        }
        var inputs = new[]
        {
            INPUT.FromKeyboard(new KEYBDINPUT
                { wVk = vk, wScan = scan, dwFlags = flag | (uint)KeyDown }),
            INPUT.FromKeyboard(new KEYBDINPUT
                { wVk = vk, wScan = scan, dwFlags = flag | (uint)KeyUp }),
        };
        return Win32.SendInput(2, inputs, Marshal.SizeOf<INPUT>());
    }

    private static readonly Dictionary<int, int> SCKeys = new()
    {
        { VK_LSHIFT, 0x02A }, { VK_RSHIFT, 0x136 },
        { VK_LCONTROL, 0x01D }, { VK_RCONTROL, 0x11D },
        { VK_LMENU, 0x038 }, { VK_RMENU, 0x138 },
        { VK_LWIN, 0x15B }, { VK_RWIN, 0x15C },
        { VK_NUMPAD0, 0x52 }, { VK_NUMPAD1, 0x4F },
        { VK_NUMPAD2, 0x50 }, { VK_NUMPAD3, 0x51 },
        { VK_NUMPAD4, 0x4B }, { VK_NUMPAD5, 0x4C },
        { VK_NUMPAD6, 0x4D }, { VK_NUMPAD7, 0x47 },
        { VK_NUMPAD8, 0x48 }, { VK_NUMPAD9, 0x49 },
        { VK_DECIMAL, 0x53 }, { VK_NUMLOCK, 0x145 },
        { VK_DIVIDE, 0x135 }, { VK_MULTIPLY, 0x037 },
        { VK_SUBTRACT, 0x04A }, { VK_ADD, 0x04E },
    };

    private static int VKtoSC(int key)
    {
        if (SCKeys.TryGetValue(key, out int sc)) return sc;
        uint scanCode = Win32.MapVirtualKeyA((uint)key, 0);
        if (scanCode == 0) return 0;
        int[] extKeys = { VK_APPS, VK_CANCEL, VK_SNAPSHOT, VK_DIVIDE, VK_NUMLOCK };
        if (Array.IndexOf(extKeys, key) >= 0) scanCode |= 0x0100;
        return (int)scanCode;
    }

    /// <summary>
    /// Simulate typing keys on keyboard.
    /// Supports special keys: {Ctrl}, {Shift}, {Alt}, {Win}, {Enter}, {Delete}, etc.
    /// </summary>
    public static void SendKeys(string text, double interval = 0.01,
        double waitTime = UiaConstants.OperationWaitTime, bool charMode = true)
    {
        var holdKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "WIN", "LWIN", "RWIN", "SHIFT", "LSHIFT", "RSHIFT",
            "CTRL", "CONTROL", "LCTRL", "RCTRL", "LCONTROL", "RCONTROL",
            "ALT", "LALT", "RALT"
        };

        var keys = new List<(object key, int flag)>();
        int i = 0, insertIndex = 0, length = text.Length;
        bool hold = false, include = false;
        object? lastKeyValue = null;

        while (i < length)
        {
            if (text[i] == '{')
            {
                int rindex = text.IndexOf('}', i + 1);
                if (rindex == i + 1) rindex = text.IndexOf('}', i + 2);
                if (rindex == -1)
                    throw new ArgumentException(
                        "Unmatched '{' in SendKeys text");
                string keyStr = text[(i + 1)..rindex].Trim();
                var parts = keyStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                    throw new ArgumentException("Empty key name in SendKeys");
                string keyName = parts[0];
                string upperKey = keyName.ToUpperInvariant();
                int count = parts.Length > 1 ? int.Parse(parts[1]) : 1;

                for (int j = 0; j < count; j++)
                {
                    if (hold)
                    {
                        if (SpecialKeyNames.Names.TryGetValue(upperKey, out int kv))
                        {
                            if (lastKeyValue is int lk && lk == kv) insertIndex++;
                            keys.Insert(insertIndex, (kv, KeyDown | ExtendedKey));
                            keys.Insert(insertIndex + 1, (kv, KeyUp | ExtendedKey));
                            lastKeyValue = kv;
                        }
                        else if (CharacterCodes.Codes.TryGetValue(keyName.Length == 1 ? keyName[0] : '\0', out int cv))
                        {
                            if (lastKeyValue is int lc && lc == cv) insertIndex++;
                            keys.Insert(insertIndex, (cv, KeyDown | ExtendedKey));
                            keys.Insert(insertIndex + 1, (cv, KeyUp | ExtendedKey));
                            lastKeyValue = cv;
                        }
                        else
                        {
                            keys.Insert(insertIndex, ((object)keyName, -1));
                            lastKeyValue = keyName;
                        }
                        if (include) insertIndex++;
                        else if (holdKeys.Contains(upperKey)) insertIndex++;
                        else hold = false;
                    }
                    else
                    {
                        if (SpecialKeyNames.Names.TryGetValue(upperKey, out int kv))
                        {
                            keys.Add((kv, KeyDown | ExtendedKey));
                            keys.Add((kv, KeyUp | ExtendedKey));
                            lastKeyValue = kv;
                            if (holdKeys.Contains(upperKey))
                            { hold = true; insertIndex = keys.Count - 1; }
                        }
                        else
                        {
                            keys.Add(((object)keyName, -1));
                            lastKeyValue = keyName;
                        }
                    }
                }
                i = rindex + 1;
            }
            else if (text[i] == '(')
            {
                if (hold) include = true;
                else { keys.Add(((object)text[i].ToString(), -1)); lastKeyValue = text[i].ToString(); }
                i++;
            }
            else if (text[i] == ')')
            {
                if (hold) { include = false; hold = false; }
                else { keys.Add(((object)text[i].ToString(), -1)); lastKeyValue = text[i].ToString(); }
                i++;
            }
            else
            {
                if (hold)
                {
                    if (CharacterCodes.Codes.TryGetValue(text[i], out int cv))
                    {
                        if (include && lastKeyValue is int lc && lc == cv) insertIndex++;
                        keys.Insert(insertIndex, (cv, KeyDown | ExtendedKey));
                        keys.Insert(insertIndex + 1, (cv, KeyUp | ExtendedKey));
                        lastKeyValue = cv;
                    }
                    else
                    {
                        keys.Add(((object)text[i].ToString(), -1));
                        lastKeyValue = text[i].ToString();
                    }
                    if (include) insertIndex++;
                    else hold = false;
                }
                else
                {
                    keys.Add(((object)text[i].ToString(), -1));
                    lastKeyValue = text[i].ToString();
                }
                i++;
            }
        }

        int hotkeyMs = 10, intervalMs = (int)(interval * 1000);
        for (int k = 0; k < keys.Count; k++)
        {
            var (key, flag) = keys[k];
            if (flag == -1)
            {
                string ch = key is string s ? s : key.ToString()!;
                if (ch.Length == 1) SendUnicodeChar(ch[0], charMode);
                Thread.Sleep(intervalMs);
            }
            else
            {
                int vk = (int)key;
                Win32.keybd_event((byte)vk, (byte)VKtoSC(vk),
                    (uint)flag, UIntPtr.Zero);
                if (k + 1 == keys.Count)
                    Thread.Sleep(intervalMs);
                else if ((flag & KeyUp) != 0)
                    Thread.Sleep(keys[k + 1].flag == -1
                        || (keys[k + 1].flag & KeyUp) == 0 ? intervalMs : hotkeyMs);
                else
                    Thread.Sleep(hotkeyMs);
            }
        }
        Thread.Sleep((int)(waitTime * 1000));
    }

    public static bool PostMessage(IntPtr handle, uint msg,
        UIntPtr wParam, IntPtr lParam) =>
        Win32.PostMessageW(handle, msg, wParam, lParam);

    public static int SendMessage(IntPtr handle, uint msg,
        UIntPtr wParam, IntPtr lParam) =>
        Win32.SendMessageW(handle, msg, wParam, lParam);

    private static readonly object ClipboardLock = new();

    private static bool OpenClipboard()
    {
        long end = Environment.TickCount64 + 200;
        while (Environment.TickCount64 < end)
        {
            if (Win32.OpenClipboard(IntPtr.Zero)) return true;
            Thread.Sleep(5);
        }
        return false;
    }

    public static string GetClipboardText()
    {
        lock (ClipboardLock)
        {
            if (!OpenClipboard()) return "";
            try
            {
                const uint CF_UNICODETEXT = 13;
                if (!Win32.IsClipboardFormatAvailable(CF_UNICODETEXT)) return "";
                IntPtr hData = Win32.GetClipboardData(CF_UNICODETEXT);
                if (hData == IntPtr.Zero) return "";
                IntPtr pText = Win32.GlobalLock(hData);
                if (pText == IntPtr.Zero) return "";
                try { return Marshal.PtrToStringUni(pText) ?? ""; }
                finally { Win32.GlobalUnlock(hData); }
            }
            finally { Win32.CloseClipboard(); }
        }
    }

    public static bool SetClipboardText(string text)
    {
        lock (ClipboardLock)
        {
            if (!OpenClipboard()) return false;
            try
            {
                Win32.EmptyClipboard();
                int byteLen = (text.Length + 1) * 2;
                IntPtr hMem = Win32.GlobalAlloc(0x0002, (UIntPtr)byteLen);
                if (hMem == IntPtr.Zero) return false;
                IntPtr pMem = Win32.GlobalLock(hMem);
                if (pMem == IntPtr.Zero) { Win32.GlobalFree(hMem); return false; }
                Marshal.Copy(text.ToCharArray(), 0, pMem, text.Length);
                Marshal.WriteInt16(pMem, text.Length * 2, 0);
                Win32.GlobalUnlock(hMem);
                const uint CF_UNICODETEXT = 13;
                if (Win32.SetClipboardData(CF_UNICODETEXT, hMem) != IntPtr.Zero)
                    return true;
                Win32.GlobalFree(hMem);
                return false;
            }
            finally { Win32.CloseClipboard(); }
        }
    }

    public static bool TerminateProcess(int pid)
    {
        IntPtr h = Win32.OpenProcess(0x0001, false, pid);
        if (h == IntPtr.Zero) return false;
        bool ret = Win32.TerminateProcess(h, -1);
        Win32.CloseHandle(h);
        return ret;
    }

    public static void SetProcessDpiAwareness(int dpiAwareness)
    {
        try { Win32.SetProcessDpiAwareness(dpiAwareness); }
        catch { /* ignore */ }
    }

    // ── UIA Condition Helpers ──

    public static IUIAutomationCondition CreateTrueCondition() =>
        AutomationClient.Instance.Automation.CreateTrueCondition();

    public static IUIAutomationCondition CreateFalseCondition() =>
        AutomationClient.Instance.Automation.CreateFalseCondition();

    public static IUIAutomationCondition CreatePropertyCondition(
        int propertyId, object value) =>
        AutomationClient.Instance.Automation.CreatePropertyCondition(propertyId, value);

    public static IUIAutomationCondition CreateAndCondition(
        IUIAutomationCondition c1, IUIAutomationCondition c2) =>
        AutomationClient.Instance.Automation.CreateAndCondition(c1, c2);

    public static IUIAutomationCondition CreateOrCondition(
        IUIAutomationCondition c1, IUIAutomationCondition c2) =>
        AutomationClient.Instance.Automation.CreateOrCondition(c1, c2);

    public static IUIAutomationCondition CreateNotCondition(
        IUIAutomationCondition condition) =>
        AutomationClient.Instance.Automation.CreateNotCondition(condition);

    public static CacheRequest CreateCacheRequest() => new();

    public static void RemoveAllEventHandlers() =>
        AutomationClient.Instance.Automation.RemoveAllEventHandlers();

    public static Control GetRootControl() =>
        new(element: AutomationClient.Instance.Automation.GetRootElement());

    public static Control? GetFocusedControl()
    {
        try
        {
            var e = AutomationClient.Instance.Automation.GetFocusedElement();
            return e != null ? Control.CreateControlFromElement(e) : null;
        }
        catch { return null; }
    }

    public static Control? ControlFromHandle(IntPtr handle)
    {
        try
        {
            var e = AutomationClient.Instance.Automation.ElementFromHandle(handle);
            return e != null ? Control.CreateControlFromElement(e) : null;
        }
        catch { return null; }
    }

    public static Control? ControlFromPoint(int x, int y)
    {
        try
        {
            var e = AutomationClient.Instance.Automation
                .ElementFromPoint(new tagPOINT(x, y));
            return e != null ? Control.CreateControlFromElement(e) : null;
        }
        catch { return null; }
    }

    public static Control? ControlFromCursor()
    {
        var (x, y) = GetCursorPos();
        return ControlFromPoint(x, y);
    }
}

// ──────────────────────────────────────────────
// Control class
// ──────────────────────────────────────────────

public class Control
{
    private IUIAutomationElement? _element;
    private readonly bool _elementDirectAssign;
    private readonly Dictionary<int, object> _supportedPatterns = new();

    public Control? SearchFromControl { get; set; }
    public int SearchDepth { get; set; }
    public double SearchInterval { get; set; }
    public int FoundIndex { get; set; }
    public Dictionary<string, object?> SearchProperties { get; } = new();

    public static readonly HashSet<string> ValidKeys = new()
    {
        "ControlType", "ClassName", "AutomationId", "Name",
        "SubName", "RegexName", "Depth", "Compare"
    };

    public Control(
        Control? searchFromControl = null,
        int searchDepth = 0x7FFFFFFF,
        double searchInterval = UiaConstants.SearchInterval,
        int foundIndex = 1,
        IUIAutomationElement? element = null,
        int? controlType = null,
        string? name = null,
        string? subName = null,
        string? regexName = null,
        string? className = null,
        string? automationId = null,
        int? processId = null,
        int? depth = null,
        Func<Control, int, bool>? compare = null)
    {
        _element = element;
        _elementDirectAssign = element != null;
        SearchFromControl = searchFromControl;
        SearchDepth = depth ?? searchDepth;
        SearchInterval = searchInterval;
        FoundIndex = foundIndex;

        if (name != null) SearchProperties["Name"] = name;
        if (subName != null) SearchProperties["SubName"] = subName;
        if (regexName != null) SearchProperties["RegexName"] = regexName;
        if (className != null) SearchProperties["ClassName"] = className;
        if (automationId != null) SearchProperties["AutomationId"] = automationId;
        if (controlType.HasValue) SearchProperties["ControlType"] = controlType.Value;
        if (processId.HasValue) SearchProperties["ProcessId"] = processId.Value;
        if (depth.HasValue) SearchProperties["Depth"] = depth.Value;
        if (compare != null) SearchProperties["Compare"] = compare;
    }

    public IUIAutomationElement Element
    {
        get
        {
            if (_element == null)
                Refind(UiaConstants.TimeOutSecond, SearchInterval);
            return _element!;
        }
    }

    public static Control? CreateControlFromElement(IUIAutomationElement? element) =>
        element != null ? new Control(element: element) : null;

    // ── Current Properties ──

    public string AcceleratorKey => Element.CurrentAcceleratorKey;
    public string AccessKey => Element.CurrentAccessKey;

    public string? AutomationId
    {
        get
        {
            try { return Element.CurrentAutomationId; }
            catch { return SearchProperties.TryGetValue("AutomationId", out var v) ? v as string : null; }
        }
    }

    public Rect BoundingRectangle
    {
        get
        {
            var r = Element.CurrentBoundingRectangle;
            return new Rect(r.left, r.top, r.right, r.bottom);
        }
    }

    public string? ClassName
    {
        get
        {
            try { return Element.CurrentClassName; }
            catch { return SearchProperties.TryGetValue("ClassName", out var v) ? v as string : null; }
        }
    }

    public int? ControlTypeId
    {
        get
        {
            try { return Element.CurrentControlType; }
            catch { return SearchProperties.TryGetValue("ControlType", out var v) ? v as int? : null; }
        }
    }

    public string ControlTypeName =>
        ControlTypeId.HasValue && ControlTypeNames.Names.TryGetValue(ControlTypeId.Value, out string? n)
            ? n : "UnknownControl";

    public int Culture => Element.CurrentCulture;
    public string FrameworkId => Element.CurrentFrameworkId;
    public bool HasKeyboardFocus => Element.CurrentHasKeyboardFocus != 0;
    public string HelpText => Element.CurrentHelpText;
    public bool IsContentElement => Element.CurrentIsContentElement != 0;
    public bool IsControlElement => Element.CurrentIsControlElement != 0;
    public bool IsEnabled => Element.CurrentIsEnabled != 0;
    public bool IsKeyboardFocusable => Element.CurrentIsKeyboardFocusable != 0;
    public bool IsOffscreen => Element.CurrentIsOffscreen != 0;
    public bool IsPassword => Element.CurrentIsPassword != 0;
    public bool IsRequiredForForm => Element.CurrentIsRequiredForForm != 0;
    public string ItemStatus => Element.CurrentItemStatus;
    public string ItemType => Element.CurrentItemType;
    public string LocalizedControlType => Element.CurrentLocalizedControlType;

    public string? Name
    {
        get
        {
            try { return Element.CurrentName ?? ""; }
            catch { return SearchProperties.TryGetValue("Name", out var v) ? v as string : null; }
        }
    }

    public IntPtr NativeWindowHandle
    {
        get
        {
            try { return Element.CurrentNativeWindowHandle; }
            catch { return IntPtr.Zero; }
        }
    }

    public int Orientation => Element.CurrentOrientation;

    public int? ProcessId
    {
        get
        {
            try { return Element.CurrentProcessId; }
            catch { return SearchProperties.TryGetValue("ProcessId", out var v) ? v as int? : null; }
        }
    }

    public string ProviderDescription => Element.CurrentProviderDescription;

    public string? RegexName =>
        SearchProperties.TryGetValue("RegexName", out var v) ? v as string : null;

    // ── Cached Properties ──

    public string CachedName => Element.CachedName;
    public string CachedAutomationId => Element.CachedAutomationId;
    public string CachedClassName => Element.CachedClassName;
    public int CachedControlTypeId => Element.CachedControlType;
    public string CachedControlTypeName =>
        ControlTypeNames.Names.TryGetValue(CachedControlTypeId, out var n) ? n : "UnknownControl";
    public string CachedLocalizedControlType => Element.CachedLocalizedControlType;
    public string CachedAcceleratorKey => Element.CachedAcceleratorKey;
    public int CachedProcessId => Element.CachedProcessId;
    public bool CachedIsEnabled => Element.CachedIsEnabled != 0;
    public bool CachedIsOffscreen => Element.CachedIsOffscreen != 0;
    public bool CachedIsControlElement => Element.CachedIsControlElement != 0;
    public bool CachedHasKeyboardFocus => Element.CachedHasKeyboardFocus != 0;
    public bool CachedIsKeyboardFocusable => Element.CachedIsKeyboardFocusable != 0;

    public Rect CachedBoundingRectangle
    {
        get
        {
            var r = Element.CachedBoundingRectangle;
            return new Rect(r.left, r.top, r.right, r.bottom);
        }
    }

    // ── Element Methods ──

    public bool SetFocus()
    {
        try { Element.SetFocus(); return true; }
        catch { return false; }
    }

    public int[] GetRuntimeId() => Element.GetRuntimeId();

    public (int x, int y, bool gotClickable) GetClickablePoint()
    {
        Element.GetClickablePoint(out tagPOINT pt, out int got);
        return (pt.x, pt.y, got != 0);
    }

    public object? GetCurrentPropertyValue(int propertyId)
    {
        try { return Element.GetCurrentPropertyValue(propertyId); }
        catch { return null; }
    }

    /// <summary>
    /// Gets the specified UI Automation pattern for this control.
    /// Returns the pattern ID if the control supports it.
    /// </summary>
    public int GetPattern(int patternId)
    {
        try
        {
            var pattern = Element.GetCurrentPattern(patternId);
            if (pattern != null)
                _supportedPatterns[patternId] = pattern;
        }
        catch { /* ignore */ }
        return patternId;
    }

    public object? GetPatternObject(int patternId)
    {
        try
        {
            var pattern = Element.GetCurrentPattern(patternId);
            if (pattern != null)
            {
                _supportedPatterns[patternId] = pattern;
                return pattern;
            }
        }
        catch { /* ignore */ }
        return null;
    }

    // ── FindFirst / FindAll ──

    public Control? FindFirst(int scope, IUIAutomationCondition condition) =>
        CreateControlFromElement(Element.FindFirst(scope, condition));

    public List<Control> FindAll(int scope, IUIAutomationCondition condition) =>
        ElementArrayToList(Element.FindAll(scope, condition));

    public Control? FindFirstBuildCache(int scope,
        IUIAutomationCondition condition, CacheRequest cacheRequest) =>
        CreateControlFromElement(Element.FindFirstBuildCache(
            scope, condition, cacheRequest.ComCacheRequest));

    public List<Control> FindAllBuildCache(int scope,
        IUIAutomationCondition condition, CacheRequest cacheRequest) =>
        ElementArrayToList(Element.FindAllBuildCache(
            scope, condition, cacheRequest.ComCacheRequest));

    public Control BuildUpdatedCache(CacheRequest cacheRequest) =>
        CreateControlFromElement(Element.BuildUpdatedCache(
            cacheRequest.ComCacheRequest))!;

    // ── Navigation ──

    public Control? GetParentControl() =>
        CreateControlFromElement(
            AutomationClient.Instance.ViewWalker.GetParentElement(Element));

    public Control? GetFirstChildControl() =>
        CreateControlFromElement(
            AutomationClient.Instance.ViewWalker.GetFirstChildElement(Element));

    public Control? GetLastChildControl() =>
        CreateControlFromElement(
            AutomationClient.Instance.ViewWalker.GetLastChildElement(Element));

    public Control? GetNextSiblingControl() =>
        CreateControlFromElement(
            AutomationClient.Instance.ViewWalker.GetNextSiblingElement(Element));

    public Control? GetPreviousSiblingControl() =>
        CreateControlFromElement(
            AutomationClient.Instance.ViewWalker.GetPreviousSiblingElement(Element));

    public List<Control> GetChildren()
    {
        var children = new List<Control>();
        var child = GetFirstChildControl();
        while (child != null)
        {
            children.Add(child);
            child = child.GetNextSiblingControl();
        }
        return children;
    }

    public Control? GetAncestorControl(Func<Control, int, bool> condition)
    {
        Control? ancestor = this;
        int depth = 0;
        while (true)
        {
            ancestor = ancestor!.GetParentControl();
            depth--;
            if (ancestor == null) return null;
            if (condition(ancestor, depth)) return ancestor;
        }
    }

    public Control? GetSiblingControl(
        Func<Control, bool> condition, bool forward = true)
    {
        if (!forward)
        {
            Control? prev = this;
            while (true)
            {
                prev = prev!.GetPreviousSiblingControl();
                if (prev == null) break;
                if (condition(prev)) return prev;
            }
        }
        Control? next = this;
        while (true)
        {
            next = next!.GetNextSiblingControl();
            if (next == null) break;
            if (condition(next)) return next;
        }
        return null;
    }

    public List<Control> GetCachedChildren()
    {
        try { return ElementArrayToList(Element.GetCachedChildren()); }
        catch { return new List<Control>(); }
    }

    public Control? GetCachedParent()
    {
        try { return CreateControlFromElement(Element.GetCachedParent()); }
        catch { return null; }
    }

    // ── Search / Refind ──

    public void Refind(double maxSearchSeconds = UiaConstants.TimeOutSecond,
        double searchIntervalSeconds = UiaConstants.SearchInterval)
    {
        var from = SearchFromControl?.Element
            ?? AutomationClient.Instance.Automation.GetRootElement();
        IUIAutomationCondition condition;

        if (SearchProperties.TryGetValue("ControlType", out var ctObj)
            && ctObj is int ct)
        {
            condition = AutomationClient.Instance.Automation
                .CreatePropertyCondition(ControlTypeProperty, ct);

            if (SearchProperties.TryGetValue("Name", out var nObj) && nObj is string nv)
                condition = AutomationClient.Instance.Automation.CreateAndCondition(
                    condition, AutomationClient.Instance.Automation
                        .CreatePropertyCondition(NameProperty, nv));

            if (SearchProperties.TryGetValue("AutomationId", out var aObj) && aObj is string av)
                condition = AutomationClient.Instance.Automation.CreateAndCondition(
                    condition, AutomationClient.Instance.Automation
                        .CreatePropertyCondition(AutomationIdProperty, av));

            if (SearchProperties.TryGetValue("ClassName", out var cObj) && cObj is string cv)
                condition = AutomationClient.Instance.Automation.CreateAndCondition(
                    condition, AutomationClient.Instance.Automation
                        .CreatePropertyCondition(ClassNameProperty, cv));
        }
        else if (SearchProperties.TryGetValue("Name", out var n2) && n2 is string nv2)
        {
            condition = AutomationClient.Instance.Automation
                .CreatePropertyCondition(NameProperty, nv2);
        }
        else if (SearchProperties.TryGetValue("AutomationId", out var a2) && a2 is string av2)
        {
            condition = AutomationClient.Instance.Automation
                .CreatePropertyCondition(AutomationIdProperty, av2);
        }
        else
        {
            condition = AutomationClient.Instance.Automation.CreateTrueCondition();
        }

        long endTime = Environment.TickCount64 + (long)(maxSearchSeconds * 1000);
        while (Environment.TickCount64 < endTime)
        {
            var element = from.FindFirst(TreeScope_Descendants, condition);
            if (element != null) { _element = element; return; }
            Thread.Sleep((int)(searchIntervalSeconds * 1000));
        }
        throw new InvalidOperationException(
            $"Control not found within {maxSearchSeconds}s. " +
            $"Search properties: {GetSearchPropertiesStr()}");
    }

    public bool Exists(double maxSearchSeconds = UiaConstants.TimeOutSecond,
        double searchIntervalSeconds = UiaConstants.SearchInterval)
    {
        try { Refind(maxSearchSeconds, searchIntervalSeconds); return true; }
        catch { return false; }
    }

    // ── Click / Input ──

    public void Click(double waitTime = UiaConstants.OperationWaitTime)
    {
        var r = BoundingRectangle;
        UiaHelpers.Click(r.XCenter(), r.YCenter(), waitTime);
    }

    public void RightClick(double waitTime = UiaConstants.OperationWaitTime)
    {
        var r = BoundingRectangle;
        UiaHelpers.RightClick(r.XCenter(), r.YCenter(), waitTime);
    }

    public void MiddleClick(double waitTime = UiaConstants.OperationWaitTime)
    {
        var r = BoundingRectangle;
        UiaHelpers.MiddleClick(r.XCenter(), r.YCenter(), waitTime);
    }

    public void MoveCursorTo(int ratioX = 50, int ratioY = 50,
        double waitTime = UiaConstants.OperationWaitTime)
    {
        var r = BoundingRectangle;
        UiaHelpers.SetCursorPos(
            r.Left + r.Width() * ratioX / 100,
            r.Top + r.Height() * ratioY / 100);
        Thread.Sleep((int)(waitTime * 1000));
    }

    public void SendKeys(string text, double interval = 0.01,
        double waitTime = UiaConstants.OperationWaitTime, bool charMode = true)
    {
        SetFocus();
        UiaHelpers.SendKeys(text, interval, waitTime, charMode);
    }

    // ── String representation ──

    public string GetSearchPropertiesStr()
    {
        var parts = new List<string>();
        foreach (var kv in SearchProperties)
        {
            string val = kv.Key == "ControlType"
                && kv.Value is int v && ControlTypeNames.Names.TryGetValue(v, out string? n)
                    ? n : $"'{kv.Value}'";
            parts.Add($"{kv.Key}: {val}");
        }
        return "{" + string.Join(", ", parts) + "}";
    }

    public override string ToString() =>
        $"ControlType: {ControlTypeName}    ClassName: {ClassName}    " +
        $"AutomationId: {AutomationId}    Rect: {BoundingRectangle}    " +
        $"Name: {Name}    Handle: 0x{NativeWindowHandle:X}({NativeWindowHandle})";

    private static List<Control> ElementArrayToList(IUIAutomationElementArray? arr)
    {
        if (arr == null) return new List<Control>();
        var list = new List<Control>();
        int len = arr.Length;
        for (int i = 0; i < len; i++)
        {
            var c = CreateControlFromElement(arr.GetElement(i));
            if (c != null) list.Add(c);
        }
        return list;
    }
}

#pragma warning restore CS0649
#pragma warning restore CA1069
#pragma warning restore SYSLIB1054
#pragma warning restore CA1401
