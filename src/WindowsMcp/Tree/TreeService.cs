using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using WindowsMcp.Uia;

namespace WindowsMcp.Tree;

/// <summary>
/// Maintains references to a Desktop service (via weak reference) and screen size.
/// Traverses the UI automation tree to find interactive, scrollable, and DOM elements.
/// </summary>
public class TreeService
{
    private static readonly ILogger Logger = LoggerFactory
        .Create(b => b.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace))
        .CreateLogger<TreeService>();

    private static readonly HashSet<string> AlwaysKeyboardFocusableTypes =
    [
        "EditControl",
        "ButtonControl",
        "CheckBoxControl",
        "RadioButtonControl",
        "TabItemControl",
    ];

    private readonly WeakReference<object> _desktopRef;
    private readonly Func<Control, bool> _isWindowBrowser;
    private readonly BoundingBox _screenBox;

    private Control? _dom;
    private BoundingBox? _domBoundingBox;
    private TreeState? _treeState;

    // Focus change debouncing
    private (int[] eventKey, double lastTime)? _lastFocusEvent;

    /// <summary>
    /// Create a new TreeService with a desktop service reference.
    /// Screen size is obtained from UiaHelpers.GetScreenSize().
    /// </summary>
    /// <param name="desktop">Reference to the Desktop service (stored as weak reference).</param>
    /// <param name="isWindowBrowser">
    /// Delegate that returns true if a given Control represents a browser window.
    /// If null, all windows are treated as non-browser.
    /// </param>
    public TreeService(object desktop, Func<Control, bool>? isWindowBrowser = null)
    {
        _desktopRef = new WeakReference<object>(desktop);
        var (width, height) = UiaHelpers.GetScreenSize();
        _screenBox = new BoundingBox(
            Left: 0,
            Top: 0,
            Right: width,
            Bottom: height,
            Width: width,
            Height: height);
        _isWindowBrowser = isWindowBrowser ?? (_ => false);
    }

    /// <summary>
    /// Create a new TreeService.
    /// </summary>
    /// <param name="desktop">Reference to the Desktop service (stored as weak reference).</param>
    /// <param name="screenWidth">Width of the primary screen in pixels.</param>
    /// <param name="screenHeight">Height of the primary screen in pixels.</param>
    /// <param name="isWindowBrowser">
    /// Delegate that returns true if a given Control represents a browser window.
    /// If null, all windows are treated as non-browser.
    /// </param>
    public TreeService(
        object desktop,
        int screenWidth,
        int screenHeight,
        Func<Control, bool>? isWindowBrowser = null)
    {
        _desktopRef = new WeakReference<object>(desktop);
        _screenBox = new BoundingBox(
            Left: 0,
            Top: 0,
            Right: screenWidth,
            Bottom: screenHeight,
            Width: screenWidth,
            Height: screenHeight);
        _isWindowBrowser = isWindowBrowser ?? (_ => false);
    }

    /// <summary>
    /// Main method that traverses the UI automation tree to find interactive,
    /// scrollable, and DOM elements.
    /// </summary>
    public TreeState GetState(
        int? activeWindowHandle,
        List<int> otherWindowsHandles,
        bool useDom = false)
    {
        // Reset DOM state to prevent leaks and stale data
        _dom = null;
        _domBoundingBox = null;
        var sw = Stopwatch.StartNew();

        bool activeWindowFlag = false;
        List<int> windowsHandles;
        if (activeWindowHandle.HasValue)
        {
            activeWindowFlag = true;
            windowsHandles = [activeWindowHandle.Value, .. otherWindowsHandles];
        }
        else
        {
            windowsHandles = [.. otherWindowsHandles];
        }

        var (interactiveNodes, scrollableNodes, domInformativeNodes) = GetWindowWiseNodes(windowsHandles, activeWindowFlag, useDom);

        var rootNode = new TreeElementNode
        {
            Name = "Desktop",
            ControlType = "PaneControl",
            BoundingBox = _screenBox,
            Center = _screenBox.GetCenter(),
            WindowName = "Desktop",
            Xpath = "",
            Value = "",
            Shortcut = "",
            IsFocused = false,
        };

        ScrollElementNode? domNode = null;
        if (_dom != null && _domBoundingBox != null)
        {
            ScrollPattern? scrollPattern = GetScrollPattern(_dom);
            domNode = new ScrollElementNode
            {
                Name = "DOM",
                ControlType = "DocumentControl",
                BoundingBox = _domBoundingBox,
                Center = _domBoundingBox.GetCenter(),
                HorizontalScrollable = scrollPattern?.HorizontallyScrollable ?? false,
                HorizontalScrollPercent = scrollPattern is { HorizontallyScrollable: true }
                    ? scrollPattern.HorizontalScrollPercent
                    : 0,
                VerticalScrollable = scrollPattern?.VerticallyScrollable ?? false,
                VerticalScrollPercent = scrollPattern is { VerticallyScrollable: true }
                    ? scrollPattern.VerticalScrollPercent
                    : 0,
                Xpath = "",
                WindowName = "DOM",
                IsFocused = false,
            };
        }

        _treeState = new TreeState
        {
            RootNode = rootNode,
            DomNode = domNode,
            InteractiveNodes = interactiveNodes,
            ScrollableNodes = scrollableNodes,
            DomInformativeNodes = domInformativeNodes,
        };

        sw.Stop();
        Logger.LogInformation("Tree State capture took {Elapsed:F2} seconds",
            sw.Elapsed.TotalSeconds);
        return _treeState;
    }

    /// <summary>
    /// Processes each window handle to build element lists.
    /// </summary>
    public (List<TreeElementNode> Interactive, List<ScrollElementNode> Scrollable, List<TextElementNode> DomInformative)
        GetWindowWiseNodes(
            List<int> windowsHandles,
            bool activeWindowFlag,
            bool useDom = false)
    {
        var interactiveNodes = new List<TreeElementNode>();
        var scrollableNodes = new List<ScrollElementNode>();
        var domInformativeNodes = new List<TextElementNode>();

        // Sequential processing to avoid COM apartment threading deadlock
        foreach (var handle in windowsHandles)
        {
            bool isBrowser = false;
            try
            {
                var tempNode = UiaHelpers.ControlFromHandle(new IntPtr(handle));
                if (tempNode != null)
                {
                    if (activeWindowFlag && tempNode.ClassName == "Progman")
                        continue;
                    isBrowser = _isWindowBrowser(tempNode);
                }
            }
            catch
            {
                // ignore
            }

            for (int attempt = 0; attempt <= TreeConfig.ThreadMaxRetries; attempt++)
            {
                try
                {
                    var result = GetNodes(handle, isBrowser, useDom);
                    if (result.HasValue)
                    {
                        var (elementNodes, scrollNodes, infoNodes) = result.Value;
                        interactiveNodes.AddRange(elementNodes);
                        scrollableNodes.AddRange(scrollNodes);
                        domInformativeNodes.AddRange(infoNodes);
                    }
                    break;
                }
                catch (Exception e)
                {
                    Logger.LogDebug(
                        "Error processing handle {Handle}, attempt {Attempt}/{MaxRetries}: {Error}",
                        handle, attempt + 1, TreeConfig.ThreadMaxRetries + 1, e.Message);
                    if (attempt >= TreeConfig.ThreadMaxRetries)
                    {
                        Logger.LogError(
                            "Task failed for handle {Handle} after {MaxRetries} retries",
                            handle, TreeConfig.ThreadMaxRetries + 1);
                        break;
                    }
                }
            }
        }

        return (interactiveNodes, scrollableNodes, domInformativeNodes);
    }

    /// <summary>
    /// Computes the intersection-over-union bounding box, clamped to screen boundaries.
    /// </summary>
    public BoundingBox IouBoundingBox(Rect windowBox, Rect elementBox)
    {
        // Step 1: Intersection of element and window
        int intersectionLeft = Math.Max(windowBox.Left, elementBox.Left);
        int intersectionTop = Math.Max(windowBox.Top, elementBox.Top);
        int intersectionRight = Math.Min(windowBox.Right, elementBox.Right);
        int intersectionBottom = Math.Min(windowBox.Bottom, elementBox.Bottom);

        // Step 2: Clamp to screen boundaries
        intersectionLeft = Math.Max(_screenBox.Left, intersectionLeft);
        intersectionTop = Math.Max(_screenBox.Top, intersectionTop);
        intersectionRight = Math.Min(_screenBox.Right, intersectionRight);
        intersectionBottom = Math.Min(_screenBox.Bottom, intersectionBottom);

        // Step 3: Validate intersection
        if (intersectionRight > intersectionLeft && intersectionBottom > intersectionTop)
        {
            return new BoundingBox(
                Left: intersectionLeft,
                Top: intersectionTop,
                Right: intersectionRight,
                Bottom: intersectionBottom,
                Width: intersectionRight - intersectionLeft,
                Height: intersectionBottom - intersectionTop);
        }

        // No valid visible intersection
        return new BoundingBox(Left: 0, Top: 0, Right: 0, Bottom: 0, Width: 0, Height: 0);
    }

    /// <summary>
    /// Overload accepting BoundingBox as window box (used for DOM bounding box).
    /// </summary>
    public BoundingBox IouBoundingBox(BoundingBox windowBox, Rect elementBox)
    {
        var windowRect = new Rect(windowBox.Left, windowBox.Top, windowBox.Right, windowBox.Bottom);
        return IouBoundingBox(windowRect, elementBox);
    }

    /// <summary>
    /// Checks if a node of a given control type has a first child of a given child control type.
    /// </summary>
    public bool ElementHasChildElement(Control node, string controlType, string childControlType)
    {
        if (node.LocalizedControlType == controlType)
        {
            var firstChild = node.GetFirstChildControl();
            if (firstChild == null)
                return false;
            return firstChild.LocalizedControlType == childControlType;
        }
        return false;
    }

    /// <summary>
    /// Applies DOM-specific corrections to interactive nodes (e.g., list items with links,
    /// group controls, links with headings).
    /// </summary>
    private void DomCorrection(
        Control node,
        List<TreeElementNode> domInteractiveNodes,
        string windowName)
    {
        if (ElementHasChildElement(node, "list item", "link")
            || ElementHasChildElement(node, "item", "link"))
        {
            domInteractiveNodes.RemoveAt(domInteractiveNodes.Count - 1);
            return;
        }

        if (node.ControlTypeName == "GroupControl")
        {
            domInteractiveNodes.RemoveAt(domInteractiveNodes.Count - 1);

            // Inlined is_keyboard_focusable logic for correction
            string controlTypeNameCheck = node.CachedControlTypeName;
            bool isKbFocusable = AlwaysKeyboardFocusableTypes.Contains(controlTypeNameCheck)
                || node.CachedIsKeyboardFocusable;

            if (isKbFocusable)
            {
                var child = node;
                try
                {
                    while (child.GetFirstChildControl() != null)
                    {
                        if (TreeConfig.InteractiveControlTypeNames.Contains(child.ControlTypeName))
                            return;
                        child = child.GetFirstChildControl()!;
                    }
                }
                catch
                {
                    return;
                }

                if (child.ControlTypeName != "TextControl")
                    return;

                var legacyPattern = GetLegacyIAccessiblePattern(node);
                string value = legacyPattern?.Value ?? "";
                var elementBoundingBox = node.BoundingRectangle;
                var boundingBox = IouBoundingBox(_domBoundingBox!, elementBoundingBox);
                var center = boundingBox.GetCenter();
                bool isFocused = node.HasKeyboardFocus;

                domInteractiveNodes.Add(new TreeElementNode
                {
                    Name = (child.Name ?? "").Trim(),
                    ControlType = node.LocalizedControlType,
                    Value = value,
                    Shortcut = node.AcceleratorKey,
                    BoundingBox = boundingBox,
                    Xpath = "",
                    Center = center,
                    WindowName = windowName,
                    IsFocused = isFocused,
                });
            }
            return;
        }

        if (ElementHasChildElement(node, "link", "heading"))
        {
            domInteractiveNodes.RemoveAt(domInteractiveNodes.Count - 1);
            var childNode = node.GetFirstChildControl()!;
            string controlType = "link";
            var legacyPattern = GetLegacyIAccessiblePattern(childNode);
            string value = legacyPattern?.Value ?? "";
            var elementBoundingBox = childNode.BoundingRectangle;
            var boundingBox = IouBoundingBox(_domBoundingBox!, elementBoundingBox);
            var center = boundingBox.GetCenter();
            bool isFocused = childNode.HasKeyboardFocus;

            domInteractiveNodes.Add(new TreeElementNode
            {
                Name = (childNode.Name ?? "").Trim(),
                ControlType = controlType,
                Value = (childNode.Name ?? "").Trim(),
                Shortcut = childNode.AcceleratorKey,
                BoundingBox = boundingBox,
                Xpath = "",
                Center = center,
                WindowName = windowName,
                IsFocused = isFocused,
            });
        }
    }

    /// <summary>
    /// Recursive traversal of the UIA element tree, filtering elements by control type.
    /// </summary>
    public void TreeTraversal(
        Control node,
        Rect windowBoundingBox,
        string windowName,
        bool isBrowser,
        List<TreeElementNode>? interactiveNodes = null,
        List<ScrollElementNode>? scrollableNodes = null,
        List<TreeElementNode>? domInteractiveNodes = null,
        List<TextElementNode>? domInformativeNodes = null,
        bool isDom = false,
        bool isDialog = false,
        CacheRequest? elementCacheReq = null,
        CacheRequest? childrenCacheReq = null)
    {
        try
        {
            // Build cached control if caching is enabled
            if (elementCacheReq != null)
            {
                node = CachedControlHelper.BuildCachedControl(node, elementCacheReq);
            }

            // Checks to skip nodes that are not interactive
            bool isOffscreen = node.CachedIsOffscreen;
            string controlTypeName = node.CachedControlTypeName;

            // Scrollable check
            if (scrollableNodes != null)
            {
                var combinedTypes = new HashSet<string>(TreeConfig.InteractiveControlTypeNames);
                combinedTypes.UnionWith(TreeConfig.InformativeControlTypeNames);

                if (!combinedTypes.Contains(controlTypeName) && !isOffscreen)
                {
                    try
                    {
                        var scrollPattern = GetScrollPattern(node);
                        if (scrollPattern != null && scrollPattern.VerticallyScrollable)
                        {
                            var box = node.CachedBoundingRectangle;
                            var (x, y) = TreeUtils.RandomPointWithinBoundingBox(node, scaleFactor: 0.8);
                            var center = new Center(X: x, Y: y);
                            string name = node.CachedName;
                            string automationId = node.CachedAutomationId;
                            string localizedControlType = node.CachedLocalizedControlType;
                            bool hasKeyboardFocus = node.CachedHasKeyboardFocus;

                            scrollableNodes.Add(new ScrollElementNode
                            {
                                Name = !string.IsNullOrEmpty(name.Trim())
                                    ? name.Trim()
                                    : !string.IsNullOrEmpty(automationId)
                                        ? automationId
                                        : !string.IsNullOrEmpty(localizedControlType)
                                            ? ToTitleCase(localizedControlType)
                                            : "''",
                                ControlType = ToTitleCase(localizedControlType),
                                BoundingBox = new BoundingBox(
                                    Left: box.Left,
                                    Top: box.Top,
                                    Right: box.Right,
                                    Bottom: box.Bottom,
                                    Width: box.Width(),
                                    Height: box.Height()),
                                Center = center,
                                Xpath = "",
                                HorizontalScrollable = scrollPattern.HorizontallyScrollable,
                                HorizontalScrollPercent = scrollPattern.HorizontallyScrollable
                                    ? scrollPattern.HorizontalScrollPercent
                                    : 0,
                                VerticalScrollable = scrollPattern.VerticallyScrollable,
                                VerticalScrollPercent = scrollPattern.VerticallyScrollable
                                    ? scrollPattern.VerticalScrollPercent
                                    : 0,
                                WindowName = windowName,
                                IsFocused = hasKeyboardFocus,
                            });
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }

            // Interactive and Informative checks
            bool isControlElement = node.CachedIsControlElement;
            var elementBoundingBox = node.CachedBoundingRectangle;
            int width = elementBoundingBox.Width();
            int height = elementBoundingBox.Height();
            long area = (long)width * height;

            // Is Visible Check
            bool isVisible = area > 0
                && (!isOffscreen || controlTypeName == "EditControl")
                && isControlElement;

            if (isVisible)
            {
                bool isEnabled = node.CachedIsEnabled;
                if (isEnabled)
                {
                    // Determine is_keyboard_focusable
                    bool isKeyboardFocusable = AlwaysKeyboardFocusableTypes.Contains(controlTypeName)
                        || node.CachedIsKeyboardFocusable;

                    // Interactive Check
                    if (interactiveNodes != null)
                    {
                        bool isInteractive = false;

                        if (isBrowser
                            && (controlTypeName == "DataItemControl" || controlTypeName == "ListItemControl")
                            && !isKeyboardFocusable)
                        {
                            isInteractive = false;
                        }
                        else if (!isBrowser
                            && controlTypeName == "ImageControl"
                            && isKeyboardFocusable)
                        {
                            isInteractive = true;
                        }
                        else if (TreeConfig.InteractiveControlTypeNames.Contains(controlTypeName)
                            || TreeConfig.DocumentControlTypeNames.Contains(controlTypeName))
                        {
                            // Role check
                            bool isRoleInteractive = false;
                            try
                            {
                                var legacyPattern = GetLegacyIAccessiblePattern(node);
                                if (legacyPattern != null)
                                {
                                    string roleName = AccessibleRoleNames.Names
                                        .GetValueOrDefault(legacyPattern.Role, "Default");
                                    isRoleInteractive = TreeConfig.InteractiveRoles.Contains(roleName);
                                }
                            }
                            catch
                            {
                                // ignore
                            }

                            // Image check
                            bool isImage = false;
                            if (controlTypeName == "ImageControl")
                            {
                                string localized = node.CachedLocalizedControlType;
                                if (localized == "graphic" || !isKeyboardFocusable)
                                    isImage = true;
                            }

                            if (isRoleInteractive && (!isImage || isKeyboardFocusable))
                                isInteractive = true;
                        }
                        else if (controlTypeName == "GroupControl")
                        {
                            if (isBrowser)
                            {
                                bool isRoleInteractive = false;
                                try
                                {
                                    var legacyPattern = GetLegacyIAccessiblePattern(node);
                                    if (legacyPattern != null)
                                    {
                                        string roleName = AccessibleRoleNames.Names
                                            .GetValueOrDefault(legacyPattern.Role, "Default");
                                        isRoleInteractive = TreeConfig.InteractiveRoles.Contains(roleName);
                                    }
                                }
                                catch
                                {
                                    // ignore
                                }

                                bool isDefaultAction = false;
                                try
                                {
                                    var legacyPattern = GetLegacyIAccessiblePattern(node);
                                    if (legacyPattern != null
                                        && TreeConfig.DefaultActions.Contains(
                                            ToTitleCase(legacyPattern.DefaultAction)))
                                    {
                                        isDefaultAction = true;
                                    }
                                }
                                catch
                                {
                                    // ignore
                                }

                                if (isRoleInteractive && (isDefaultAction || isKeyboardFocusable))
                                    isInteractive = true;
                            }
                        }

                        if (isInteractive)
                        {
                            var legacyPattern = GetLegacyIAccessiblePattern(node);
                            string value = legacyPattern?.Value != null
                                ? legacyPattern.Value.Trim()
                                : "";
                            bool isFocused = node.CachedHasKeyboardFocus;
                            string name = node.CachedName.Trim();
                            string localizedControlType = node.CachedLocalizedControlType;
                            string acceleratorKey = node.CachedAcceleratorKey;

                            if (isBrowser && isDom)
                            {
                                var boundingBox = IouBoundingBox(_domBoundingBox!, elementBoundingBox);
                                var center = boundingBox.GetCenter();
                                var treeNode = new TreeElementNode
                                {
                                    Name = name,
                                    ControlType = ToTitleCase(localizedControlType),
                                    Value = value,
                                    Shortcut = acceleratorKey,
                                    BoundingBox = boundingBox,
                                    Center = center,
                                    Xpath = "",
                                    WindowName = windowName,
                                    IsFocused = isFocused,
                                };
                                domInteractiveNodes!.Add(treeNode);
                                DomCorrection(node, domInteractiveNodes, windowName);
                            }
                            else
                            {
                                var boundingBox = IouBoundingBox(windowBoundingBox, elementBoundingBox);
                                var center = boundingBox.GetCenter();
                                var treeNode = new TreeElementNode
                                {
                                    Name = name,
                                    ControlType = ToTitleCase(localizedControlType),
                                    Value = value,
                                    Shortcut = acceleratorKey,
                                    BoundingBox = boundingBox,
                                    Center = center,
                                    Xpath = "",
                                    WindowName = windowName,
                                    IsFocused = isFocused,
                                };
                                interactiveNodes.Add(treeNode);
                            }
                        }
                    }

                    // Informative Check
                    if (domInformativeNodes != null)
                    {
                        bool isText = false;
                        if (TreeConfig.InformativeControlTypeNames.Contains(controlTypeName))
                        {
                            bool isImageCheck = false;
                            if (controlTypeName == "ImageControl")
                            {
                                string localized = node.CachedLocalizedControlType;
                                if (!isKeyboardFocusable)
                                {
                                    isImageCheck = true;
                                }
                                else if (localized == "graphic")
                                {
                                    isImageCheck = true;
                                }
                            }

                            if (!isImageCheck)
                                isText = true;
                        }

                        if (isText && isBrowser && isDom)
                        {
                            string name = node.CachedName;
                            domInformativeNodes.Add(new TextElementNode
                            {
                                Text = name.Trim(),
                            });
                        }
                    }
                }
            }

            // Phase 3: Cached Children Retrieval
            var children = CachedControlHelper.GetCachedChildren(node, childrenCacheReq);

            // Recursively traverse: right-to-left for normal apps, left-to-right for DOM
            var orderedChildren = isDom ? children : Enumerable.Reverse(children).ToList();
            foreach (var child in orderedChildren)
            {
                // Check if the child is a DOM element
                if (isBrowser && child.CachedAutomationId == "RootWebArea")
                {
                    var bb = child.CachedBoundingRectangle;
                    _domBoundingBox = new BoundingBox(
                        Left: bb.Left,
                        Top: bb.Top,
                        Right: bb.Right,
                        Bottom: bb.Bottom,
                        Width: bb.Width(),
                        Height: bb.Height());
                    _dom = child;
                    // Enter DOM subtree
                    TreeTraversal(
                        child, windowBoundingBox, windowName, isBrowser,
                        interactiveNodes, scrollableNodes, domInteractiveNodes, domInformativeNodes,
                        isDom: true, isDialog: isDialog,
                        elementCacheReq: elementCacheReq, childrenCacheReq: childrenCacheReq);
                }
                // Check if the child is a WindowControl (dialog)
                else if (child.CachedControlTypeId == Uia.ControlType.WindowControl)
                {
                    if (!child.CachedIsOffscreen)
                    {
                        if (isDom)
                        {
                            var bb = child.CachedBoundingRectangle;
                            if (_domBoundingBox != null && bb.Width() > 0.8 * _domBoundingBox.Width)
                            {
                                // Window covers majority of DOM — clear DOM interactive nodes
                                domInteractiveNodes?.Clear();
                            }
                        }
                        else
                        {
                            // Inline is_window_modal
                            bool isModal = false;
                            try
                            {
                                var windowPattern = GetWindowPattern(child);
                                if (windowPattern != null)
                                    isModal = windowPattern.IsModal;
                            }
                            catch
                            {
                                // ignore
                            }

                            if (isModal)
                            {
                                // Modal window — clear interactive nodes
                                interactiveNodes?.Clear();
                            }
                        }
                    }
                    // Enter dialog subtree
                    TreeTraversal(
                        child, windowBoundingBox, windowName, isBrowser,
                        interactiveNodes, scrollableNodes, domInteractiveNodes, domInformativeNodes,
                        isDom: isDom, isDialog: true,
                        elementCacheReq: elementCacheReq, childrenCacheReq: childrenCacheReq);
                }
                else
                {
                    // Normal non-dialog children
                    TreeTraversal(
                        child, windowBoundingBox, windowName, isBrowser,
                        interactiveNodes, scrollableNodes, domInteractiveNodes, domInformativeNodes,
                        isDom: isDom, isDialog: isDialog,
                        elementCacheReq: elementCacheReq, childrenCacheReq: childrenCacheReq);
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error in TreeTraversal: {Error}", e.Message);
            throw;
        }
    }

    /// <summary>
    /// Corrects well-known window class names to user-friendly application names.
    /// </summary>
    public static string AppNameCorrection(string appName) => appName switch
    {
        "Progman" => "Desktop",
        "Shell_TrayWnd" or "Shell_SecondaryTrayWnd" => "Taskbar",
        "Microsoft.UI.Content.PopupWindowSiteBridge" => "Context Menu",
        _ => appName,
    };

    /// <summary>
    /// Gets interactive, scrollable, and informative nodes for a single window handle.
    /// </summary>
    public (List<TreeElementNode>, List<ScrollElementNode>, List<TextElementNode>)?
        GetNodes(int handle, bool isBrowser = false, bool useDom = false)
    {
        Control? node = null;
        try
        {
            // Rehydrate Control from handle
            node = UiaHelpers.ControlFromHandle(new IntPtr(handle));
            if (node == null)
                throw new InvalidOperationException("Failed to create Control from handle");

            // Create fresh cache requests for this traversal session
            var elementCacheReq = CacheRequestFactory.CreateTreeTraversalCache();
            elementCacheReq.TreeScopeValue = TreeScope.TreeScope_Element;

            var childrenCacheReq = CacheRequestFactory.CreateTreeTraversalCache();
            childrenCacheReq.TreeScopeValue =
                TreeScope.TreeScope_Element | TreeScope.TreeScope_Children;

            var windowBoundingBox = node.BoundingRectangle;

            var interactiveNodes = new List<TreeElementNode>();
            var domInteractiveNodes = new List<TreeElementNode>();
            var domInformativeNodes = new List<TextElementNode>();
            var scrollableNodes = new List<ScrollElementNode>();
            string windowName = AppNameCorrection((node.Name ?? "").Trim());

            TreeTraversal(
                node, windowBoundingBox, windowName, isBrowser,
                interactiveNodes, scrollableNodes, domInteractiveNodes, domInformativeNodes,
                isDom: false, isDialog: false,
                elementCacheReq: elementCacheReq, childrenCacheReq: childrenCacheReq);

            Logger.LogDebug("Window name: {WindowName}", windowName);
            Logger.LogDebug("Interactive nodes: {Count}", interactiveNodes.Count);
            if (isBrowser)
            {
                Logger.LogDebug("DOM interactive nodes: {Count}", domInteractiveNodes.Count);
                Logger.LogDebug("DOM informative nodes: {Count}", domInformativeNodes.Count);
            }
            Logger.LogDebug("Scrollable nodes: {Count}", scrollableNodes.Count);

            if (useDom)
            {
                return isBrowser
                    ? (domInteractiveNodes, scrollableNodes, domInformativeNodes)
                    : (new List<TreeElementNode>(), new List<ScrollElementNode>(), new List<TextElementNode>());
            }

            interactiveNodes.AddRange(domInteractiveNodes);
            return (interactiveNodes, scrollableNodes, domInformativeNodes);
        }
        catch (Exception e)
        {
            Logger.LogError("Error getting nodes for {Name}: {Error}",
                node?.Name ?? "<unknown>", e.Message);
            throw;
        }
    }

    /// <summary>
    /// Handle focus change events.
    /// </summary>
    public void OnFocusChange(object? sender)
    {
        if (sender is not IUIAutomationElement senderElement)
            return;

        Control? element;
        int[] runtimeId;
        try
        {
            element = Control.CreateControlFromElement(senderElement);
            if (element == null) return;
            runtimeId = element.GetRuntimeId();
        }
        catch (COMException)
        {
            return;
        }
        catch (IOException)
        {
            return;
        }

        // Debounce duplicate events
        double currentTime = Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;
        if (_lastFocusEvent.HasValue)
        {
            var (lastKey, lastTime) = _lastFocusEvent.Value;
            if (lastKey.SequenceEqual(runtimeId) && (currentTime - lastTime) < 1.0)
                return;
        }
        _lastFocusEvent = (runtimeId, currentTime);

        try
        {
            Logger.LogDebug("[WatchDog] Focus changed to: '{Name}' ({ControlType})",
                element.Name, element.ControlTypeName);
        }
        catch
        {
            // ignore
        }
    }

    /// <summary>
    /// Handle property change events.
    /// </summary>
    public void OnPropertyChange(object? sender, int propertyId, object? newValue)
    {
        if (sender is not IUIAutomationElement senderElement)
            return;

        try
        {
            var element = Control.CreateControlFromElement(senderElement);
            if (element == null) return;
            Logger.LogDebug(
                "[WatchDog] Property changed: ID={PropertyId} Value={Value} Element: '{Name}' ({ControlType})",
                propertyId, newValue, element.Name, element.ControlTypeName);
        }
        catch
        {
            // ignore
        }
    }

    // ── Helper methods ──

    private static ScrollPattern? GetScrollPattern(Control node)
    {
        try
        {
            var patternObj = node.GetPatternObject(PatternId.ScrollPattern);
            return patternObj != null ? new ScrollPattern(patternObj) : null;
        }
        catch
        {
            return null;
        }
    }

    private static LegacyIAccessiblePattern? GetLegacyIAccessiblePattern(Control node)
    {
        try
        {
            var patternObj = node.GetPatternObject(PatternId.LegacyIAccessiblePattern);
            return patternObj != null ? new LegacyIAccessiblePattern(patternObj) : null;
        }
        catch
        {
            return null;
        }
    }

    private static WindowPattern? GetWindowPattern(Control node)
    {
        try
        {
            var patternObj = node.GetPatternObject(PatternId.WindowPattern);
            return patternObj != null ? new WindowPattern(patternObj) : null;
        }
        catch
        {
            return null;
        }
    }

    private static string ToTitleCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(input);
    }
}
