using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using WindowsMcp.Uia;

namespace WindowsMcp.Watchdog;

[ComVisible(true)]
public class FocusChangedEventHandler : IUIAutomationFocusChangedEventHandler
{
    private static readonly ILogger _logger =
        LoggerFactory.Create(b => b.AddDebug()).CreateLogger<FocusChangedEventHandler>();

    private readonly WeakReference<WatchDog> _parent;

    public FocusChangedEventHandler(WatchDog parent)
    {
        _parent = new WeakReference<WatchDog>(parent);
    }

    public int HandleFocusChangedEvent(IUIAutomationElement sender)
    {
        try
        {
            if (_parent.TryGetTarget(out var parent) && parent.FocusCallback is { } cb)
                cb(sender);
        }
        catch (COMException ex)
        {
            _logger.LogDebug("Focus callback COM error: {Error}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Focus callback error: {Error}", ex.Message);
        }
        return 0; // S_OK
    }
}

[ComVisible(true)]
public class StructureChangedEventHandler : IUIAutomationStructureChangedEventHandler
{
    private static readonly ILogger _logger =
        LoggerFactory.Create(b => b.AddDebug()).CreateLogger<StructureChangedEventHandler>();

    private readonly WeakReference<WatchDog> _parent;

    public StructureChangedEventHandler(WatchDog parent)
    {
        _parent = new WeakReference<WatchDog>(parent);
    }

    public int HandleStructureChangedEvent(IUIAutomationElement sender, int changeType, int[] runtimeId)
    {
        try
        {
            if (_parent.TryGetTarget(out var parent) && parent.StructureCallback is { } cb)
                cb(sender, changeType, runtimeId);
        }
        catch (COMException ex)
        {
            _logger.LogDebug("Structure callback COM error: {Error}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Structure callback error: {Error}", ex.Message);
        }
        return 0; // S_OK
    }
}

[ComVisible(true)]
public class PropertyChangedEventHandler : IUIAutomationPropertyChangedEventHandler
{
    private static readonly ILogger _logger =
        LoggerFactory.Create(b => b.AddDebug()).CreateLogger<PropertyChangedEventHandler>();

    private readonly WeakReference<WatchDog> _parent;

    public PropertyChangedEventHandler(WatchDog parent)
    {
        _parent = new WeakReference<WatchDog>(parent);
    }

    public int HandlePropertyChangedEvent(IUIAutomationElement sender, int propertyId, object newValue)
    {
        try
        {
            if (_parent.TryGetTarget(out var parent) && parent.PropertyCallback is { } cb)
                cb(sender, propertyId, newValue);
        }
        catch (COMException ex)
        {
            _logger.LogDebug("Property callback COM error: {Error}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Property callback error: {Error}", ex.Message);
        }
        return 0; // S_OK
    }
}
