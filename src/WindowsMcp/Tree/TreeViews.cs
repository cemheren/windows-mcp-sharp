using System.Text;

namespace WindowsMcp.Tree;

public class TreeState
{
    public TreeElementNode? RootNode { get; set; }
    public ScrollElementNode? DomNode { get; set; }
    public List<TreeElementNode> InteractiveNodes { get; set; } = [];
    public List<ScrollElementNode> ScrollableNodes { get; set; } = [];
    public List<TextElementNode> DomInformativeNodes { get; set; } = [];

    public string InteractiveElementsToString()
    {
        if (InteractiveNodes.Count == 0)
            return "No interactive elements";

        var sb = new StringBuilder();
        sb.AppendLine("# id|window|control_type|name|coords|focus");
        for (var idx = 0; idx < InteractiveNodes.Count; idx++)
        {
            var node = InteractiveNodes[idx];
            sb.AppendLine($"{idx}|{node.WindowName}|{node.ControlType}|{node.Name}|{node.Center.ToFormattedString()}|{node.IsFocused}");
        }

        return sb.ToString().TrimEnd();
    }

    public string ScrollableElementsToString()
    {
        if (ScrollableNodes.Count == 0)
            return "No scrollable elements";

        var sb = new StringBuilder();
        sb.AppendLine("# id|window|control_type|name|coords|h_scroll|h_pct|v_scroll|v_pct|focus");
        var baseIndex = InteractiveNodes.Count;
        for (var idx = 0; idx < ScrollableNodes.Count; idx++)
        {
            var node = ScrollableNodes[idx];
            sb.AppendLine(
                $"{baseIndex + idx}|{node.WindowName}|{node.ControlType}|{node.Name}|" +
                $"{node.Center.ToFormattedString()}|{node.HorizontalScrollable}|{node.HorizontalScrollPercent}|" +
                $"{node.VerticalScrollable}|{node.VerticalScrollPercent}|{node.IsFocused}");
        }

        return sb.ToString().TrimEnd();
    }
}

public record BoundingBox(int Left, int Top, int Right, int Bottom, int Width, int Height)
{
    public static BoundingBox FromBoundingRectangle(dynamic boundingRectangle)
    {
        return new BoundingBox(
            Left: boundingRectangle.Left,
            Top: boundingRectangle.Top,
            Right: boundingRectangle.Right,
            Bottom: boundingRectangle.Bottom,
            Width: boundingRectangle.Width(),
            Height: boundingRectangle.Height());
    }

    public Center GetCenter()
    {
        return new Center(X: Left + Width / 2, Y: Top + Height / 2);
    }

    public string XywhToString()
    {
        return $"({Left},{Top},{Width},{Height})";
    }

    public string XyxyToString()
    {
        var (x1, y1, x2, y2) = ConvertXywhToXyxy();
        return $"({x1},{y1},{x2},{y2})";
    }

    public (int X1, int Y1, int X2, int Y2) ConvertXywhToXyxy()
    {
        return (Left, Top, Left + Width, Top + Height);
    }
}

public record Center(int X, int Y)
{
    public string ToFormattedString()
    {
        return $"({X},{Y})";
    }
}

public class TreeElementNode
{
    public required BoundingBox BoundingBox { get; set; }
    public required Center Center { get; set; }
    public string Name { get; set; } = "";
    public string ControlType { get; set; } = "";
    public string WindowName { get; set; } = "";
    public string Value { get; set; } = "";
    public string Shortcut { get; set; } = "";
    public string Xpath { get; set; } = "";
    public bool IsFocused { get; set; }

    public void UpdateFromNode(TreeElementNode node)
    {
        Name = node.Name;
        ControlType = node.ControlType;
        WindowName = node.WindowName;
        Value = node.Value;
        Shortcut = node.Shortcut;
        BoundingBox = node.BoundingBox;
        Center = node.Center;
        Xpath = node.Xpath;
        IsFocused = node.IsFocused;
    }

    public object[] ToRow(int index)
    {
        return [index, WindowName, ControlType, Name, Value, Shortcut, Center.ToFormattedString(), IsFocused];
    }
}

public class ScrollElementNode
{
    public required string Name { get; set; }
    public required string ControlType { get; set; }
    public required string Xpath { get; set; }
    public required string WindowName { get; set; }
    public required BoundingBox BoundingBox { get; set; }
    public required Center Center { get; set; }
    public bool HorizontalScrollable { get; set; }
    public double HorizontalScrollPercent { get; set; }
    public bool VerticalScrollable { get; set; }
    public double VerticalScrollPercent { get; set; }
    public bool IsFocused { get; set; }

    public object[] ToRow(int index, int baseIndex)
    {
        return
        [
            baseIndex + index, WindowName, ControlType, Name,
            Center.ToFormattedString(), HorizontalScrollable, HorizontalScrollPercent,
            VerticalScrollable, VerticalScrollPercent, IsFocused,
        ];
    }
}

public class TextElementNode
{
    public required string Text { get; set; }
}
