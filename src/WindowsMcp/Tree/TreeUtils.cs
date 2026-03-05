using WindowsMcp.Uia;

namespace WindowsMcp.Tree;

/// <summary>
/// Utility methods for tree operations on UI Automation controls.
/// </summary>
public static class TreeUtils
{
    private static readonly Random Rng = new();

    /// <summary>
    /// Generate a random point within a scaled-down bounding box.
    /// </summary>
    /// <param name="node">The node with a bounding rectangle.</param>
    /// <param name="scaleFactor">The factor to scale down the bounding box. Defaults to 1.0.</param>
    /// <returns>A random point (x, y) within the scaled-down bounding box.</returns>
    public static (int X, int Y) RandomPointWithinBoundingBox(Control node, double scaleFactor = 1.0)
    {
        var box = node.BoundingRectangle;
        int scaledWidth = (int)(box.Width() * scaleFactor);
        int scaledHeight = (int)(box.Height() * scaleFactor);
        int scaledLeft = box.Left + (box.Width() - scaledWidth) / 2;
        int scaledTop = box.Top + (box.Height() - scaledHeight) / 2;
        int x = Rng.Next(scaledLeft, scaledLeft + scaledWidth + 1);
        int y = Rng.Next(scaledTop, scaledTop + scaledHeight + 1);
        return (x, y);
    }
}
