using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using WindowsMcp.Uia;
using static WindowsMcp.Uia.TreeScope;

namespace WindowsMcp.Watchdog;

/// <summary>
/// Unified WatchDog service for monitoring UI Automation events.
/// Runs a dedicated STA thread for COM event pumping and supports
/// focus, structure-changed, and property-changed event handlers.
/// </summary>
public sealed class WatchDog : IDisposable
{
    private static readonly ILogger _logger =
        LoggerFactory.Create(b => b.AddDebug()).CreateLogger<WatchDog>();

    private readonly ManualResetEventSlim _isRunning = new(false);
    private Thread? _thread;

    // Callbacks (set by the consumer)
    internal Action<IUIAutomationElement>? FocusCallback;
    internal Action<IUIAutomationElement, int, int[]>? StructureCallback;
    private IUIAutomationElement? _structureElement;
    internal Action<IUIAutomationElement, int, object>? PropertyCallback;
    private IUIAutomationElement? _propertyElement;
    private int[]? _propertyIds;

    // Active handler state (managed on the STA thread)
    private FocusChangedEventHandler? _focusHandler;
    private StructureChangedEventHandler? _structureHandler;
    private IUIAutomationElement? _activeStructureElement;
    private PropertyChangedEventHandler? _propertyHandler;
    private IUIAutomationElement? _activePropertyElement;
    private int[]? _activePropertyIds;

    public void Start()
    {
        if (_isRunning.IsSet) return;
        _isRunning.Set();
        _thread = new Thread(Run) { Name = "WatchDogThread", IsBackground = true };
        _thread.SetApartmentState(ApartmentState.STA);
        _thread.Start();
    }

    public void Stop()
    {
        if (!_isRunning.IsSet) return;
        _isRunning.Reset();
        _thread?.Join(TimeSpan.FromSeconds(2));
        _thread = null;
    }

    public void Dispose() => Stop();

    /// <summary>Set the callback for focus changes. Pass null to disable.</summary>
    public void SetFocusCallback(Action<IUIAutomationElement>? callback)
    {
        FocusCallback = callback;
    }

    /// <summary>Set the callback for structure changes. Pass null to disable.</summary>
    public void SetStructureCallback(
        Action<IUIAutomationElement, int, int[]>? callback,
        IUIAutomationElement? element = null)
    {
        StructureCallback = callback;
        _structureElement = element;
    }

    /// <summary>Set the callback for property changes. Pass null to disable.</summary>
    public void SetPropertyCallback(
        Action<IUIAutomationElement, int, object>? callback,
        IUIAutomationElement? element = null,
        int[]? propertyIds = null)
    {
        PropertyCallback = callback;
        _propertyElement = element;
        _propertyIds = propertyIds;
    }

    private void Run()
    {
        var uia = AutomationClient.Instance.Automation;

        try
        {
            while (_isRunning.IsSet)
            {
                // --- Focus Monitoring ---
                if (FocusCallback is not null && _focusHandler is null)
                {
                    try
                    {
                        _focusHandler = new FocusChangedEventHandler(this);
                        uia.AddFocusChangedEventHandler(null!, _focusHandler);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug("Failed to add focus handler: {Error}", ex.Message);
                    }
                }
                else if (FocusCallback is null && _focusHandler is not null)
                {
                    try
                    {
                        uia.RemoveFocusChangedEventHandler(_focusHandler);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug("Failed to remove focus handler: {Error}", ex.Message);
                    }
                    _focusHandler = null;
                }

                // --- Structure Monitoring ---
                {
                    bool configChanged = _structureElement != _activeStructureElement;
                    bool shouldBeActive = StructureCallback is not null;
                    bool isActive = _structureHandler is not null;

                    if (isActive && (!shouldBeActive || configChanged))
                    {
                        try
                        {
                            var target = _activeStructureElement ?? uia.GetRootElement();
                            uia.RemoveStructureChangedEventHandler(target, _structureHandler!);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug("Failed to remove structure handler: {Error}", ex.Message);
                        }
                        _structureHandler = null;
                        _activeStructureElement = null;
                        isActive = false;
                    }

                    if (shouldBeActive && !isActive)
                    {
                        try
                        {
                            var target = _structureElement ?? uia.GetRootElement();
                            _structureHandler = new StructureChangedEventHandler(this);
                            uia.AddStructureChangedEventHandler(
                                target, TreeScope_Subtree, null!, _structureHandler);
                            _activeStructureElement = target;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug("Failed to add structure handler: {Error}", ex.Message);
                        }
                    }
                }

                // --- Property Monitoring ---
                {
                    bool configChanged =
                        _propertyElement != _activePropertyElement ||
                        !ArrayEquals(_propertyIds, _activePropertyIds);
                    bool shouldBeActive = PropertyCallback is not null;
                    bool isActive = _propertyHandler is not null;

                    if (isActive && (!shouldBeActive || configChanged))
                    {
                        try
                        {
                            var target = _activePropertyElement ?? uia.GetRootElement();
                            uia.RemovePropertyChangedEventHandler(target, _propertyHandler!);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug("Failed to remove property handler: {Error}", ex.Message);
                        }
                        _propertyHandler = null;
                        _activePropertyElement = null;
                        _activePropertyIds = null;
                        isActive = false;
                    }

                    if (shouldBeActive && !isActive)
                    {
                        try
                        {
                            var target = _propertyElement ?? uia.GetRootElement();
                            // Default property IDs: Name(30005), Value(30045),
                            // LegacyIAccessibleValue(30093), ToggleState(30128)
                            var pIds = _propertyIds ?? [30005, 30045, 30093, 30128];

                            _propertyHandler = new PropertyChangedEventHandler(this);
                            uia.AddPropertyChangedEventHandler(
                                target, TreeScope_Subtree, null!, _propertyHandler, pIds);
                            _activePropertyElement = target;
                            _activePropertyIds = pIds;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogDebug("Failed to add property handler: {Error}", ex.Message);
                        }
                    }
                }

                // Pump COM events
                Thread.Sleep(100);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("WatchDogService died: {Error}", ex.Message);
        }
        finally
        {
            // Cleanup handlers on exit
            if (_focusHandler is not null)
            {
                try { uia.RemoveFocusChangedEventHandler(_focusHandler); }
                catch { /* best effort */ }
                _focusHandler = null;
            }

            if (_structureHandler is not null)
            {
                try
                {
                    var target = _activeStructureElement ?? uia.GetRootElement();
                    uia.RemoveStructureChangedEventHandler(target, _structureHandler);
                }
                catch { /* best effort */ }
                _structureHandler = null;
                _activeStructureElement = null;
            }

            if (_propertyHandler is not null)
            {
                try
                {
                    var target = _activePropertyElement ?? uia.GetRootElement();
                    uia.RemovePropertyChangedEventHandler(target, _propertyHandler);
                }
                catch { /* best effort */ }
                _propertyHandler = null;
                _activePropertyElement = null;
                _activePropertyIds = null;
            }
        }
    }

    private static bool ArrayEquals(int[]? a, int[]? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.AsSpan().SequenceEqual(b);
    }
}
