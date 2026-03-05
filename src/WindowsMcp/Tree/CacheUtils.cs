using Microsoft.Extensions.Logging;
using WindowsMcp.Uia;

namespace WindowsMcp.Tree;

/// <summary>
/// Factory for creating optimized cache requests for different scenarios.
/// </summary>
public static class CacheRequestFactory
{
    /// <summary>
    /// Creates a cache request optimized for tree traversal.
    /// Caches all commonly accessed properties and patterns.
    /// </summary>
    public static CacheRequest CreateTreeTraversalCache()
    {
        var cacheRequest = new CacheRequest();

        // Set scope to cache element and its children
        cacheRequest.TreeScopeValue = TreeScope.TreeScope_Element | TreeScope.TreeScope_Children;

        // Basic identification properties
        cacheRequest.AddProperty(PropertyId.NameProperty);
        cacheRequest.AddProperty(PropertyId.AutomationIdProperty);
        cacheRequest.AddProperty(PropertyId.LocalizedControlTypeProperty);
        cacheRequest.AddProperty(PropertyId.AcceleratorKeyProperty);
        cacheRequest.AddProperty(PropertyId.ClassNameProperty);
        cacheRequest.AddProperty(PropertyId.ControlTypeProperty);

        // State properties for visibility and interaction checks
        cacheRequest.AddProperty(PropertyId.IsEnabledProperty);
        cacheRequest.AddProperty(PropertyId.IsOffscreenProperty);
        cacheRequest.AddProperty(PropertyId.IsControlElementProperty);
        cacheRequest.AddProperty(PropertyId.HasKeyboardFocusProperty);
        cacheRequest.AddProperty(PropertyId.IsKeyboardFocusableProperty);

        // Layout properties
        cacheRequest.AddProperty(PropertyId.BoundingRectangleProperty);

        return cacheRequest;
    }
}

/// <summary>
/// Helper class for working with cached controls.
/// </summary>
public static class CachedControlHelper
{
    /// <summary>
    /// Build a cached version of a control.
    /// </summary>
    /// <param name="node">The control to cache.</param>
    /// <param name="cacheRequest">Optional custom cache request. If null, uses tree traversal cache.</param>
    /// <param name="logger">Optional logger.</param>
    /// <returns>A control with cached properties, or the original control if caching fails.</returns>
    public static Control BuildCachedControl(Control node, CacheRequest? cacheRequest = null, ILogger? logger = null)
    {
        cacheRequest ??= CacheRequestFactory.CreateTreeTraversalCache();

        try
        {
            return node.BuildUpdatedCache(cacheRequest);
        }
        catch (Exception e)
        {
            logger?.LogDebug("Failed to build cached control: {Error}", e.Message);
            return node;
        }
    }

    /// <summary>
    /// Get children with pre-cached properties.
    /// </summary>
    /// <param name="node">The parent control.</param>
    /// <param name="cacheRequest">Optional custom cache request. If null, uses tree traversal cache.</param>
    /// <param name="logger">Optional logger.</param>
    /// <returns>List of children with cached properties.</returns>
    public static List<Control> GetCachedChildren(Control node, CacheRequest? cacheRequest = null, ILogger? logger = null)
    {
        cacheRequest ??= CacheRequestFactory.CreateTreeTraversalCache();

        if ((cacheRequest.TreeScopeValue & TreeScope.TreeScope_Children) == 0)
        {
            logger?.LogWarning("Cache request passed to GetCachedChildren does not have Children scope!");
        }

        try
        {
            var cachedNode = node.BuildUpdatedCache(cacheRequest);
            var children = cachedNode.GetCachedChildren();

            logger?.LogDebug("Retrieved {Count} cached children (newly built)", children.Count);
            return children;
        }
        catch (Exception e)
        {
            logger?.LogDebug("Failed to get cached children, falling back to regular access: {Error}", e.Message);
            return node.GetChildren();
        }
    }
}
