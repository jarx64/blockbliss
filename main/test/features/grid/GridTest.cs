using System;
using Godot;

public class GridTest
{
    private const string SourceClassPath = "res://features/grid/Grid.cs";

    public void TestIndexToPosition()
    {
        AssertEqual(Grid.IndexToPosition(0), Vector2.Zero);
        AssertEqual(Grid.IndexToPosition(1), new Vector2(32, 0));
        AssertEqual(Grid.IndexToPosition(2), new Vector2(64, 0));
        AssertEqual(Grid.IndexToPosition(10), new Vector2(0, 32));
        AssertEqual(Grid.IndexToPosition(11), new Vector2(32, 32));
    }

    public void TestAddressToPosition()
    {
        AssertEqual(Grid.AddressToPosition(Vector2I.Zero), Vector2.Zero);
        AssertEqual(Grid.AddressToPosition(new Vector2I(1, 0)), new Vector2(32, 0));
        AssertEqual(Grid.AddressToPosition(new Vector2I(2, 0)), new Vector2(64, 0));
        AssertEqual(Grid.AddressToPosition(new Vector2I(0, 1)), new Vector2(0, 32));
        AssertEqual(Grid.AddressToPosition(new Vector2I(1, 1)), new Vector2(32, 32));
    }

    public void TestTileToColor()
    {
        AssertEqual(Grid.TileToColor(0), Colors.White);
        AssertEqual(Grid.TileToColor(1), Grid.Colors[1]);
    }

    public void TestGetIndexBelowIndex()
    {
        AssertEqual(Grid.GetIndexBelowIndex(0), 10);
        AssertEqual(Grid.GetIndexBelowIndex(1), 11);
    }

    public void TestIndexToVector2I()
    {
        AssertEqual(Grid.IndexToVector2I(0), Vector2I.Zero);
        AssertEqual(Grid.IndexToVector2I(1), new Vector2I(1, 0));
        AssertEqual(Grid.IndexToVector2I(10), new Vector2I(0, 1));
        AssertEqual(Grid.IndexToVector2I(11), new Vector2I(1, 1));
    }

    public void TestVector2IToIndex()
    {
        AssertEqual(Grid.Vector2IToIndex(Vector2I.Zero), 0);
        AssertEqual(Grid.Vector2IToIndex(new Vector2I(1, 0)), 1);
        AssertEqual(Grid.Vector2IToIndex(new Vector2I(0, 1)), 10);
        AssertEqual(Grid.Vector2IToIndex(new Vector2I(1, 1)), 11);
    }

    private static void AssertEqual<T>(T actual, T expected)
    {
        if (!Equals(actual, expected))
        {
            throw new InvalidOperationException($"Expected {expected}, got {actual}.");
        }
    }
}
