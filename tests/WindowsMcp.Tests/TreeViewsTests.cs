using WindowsMcp.Tree;
using Xunit;

namespace WindowsMcp.Tests;

public class BoundingBoxTests
{
    [Fact]
    public void GetCenter_Standard()
    {
        var bb = TestFixtures.CreateSampleBoundingBox();
        var center = bb.GetCenter();
        Assert.Equal(200, center.X);
        Assert.Equal(100, center.Y);
    }

    [Fact]
    public void GetCenter_ZeroSize()
    {
        var bb = new BoundingBox(Left: 50, Top: 50, Right: 50, Bottom: 50, Width: 0, Height: 0);
        var center = bb.GetCenter();
        Assert.Equal(50, center.X);
        Assert.Equal(50, center.Y);
    }

    [Fact]
    public void GetCenter_NegativeCoords()
    {
        var bb = new BoundingBox(Left: -100, Top: -200, Right: 100, Bottom: 0, Width: 200, Height: 200);
        var center = bb.GetCenter();
        Assert.Equal(0, center.X);
        Assert.Equal(-100, center.Y);
    }

    [Fact]
    public void XywhToString_Format()
    {
        var bb = TestFixtures.CreateSampleBoundingBox();
        Assert.Equal("(100,50,200,100)", bb.XywhToString());
    }

    [Fact]
    public void XyxyToString_Format()
    {
        var bb = TestFixtures.CreateSampleBoundingBox();
        Assert.Equal("(100,50,300,150)", bb.XyxyToString());
    }

    [Fact]
    public void ConvertXywhToXyxy_Values()
    {
        var bb = TestFixtures.CreateSampleBoundingBox();
        var (x1, y1, x2, y2) = bb.ConvertXywhToXyxy();
        Assert.Equal(100, x1);
        Assert.Equal(50, y1);
        Assert.Equal(300, x2);
        Assert.Equal(150, y2);
    }

    [Fact]
    public void FromBoundingRectangle_Values()
    {
        var mockRect = new MockBoundingRect
        {
            Left = 10, Top = 20, Right = 110, Bottom = 70
        };
        var bb = BoundingBox.FromBoundingRectangle(mockRect);
        Assert.Equal(10, bb.Left);
        Assert.Equal(20, bb.Top);
        Assert.Equal(110, bb.Right);
        Assert.Equal(70, bb.Bottom);
        Assert.Equal(100, bb.Width);
        Assert.Equal(50, bb.Height);
    }

    public class MockBoundingRect
    {
        public int Left { get; init; }
        public int Top { get; init; }
        public int Right { get; init; }
        public int Bottom { get; init; }
        public int Width() => Right - Left;
        public int Height() => Bottom - Top;
    }
}

public class CenterTests
{
    [Fact]
    public void ToFormattedString_Standard()
    {
        var c = TestFixtures.CreateSampleCenter();
        Assert.Equal("(200,100)", c.ToFormattedString());
    }

    [Fact]
    public void ToFormattedString_Negative()
    {
        var c = new Center(X: -10, Y: -20);
        Assert.Equal("(-10,-20)", c.ToFormattedString());
    }
}

public class TreeStateTests
{
    [Fact]
    public void InteractiveElementsToString_Empty()
    {
        var ts = new TreeState();
        Assert.Equal("No interactive elements", ts.InteractiveElementsToString());
    }

    [Fact]
    public void InteractiveElementsToString_WithElements()
    {
        var node = TestFixtures.CreateSampleTreeElementNode();
        var ts = new TreeState { InteractiveNodes = [node] };
        var result = ts.InteractiveElementsToString();
        var lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        Assert.Equal("# id|window|control_type|name|coords|focus", lines[0]);
        Assert.Equal("0|Notepad|Button|OK|(200,100)|True", lines[1]);
    }

    [Fact]
    public void InteractiveElementsToString_Indices()
    {
        var node1 = TestFixtures.CreateSampleTreeElementNode();
        var node2 = new TreeElementNode
        {
            BoundingBox = node1.BoundingBox,
            Center = node1.Center,
            Name = "Cancel",
            ControlType = "Button",
            WindowName = "Notepad",
        };
        var ts = new TreeState { InteractiveNodes = [node1, node2] };
        var result = ts.InteractiveElementsToString();
        var lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        Assert.StartsWith("0|", lines[1]);
        Assert.StartsWith("1|", lines[2]);
    }

    [Fact]
    public void ScrollableElementsToString_Empty()
    {
        var ts = new TreeState();
        Assert.Equal("No scrollable elements", ts.ScrollableElementsToString());
    }

    [Fact]
    public void ScrollableElementsToString_WithElements()
    {
        var interactive = TestFixtures.CreateSampleTreeElementNode();
        var scrollable = TestFixtures.CreateSampleScrollElementNode();
        var ts = new TreeState
        {
            InteractiveNodes = [interactive],
            ScrollableNodes = [scrollable],
        };
        var result = ts.ScrollableElementsToString();
        var lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        Assert.Equal("# id|window|control_type|name|coords|h_scroll|h_pct|v_scroll|v_pct|focus", lines[0]);
        // base_index = len(interactive_nodes) = 1
        Assert.StartsWith("1|", lines[1]);
    }

    [Fact]
    public void ScrollableElementsToString_BaseIndexOffset()
    {
        var bb = new BoundingBox(Left: 0, Top: 0, Right: 10, Bottom: 10, Width: 10, Height: 10);
        var c = new Center(X: 5, Y: 5);
        var interactive = Enumerable.Range(0, 3)
            .Select(i => new TreeElementNode { BoundingBox = bb, Center = c, Name = $"btn{i}" })
            .ToList();
        var scrollable = TestFixtures.CreateSampleScrollElementNode();
        var ts = new TreeState
        {
            InteractiveNodes = interactive,
            ScrollableNodes = [scrollable],
        };
        var result = ts.ScrollableElementsToString();
        var lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        // base_index = 3 (three interactive nodes)
        Assert.StartsWith("3|", lines[1]);
    }
}

public class TreeElementNodeTests
{
    [Fact]
    public void ToRow_Values()
    {
        var node = TestFixtures.CreateSampleTreeElementNode();
        var row = node.ToRow(0);
        Assert.Equal(new object[] { 0, "Notepad", "Button", "OK", "", "Alt+O", "(200,100)", true }, row);
    }

    [Fact]
    public void UpdateFromNode_CopiesAllProperties()
    {
        var source = TestFixtures.CreateSampleTreeElementNode();
        var target = new TreeElementNode
        {
            BoundingBox = new BoundingBox(Left: 0, Top: 0, Right: 0, Bottom: 0, Width: 0, Height: 0),
            Center = new Center(X: 0, Y: 0),
        };
        target.UpdateFromNode(source);
        Assert.Equal("OK", target.Name);
        Assert.Equal("Button", target.ControlType);
        Assert.Equal("Notepad", target.WindowName);
        Assert.Equal("", target.Value);
        Assert.Equal("Alt+O", target.Shortcut);
        Assert.Equal("/Pane/Button", target.Xpath);
        Assert.True(target.IsFocused);
        Assert.Same(source.BoundingBox, target.BoundingBox);
        Assert.Same(source.Center, target.Center);
    }
}

public class ScrollElementNodeTests
{
    [Fact]
    public void ToRow_WithBaseIndex()
    {
        var node = TestFixtures.CreateSampleScrollElementNode();
        var row = node.ToRow(index: 0, baseIndex: 5);
        Assert.Equal(5, row[0]);        // base_index + index
        Assert.Equal("Notepad", row[1]);
        Assert.Equal("Pane", row[2]);
        Assert.Equal("Document", row[3]);
        Assert.Equal("(200,100)", row[4]);
        Assert.Equal(false, row[5]);    // HorizontalScrollable
        Assert.Equal(0.0, row[6]);      // HorizontalScrollPercent
        Assert.Equal(true, row[7]);     // VerticalScrollable
        Assert.Equal(42.5, row[8]);     // VerticalScrollPercent
        Assert.Equal(false, row[9]);    // IsFocused
    }
}
