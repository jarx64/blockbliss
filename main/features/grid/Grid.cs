using Godot;

public static class Grid
{
    public static readonly Color[] Colors =
    {
        Godot.Colors.White,
        ColorFromRgba(0x7776BCFF),
        ColorFromRgba(0xCDC7E8FF),
        ColorFromRgba(0xFFFBDBFF),
        ColorFromRgba(0xFFEC51FF),
        ColorFromRgba(0xFF674DFF),
        ColorFromRgba(0x456990FF),
    };

    public static Vector2 IndexToPosition(int index)
    {
        return new Vector2(index % GameConstants.Width, index / GameConstants.Width) * GameConstants.TileSize;
    }

    public static Vector2 AddressToPosition(Vector2I address)
    {
        return new Vector2(address.X, address.Y) * GameConstants.TileSize;
    }

    public static Color TileToColor(int tile)
    {
        return Colors[tile];
    }

    public static int GetIndexBelowIndex(int index)
    {
        return index + GameConstants.Width;
    }

    public static Vector2I IndexToVector2I(int index)
    {
        return new Vector2I(index % GameConstants.Width, index / GameConstants.Width);
    }

    public static int Vector2IToIndex(Vector2I address)
    {
        return address.Y * GameConstants.Width + address.X;
    }

    private static Color ColorFromRgba(uint rgba)
    {
        return new Color(
            ((rgba >> 24) & 0xFF) / 255.0f,
            ((rgba >> 16) & 0xFF) / 255.0f,
            ((rgba >> 8) & 0xFF) / 255.0f,
            (rgba & 0xFF) / 255.0f);
    }
}
