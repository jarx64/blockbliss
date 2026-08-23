using System;

public class GridRendererTest
{
    private const string SourceClassPath = "res://features/grid/GridRenderer.cs";

    public void TestDrawGrid()
    {
        GridRenderer gridRenderer = new();

        gridRenderer.DrawGrid();

        if (gridRenderer.GetChildCount() != GameConstants.TileCount)
        {
            throw new InvalidOperationException($"Expected {GameConstants.TileCount} cells, got {gridRenderer.GetChildCount()}.");
        }

        gridRenderer.QueueFree();
    }
}
