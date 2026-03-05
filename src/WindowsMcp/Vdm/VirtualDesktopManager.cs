#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable SYSLIB1054 // Use LibraryImport instead of DllImport
#pragma warning disable CS0649 // Field is never assigned to

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace WindowsMcp.Vdm;

// ──────────────────────────────────────────────
// COM Interface Declarations
// ──────────────────────────────────────────────

/// <summary>
/// Standard documented IVirtualDesktopManager COM interface.
/// </summary>
[ComImport]
[Guid("a5cd92ff-29be-454c-8d04-d82879fb3f1b")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IVirtualDesktopManagerCom
{
    [PreserveSig]
    int IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow, [MarshalAs(UnmanagedType.Bool)] out bool onCurrentDesktop);

    [PreserveSig]
    int GetWindowDesktopId(IntPtr topLevelWindow, out Guid desktopId);

    [PreserveSig]
    int MoveWindowToDesktop(IntPtr topLevelWindow, ref Guid desktopId);
}

/// <summary>
/// IServiceProvider COM interface for querying internal desktop manager.
/// </summary>
[ComImport]
[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IServiceProvider10
{
    [PreserveSig]
    int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
}

/// <summary>
/// IObjectArray COM interface for iterating desktops.
/// </summary>
[ComImport]
[Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8CC")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IObjectArray
{
    [PreserveSig]
    int GetCount(out uint pcObjects);

    [PreserveSig]
    int GetAt(uint uiIndex, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);
}

// ──────────────────────────────────────────────
// Internal (undocumented) COM Interfaces
// ──────────────────────────────────────────────

/// <summary>
/// IVirtualDesktop internal COM interface (Windows 11 22621+).
/// VTable order: IsViewVisible, GetID, GetName, GetWallpaperPath, IsRemote.
/// </summary>
[ComImport]
[Guid("3F07F4BE-B107-441A-AF0F-39D82529072C")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IVirtualDesktop
{
    [PreserveSig]
    int IsViewVisible([MarshalAs(UnmanagedType.IUnknown)] object pView, out uint pfVisible);

    [PreserveSig]
    int GetID(out Guid pGuid);

    [PreserveSig]
    int GetName(out IntPtr pName); // HSTRING

    [PreserveSig]
    int GetWallpaperPath(out IntPtr pPath); // HSTRING

    [PreserveSig]
    int IsRemote(out IntPtr pW);
}

/// <summary>
/// IVirtualDesktop for Windows 10 (Build 19041+), different IID.
/// Same vtable layout as the 22621+ version.
/// </summary>
[ComImport]
[Guid("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IVirtualDesktopWin10
{
    [PreserveSig]
    int IsViewVisible([MarshalAs(UnmanagedType.IUnknown)] object pView, out uint pfVisible);

    [PreserveSig]
    int GetID(out Guid pGuid);

    [PreserveSig]
    int GetName(out IntPtr pName);

    [PreserveSig]
    int GetWallpaperPath(out IntPtr pPath);

    [PreserveSig]
    int IsRemote(out IntPtr pW);
}

/// <summary>
/// IVirtualDesktopManagerInternal for Windows 11 Build 26100+.
/// VTable: GetCount, MoveViewToDesktop, CanViewMoveDesktops, GetCurrentDesktop,
///         GetDesktops, GetAdjacentDesktop, SwitchDesktop, SwitchDesktopAndMoveForegroundView,
///         CreateDesktopW, MoveDesktop, RemoveDesktop, FindDesktop,
///         GetDesktopSwitchIncludeExcludeViews, SetName.
/// </summary>
[ComImport]
[Guid("53F5CA0B-158F-4124-900C-057158060B27")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IVirtualDesktopManagerInternal26100
{
    [PreserveSig]
    int GetCount(out uint pCount);

    [PreserveSig]
    int MoveViewToDesktop([MarshalAs(UnmanagedType.IUnknown)] object pView, [MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop);

    [PreserveSig]
    int CanViewMoveDesktops([MarshalAs(UnmanagedType.IUnknown)] object pView, out uint pfCanMove);

    [PreserveSig]
    int GetCurrentDesktop([MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);

    [PreserveSig]
    int GetDesktops([MarshalAs(UnmanagedType.Interface)] out IObjectArray array);

    [PreserveSig]
    int GetAdjacentDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktopReference, uint uDirection, [MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop ppAdjacentDesktop);

    [PreserveSig]
    int SwitchDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop);

    [PreserveSig]
    int SwitchDesktopAndMoveForegroundView([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop);

    [PreserveSig]
    int CreateDesktopW([MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);

    [PreserveSig]
    int MoveDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop, uint nIndex);

    [PreserveSig]
    int RemoveDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop destroyDesktop, [MarshalAs(UnmanagedType.Interface)] IVirtualDesktop fallbackDesktop);

    [PreserveSig]
    int FindDesktop(ref Guid pGuid, [MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);

    [PreserveSig]
    int GetDesktopSwitchIncludeExcludeViews([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop, [MarshalAs(UnmanagedType.Interface)] out IObjectArray ppInclude, [MarshalAs(UnmanagedType.Interface)] out IObjectArray ppExclude);

    [PreserveSig]
    int SetName([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop, IntPtr name); // HSTRING
}

/// <summary>
/// IVirtualDesktopManagerInternal for Windows 11 Build 22621+.
/// Same vtable as 26100 but without SwitchDesktopAndMoveForegroundView and MoveDesktop.
/// VTable: GetCount, MoveViewToDesktop, CanViewMoveDesktops, GetCurrentDesktop,
///         GetDesktops, GetAdjacentDesktop, SwitchDesktop, CreateDesktopW, MoveDesktop,
///         RemoveDesktop, FindDesktop, GetDesktopSwitchIncludeExcludeViews, SetName.
/// </summary>
[ComImport]
[Guid("A3175F2D-239C-4BD2-8AA0-EEBA8B0B138E")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IVirtualDesktopManagerInternal22621
{
    [PreserveSig]
    int GetCount(out uint pCount);

    [PreserveSig]
    int MoveViewToDesktop([MarshalAs(UnmanagedType.IUnknown)] object pView, [MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop);

    [PreserveSig]
    int CanViewMoveDesktops([MarshalAs(UnmanagedType.IUnknown)] object pView, out uint pfCanMove);

    [PreserveSig]
    int GetCurrentDesktop([MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);

    [PreserveSig]
    int GetDesktops([MarshalAs(UnmanagedType.Interface)] out IObjectArray array);

    [PreserveSig]
    int GetAdjacentDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktopReference, uint uDirection, [MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop ppAdjacentDesktop);

    [PreserveSig]
    int SwitchDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop);

    [PreserveSig]
    int CreateDesktopW([MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);

    [PreserveSig]
    int MoveDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop, uint nIndex);

    [PreserveSig]
    int RemoveDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop destroyDesktop, [MarshalAs(UnmanagedType.Interface)] IVirtualDesktop fallbackDesktop);

    [PreserveSig]
    int FindDesktop(ref Guid pGuid, [MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);

    [PreserveSig]
    int GetDesktopSwitchIncludeExcludeViews([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop, [MarshalAs(UnmanagedType.Interface)] out IObjectArray ppInclude, [MarshalAs(UnmanagedType.Interface)] out IObjectArray ppExclude);

    [PreserveSig]
    int SetName([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop, IntPtr name); // HSTRING
}

/// <summary>
/// IVirtualDesktopManagerInternal for Windows 10 (Build 19041+).
/// VTable: GetCount, MoveViewToDesktop, CanViewMoveDesktops, GetCurrentDesktop,
///         GetDesktops, GetAdjacentDesktop, SwitchDesktop, CreateDesktopW,
///         RemoveDesktop, FindDesktop.
/// </summary>
[ComImport]
[Guid("F31574D6-B682-4CDC-BD56-1827860ABEC6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IVirtualDesktopManagerInternalWin10
{
    [PreserveSig]
    int GetCount(out uint pCount);

    [PreserveSig]
    int MoveViewToDesktop([MarshalAs(UnmanagedType.IUnknown)] object pView, [MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop);

    [PreserveSig]
    int CanViewMoveDesktops([MarshalAs(UnmanagedType.IUnknown)] object pView, out uint pfCanMove);

    [PreserveSig]
    int GetCurrentDesktop([MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);

    [PreserveSig]
    int GetDesktops([MarshalAs(UnmanagedType.Interface)] out IObjectArray array);

    [PreserveSig]
    int GetAdjacentDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktopReference, uint uDirection, [MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop ppAdjacentDesktop);

    [PreserveSig]
    int SwitchDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop pDesktop);

    [PreserveSig]
    int CreateDesktopW([MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);

    [PreserveSig]
    int RemoveDesktop([MarshalAs(UnmanagedType.Interface)] IVirtualDesktop destroyDesktop, [MarshalAs(UnmanagedType.Interface)] IVirtualDesktop fallbackDesktop);

    [PreserveSig]
    int FindDesktop(ref Guid pGuid, [MarshalAs(UnmanagedType.Interface)] out IVirtualDesktop pDesktop);
}

// ──────────────────────────────────────────────
// CLSIDs and IIDs
// ──────────────────────────────────────────────

public static class VdmGuids
{
    public static readonly Guid CLSID_VirtualDesktopManager = new("aa509086-5ca9-4c25-8f95-589d3c07b48a");
    public static readonly Guid CLSID_ImmersiveShell = new("C2F03A33-21F5-47FA-B4BB-156362A2F239");
    public static readonly Guid CLSID_VirtualDesktopManagerInternal = new("C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B");

    // IIDs per build
    public static readonly Guid IID_IVirtualDesktopManagerInternal_26100 = new("53F5CA0B-158F-4124-900C-057158060B27");
    public static readonly Guid IID_IVirtualDesktopManagerInternal_22621 = new("A3175F2D-239C-4BD2-8AA0-EEBA8B0B138E");
    public static readonly Guid IID_IVirtualDesktopManagerInternal_Win10 = new("F31574D6-B682-4CDC-BD56-1827860ABEC6");

    public static readonly Guid IID_IVirtualDesktop = new("3F07F4BE-B107-441A-AF0F-39D82529072C");
    public static readonly Guid IID_IVirtualDesktopWin10 = new("FF72FFDD-BE7E-43FC-9C03-AD81681E88E4");
}

// ──────────────────────────────────────────────
// HSTRING helpers (P/Invoke)
// ──────────────────────────────────────────────

public static class HStringInterop
{
    [DllImport("combase.dll", PreserveSig = true)]
    private static extern int WindowsCreateString(
        [MarshalAs(UnmanagedType.LPWStr)] string sourceString,
        uint length,
        out IntPtr hstring);

    [DllImport("combase.dll", PreserveSig = true)]
    private static extern int WindowsDeleteString(IntPtr hstring);

    [DllImport("combase.dll", PreserveSig = true)]
    private static extern IntPtr WindowsGetStringRawBuffer(IntPtr hstring, out uint length);

    private static bool _available = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static IntPtr Create(string text)
    {
        if (!_available)
            return IntPtr.Zero;

        int hr = WindowsCreateString(text, (uint)text.Length, out IntPtr hs);
        if (hr != 0)
            throw new COMException($"WindowsCreateString failed", hr);
        return hs;
    }

    public static void Delete(IntPtr hs)
    {
        if (_available && hs != IntPtr.Zero)
            WindowsDeleteString(hs);
    }

    public static string? GetString(IntPtr hs)
    {
        if (!_available || hs == IntPtr.Zero)
            return null;

        IntPtr rawBuffer = WindowsGetStringRawBuffer(hs, out uint length);
        if (rawBuffer == IntPtr.Zero || length == 0)
            return null;
        return Marshal.PtrToStringUni(rawBuffer, (int)length);
    }
}

// ──────────────────────────────────────────────
// Desktop info DTO
// ──────────────────────────────────────────────

public class DesktopInfo
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}

// ──────────────────────────────────────────────
// Internal manager adapter (build-agnostic)
// ──────────────────────────────────────────────

/// <summary>
/// Wraps the build-specific IVirtualDesktopManagerInternal COM interfaces
/// behind a common abstraction.
/// </summary>
internal interface IInternalManagerAdapter
{
    int GetCount(out uint pCount);
    int GetCurrentDesktop(out IVirtualDesktop pDesktop);
    int GetDesktops(out IObjectArray array);
    int SwitchDesktop(IVirtualDesktop pDesktop);
    int CreateDesktopW(out IVirtualDesktop pDesktop);
    int RemoveDesktop(IVirtualDesktop destroyDesktop, IVirtualDesktop fallbackDesktop);
    int FindDesktop(ref Guid pGuid, out IVirtualDesktop pDesktop);
    int SetName(IVirtualDesktop pDesktop, IntPtr name);
    bool SupportsSetName { get; }
}

internal sealed class InternalManagerAdapter26100 : IInternalManagerAdapter
{
    private readonly IVirtualDesktopManagerInternal26100 _inner;
    public InternalManagerAdapter26100(IVirtualDesktopManagerInternal26100 inner) => _inner = inner;
    public bool SupportsSetName => true;

    public int GetCount(out uint pCount) => _inner.GetCount(out pCount);
    public int GetCurrentDesktop(out IVirtualDesktop pDesktop) => _inner.GetCurrentDesktop(out pDesktop);
    public int GetDesktops(out IObjectArray array) => _inner.GetDesktops(out array);
    public int SwitchDesktop(IVirtualDesktop pDesktop) => _inner.SwitchDesktop(pDesktop);
    public int CreateDesktopW(out IVirtualDesktop pDesktop) => _inner.CreateDesktopW(out pDesktop);
    public int RemoveDesktop(IVirtualDesktop destroyDesktop, IVirtualDesktop fallbackDesktop) => _inner.RemoveDesktop(destroyDesktop, fallbackDesktop);
    public int FindDesktop(ref Guid pGuid, out IVirtualDesktop pDesktop) => _inner.FindDesktop(ref pGuid, out pDesktop);
    public int SetName(IVirtualDesktop pDesktop, IntPtr name) => _inner.SetName(pDesktop, name);
}

internal sealed class InternalManagerAdapter22621 : IInternalManagerAdapter
{
    private readonly IVirtualDesktopManagerInternal22621 _inner;
    public InternalManagerAdapter22621(IVirtualDesktopManagerInternal22621 inner) => _inner = inner;
    public bool SupportsSetName => true;

    public int GetCount(out uint pCount) => _inner.GetCount(out pCount);
    public int GetCurrentDesktop(out IVirtualDesktop pDesktop) => _inner.GetCurrentDesktop(out pDesktop);
    public int GetDesktops(out IObjectArray array) => _inner.GetDesktops(out array);
    public int SwitchDesktop(IVirtualDesktop pDesktop) => _inner.SwitchDesktop(pDesktop);
    public int CreateDesktopW(out IVirtualDesktop pDesktop) => _inner.CreateDesktopW(out pDesktop);
    public int RemoveDesktop(IVirtualDesktop destroyDesktop, IVirtualDesktop fallbackDesktop) => _inner.RemoveDesktop(destroyDesktop, fallbackDesktop);
    public int FindDesktop(ref Guid pGuid, out IVirtualDesktop pDesktop) => _inner.FindDesktop(ref pGuid, out pDesktop);
    public int SetName(IVirtualDesktop pDesktop, IntPtr name) => _inner.SetName(pDesktop, name);
}

internal sealed class InternalManagerAdapterWin10 : IInternalManagerAdapter
{
    private readonly IVirtualDesktopManagerInternalWin10 _inner;
    public InternalManagerAdapterWin10(IVirtualDesktopManagerInternalWin10 inner) => _inner = inner;
    public bool SupportsSetName => false;

    public int GetCount(out uint pCount) => _inner.GetCount(out pCount);
    public int GetDesktops(out IObjectArray array) => _inner.GetDesktops(out array);
    public int SwitchDesktop(IVirtualDesktop pDesktop) => _inner.SwitchDesktop(pDesktop);
    public int CreateDesktopW(out IVirtualDesktop pDesktop) => _inner.CreateDesktopW(out pDesktop);
    public int RemoveDesktop(IVirtualDesktop destroyDesktop, IVirtualDesktop fallbackDesktop) => _inner.RemoveDesktop(destroyDesktop, fallbackDesktop);
    public int FindDesktop(ref Guid pGuid, out IVirtualDesktop pDesktop) => _inner.FindDesktop(ref pGuid, out pDesktop);

    public int GetCurrentDesktop(out IVirtualDesktop pDesktop)
    {
        // Win10 interface returns the same IVirtualDesktop vtable but with a different IID.
        // The cast works because the vtable layout is identical.
        return _inner.GetCurrentDesktop(out pDesktop);
    }

    public int SetName(IVirtualDesktop pDesktop, IntPtr name)
    {
        // Not supported on Windows 10
        pDesktop = null!;
        return unchecked((int)0x80004001); // E_NOTIMPL
    }
}

// ──────────────────────────────────────────────
// VirtualDesktopManager
// ──────────────────────────────────────────────

/// <summary>
/// Wrapper around Windows IVirtualDesktopManager and internal interfaces.
/// Provides methods for checking if a window is on the current desktop,
/// getting/setting desktop information, and managing virtual desktops.
/// </summary>
public sealed class VirtualDesktopManager
{
    private static readonly ILogger Logger = LoggerFactory
        .Create(builder => builder.AddConsole())
        .CreateLogger<VirtualDesktopManager>();

    private static readonly ThreadLocal<VirtualDesktopManager> _threadLocal = new(() => new VirtualDesktopManager());

    private IVirtualDesktopManagerCom? _manager;
    private IInternalManagerAdapter? _internalManager;

    private static readonly DesktopInfo DefaultDesktop = new()
    {
        Id = "00000000-0000-0000-0000-000000000000",
        Name = "Default Desktop"
    };

    public VirtualDesktopManager()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        try
        {
            var managerType = Type.GetTypeFromCLSID(VdmGuids.CLSID_VirtualDesktopManager);
            if (managerType != null)
            {
                var obj = Activator.CreateInstance(managerType);
                _manager = (IVirtualDesktopManagerCom?)obj;
            }

            InitializeInternalManager();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to initialize VirtualDesktopManager");
        }
    }

    private void InitializeInternalManager()
    {
        try
        {
            var shellType = Type.GetTypeFromCLSID(VdmGuids.CLSID_ImmersiveShell);
            if (shellType == null) return;

            var shell = Activator.CreateInstance(shellType);
            if (shell is not IServiceProvider10 serviceProvider) return;

            var guidService = VdmGuids.CLSID_VirtualDesktopManagerInternal;
            int build = GetWindowsBuild();

            if (build >= 26100)
            {
                var riid = VdmGuids.IID_IVirtualDesktopManagerInternal_26100;
                int hr = serviceProvider.QueryService(ref guidService, ref riid, out var ppv);
                if (hr == 0 && ppv is IVirtualDesktopManagerInternal26100 mgr)
                    _internalManager = new InternalManagerAdapter26100(mgr);
            }
            else if (build >= 22621)
            {
                var riid = VdmGuids.IID_IVirtualDesktopManagerInternal_22621;
                int hr = serviceProvider.QueryService(ref guidService, ref riid, out var ppv);
                if (hr == 0 && ppv is IVirtualDesktopManagerInternal22621 mgr)
                    _internalManager = new InternalManagerAdapter22621(mgr);
            }
            else
            {
                var riid = VdmGuids.IID_IVirtualDesktopManagerInternal_Win10;
                int hr = serviceProvider.QueryService(ref guidService, ref riid, out var ppv);
                if (hr == 0 && ppv is IVirtualDesktopManagerInternalWin10 mgr)
                    _internalManager = new InternalManagerAdapterWin10(mgr);
            }
        }
        catch (Exception e)
        {
            Logger.LogWarning(e, "Failed to initialize VirtualDesktopManagerInternal");
        }
    }

    private static int GetWindowsBuild()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return 0;
        return Environment.OSVersion.Version.Build;
    }

    // ──────────────────────────────────────────
    // Public API — documented COM interface
    // ──────────────────────────────────────────

    /// <summary>
    /// Checks if the specified window is on the currently active virtual desktop.
    /// </summary>
    public bool IsWindowOnCurrentDesktop(IntPtr hwnd)
    {
        if (_manager == null)
            return true; // Fail open

        try
        {
            int hr = _manager.IsWindowOnCurrentVirtualDesktop(hwnd, out bool onCurrent);
            return hr == 0 ? onCurrent : true;
        }
        catch
        {
            return true; // Fail open
        }
    }

    /// <summary>
    /// Returns the GUID (as a string) of the virtual desktop the window is on.
    /// </summary>
    public string GetWindowDesktopId(IntPtr hwnd)
    {
        if (_manager == null)
            return "";

        try
        {
            int hr = _manager.GetWindowDesktopId(hwnd, out Guid desktopId);
            return hr == 0 ? desktopId.ToString() : "";
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// Moves a window to the specified virtual desktop (by name or GUID).
    /// </summary>
    public void MoveWindowToDesktop(IntPtr hwnd, string desktopName)
    {
        if (_manager == null) return;

        try
        {
            var targetGuidStr = ResolveToGuid(desktopName);
            if (targetGuidStr == null)
            {
                Logger.LogError("Desktop '{DesktopName}' not found", desktopName);
                return;
            }

            var guid = Guid.Parse(targetGuidStr);
            _manager.MoveWindowToDesktop(hwnd, ref guid);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to move window to desktop");
        }
    }

    // ──────────────────────────────────────────
    // Public API — internal COM interface
    // ──────────────────────────────────────────

    /// <summary>
    /// Returns info about the current virtual desktop.
    /// </summary>
    public DesktopInfo GetCurrentDesktop()
    {
        if (_internalManager == null)
            return DefaultDesktop;

        int hr = _internalManager.GetCurrentDesktop(out var currentDesktop);
        if (hr != 0 || currentDesktop == null)
            return DefaultDesktop;

        currentDesktop.GetID(out Guid guid);
        var guidStr = guid.ToString();

        var allDesktops = GetAllDesktops();
        foreach (var d in allDesktops)
        {
            if (string.Equals(d.Id, guidStr, StringComparison.OrdinalIgnoreCase))
                return d;
        }

        return new DesktopInfo { Id = guidStr, Name = "Unknown" };
    }

    /// <summary>
    /// Returns a list of all virtual desktops.
    /// </summary>
    public List<DesktopInfo> GetAllDesktops()
    {
        if (_internalManager == null)
            return [DefaultDesktop];

        int hr = _internalManager.GetDesktops(out var desktopsArray);
        if (hr != 0 || desktopsArray == null)
            return [DefaultDesktop];

        desktopsArray.GetCount(out uint count);
        var iidDesktop = VdmGuids.IID_IVirtualDesktop;

        var result = new List<DesktopInfo>((int)count);
        for (uint i = 0; i < count; i++)
        {
            try
            {
                hr = desktopsArray.GetAt(i, ref iidDesktop, out var unk);
                if (hr != 0 || unk is not IVirtualDesktop desktop)
                    continue;

                desktop.GetID(out Guid guid);
                var guidStr = guid.ToString();

                var regName = GetNameFromRegistry(guidStr);
                var name = regName ?? $"Desktop {i + 1}";

                result.Add(new DesktopInfo { Id = guidStr, Name = name });
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error retrieving desktop at index {Index}", i);
            }
        }

        return result;
    }

    /// <summary>
    /// Creates a new virtual desktop and returns its name.
    /// </summary>
    public string CreateDesktop(string? name = null)
    {
        if (_internalManager == null)
            throw new InvalidOperationException("Internal VDM not initialized");

        int hr = _internalManager.CreateDesktopW(out var desktop);
        Marshal.ThrowExceptionForHR(hr);

        desktop.GetID(out Guid guid);
        var guidStr = guid.ToString();

        if (name != null)
        {
            RenameDesktopByGuid(guidStr, name);
            return name;
        }

        var desktops = GetAllDesktops();
        return desktops.Count > 0 ? desktops[^1].Name : guidStr;
    }

    /// <summary>
    /// Removes a virtual desktop by name.
    /// </summary>
    public void RemoveDesktop(string desktopName)
    {
        if (_internalManager == null)
            throw new InvalidOperationException("Internal VDM not initialized");

        var targetGuidStr = ResolveToGuid(desktopName);
        if (targetGuidStr == null)
        {
            Logger.LogError("Desktop '{DesktopName}' not found", desktopName);
            return;
        }

        var targetGuid = Guid.Parse(targetGuidStr);
        int hr = _internalManager.FindDesktop(ref targetGuid, out var targetDesktop);
        if (hr != 0 || targetDesktop == null)
        {
            Logger.LogError("Could not find desktop with GUID {Guid}", targetGuidStr);
            return;
        }

        // Find a fallback desktop
        hr = _internalManager.GetDesktops(out var desktopsArray);
        if (hr != 0 || desktopsArray == null) return;

        desktopsArray.GetCount(out uint count);
        var iidDesktop = VdmGuids.IID_IVirtualDesktop;
        IVirtualDesktop? fallbackDesktop = null;

        for (uint i = 0; i < count; i++)
        {
            hr = desktopsArray.GetAt(i, ref iidDesktop, out var unk);
            if (hr != 0 || unk is not IVirtualDesktop candidate) continue;

            candidate.GetID(out Guid candidateId);
            if (candidateId != targetGuid)
            {
                fallbackDesktop = candidate;
                break;
            }
        }

        if (fallbackDesktop == null)
        {
            Logger.LogError("No fallback desktop found (cannot delete the only desktop)");
            return;
        }

        _internalManager.RemoveDesktop(targetDesktop, fallbackDesktop);
    }

    /// <summary>
    /// Renames a virtual desktop (identified by current name).
    /// </summary>
    public void RenameDesktop(string desktopName, string newName)
    {
        var targetGuidStr = ResolveToGuid(desktopName);
        if (targetGuidStr == null)
        {
            Logger.LogError("Desktop '{DesktopName}' not found", desktopName);
            return;
        }

        RenameDesktopByGuid(targetGuidStr, newName);
    }

    /// <summary>
    /// Switches to the specified virtual desktop (by name).
    /// </summary>
    public void SwitchDesktop(string desktopName)
    {
        if (_internalManager == null)
            throw new InvalidOperationException("Internal VDM not initialized");

        var targetGuidStr = ResolveToGuid(desktopName);
        if (targetGuidStr == null)
        {
            Logger.LogError("Desktop '{DesktopName}' not found", desktopName);
            return;
        }

        var targetGuid = Guid.Parse(targetGuidStr);
        try
        {
            int hr = _internalManager.FindDesktop(ref targetGuid, out var targetDesktop);
            if (hr == 0 && targetDesktop != null)
                _internalManager.SwitchDesktop(targetDesktop);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to switch desktop");
        }
    }

    // ──────────────────────────────────────────
    // Private helpers
    // ──────────────────────────────────────────

    private void RenameDesktopByGuid(string guidStr, string newName)
    {
        if (_internalManager == null) return;

        var targetGuid = Guid.Parse(guidStr);
        int hr = _internalManager.FindDesktop(ref targetGuid, out var targetDesktop);
        if (hr != 0 || targetDesktop == null) return;

        var hsName = HStringInterop.Create(newName);
        try
        {
            if (_internalManager.SupportsSetName)
                _internalManager.SetName(targetDesktop, hsName);
            else
                Logger.LogWarning("Rename desktop is not supported on this Windows build");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to rename desktop");
        }
        finally
        {
            HStringInterop.Delete(hsName);
        }
    }

    /// <summary>
    /// Resolves a desktop name to a GUID string. Also accepts a GUID directly.
    /// </summary>
    private string? ResolveToGuid(string name)
    {
        if (_internalManager == null) return null;

        var desktopsMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            int hr = _internalManager.GetDesktops(out var desktopsArray);
            if (hr != 0 || desktopsArray == null) return null;

            desktopsArray.GetCount(out uint count);
            var iidDesktop = VdmGuids.IID_IVirtualDesktop;

            for (uint i = 0; i < count; i++)
            {
                hr = desktopsArray.GetAt(i, ref iidDesktop, out var unk);
                if (hr != 0 || unk is not IVirtualDesktop desktop) continue;

                desktop.GetID(out Guid guid);
                var guidStr = guid.ToString();

                var regName = GetNameFromRegistry(guidStr);
                var displayName = regName ?? $"Desktop {i + 1}";
                desktopsMap[displayName] = guidStr;

                // Also check if the input IS the GUID (fallback support)
                if (string.Equals(name, guidStr, StringComparison.OrdinalIgnoreCase))
                    return guidStr;
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error scanning desktops for resolution");
            return null;
        }

        return desktopsMap.TryGetValue(name, out var result) ? result : null;
    }

    /// <summary>
    /// Retrieves the user-friendly name of a desktop from the Windows Registry.
    /// </summary>
    private static string? GetNameFromRegistry(string guidStr)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return null;

        try
        {
            var path = $@"Software\Microsoft\Windows\CurrentVersion\Explorer\VirtualDesktops\Desktops\{guidStr}";
            using var key = Registry.CurrentUser.OpenSubKey(path);
            return key?.GetValue("Name") as string;
        }
        catch
        {
            return null;
        }
    }

    // ──────────────────────────────────────────
    // Thread-local singleton + static accessors
    // ──────────────────────────────────────────

    private static VirtualDesktopManager GetManager() => _threadLocal.Value!;

    public static bool IsWindowOnCurrentDesktopStatic(IntPtr hwnd) => GetManager().IsWindowOnCurrentDesktop(hwnd);

    public static string GetWindowDesktopIdStatic(IntPtr hwnd) => GetManager().GetWindowDesktopId(hwnd);

    public static void MoveWindowToDesktopStatic(IntPtr hwnd, string desktopName) => GetManager().MoveWindowToDesktop(hwnd, desktopName);

    public static DesktopInfo GetCurrentDesktopStatic() => GetManager().GetCurrentDesktop();

    public static List<DesktopInfo> GetAllDesktopsStatic() => GetManager().GetAllDesktops();

    public static string CreateDesktopStatic(string? name = null) => GetManager().CreateDesktop(name);

    public static void RemoveDesktopStatic(string desktopName) => GetManager().RemoveDesktop(desktopName);

    public static void RenameDesktopStatic(string desktopName, string newName) => GetManager().RenameDesktop(desktopName, newName);

    public static void SwitchDesktopStatic(string desktopName) => GetManager().SwitchDesktop(desktopName);
}
