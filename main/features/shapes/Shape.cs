using Godot;

public partial class Shape : RefCounted
{
    public ShapeResource Resource { get; set; }
    public Vector2I Offset { get; set; }
    public int Rotation { get; set; }

    public Godot.Collections.Array<Vector2I> Tiles => GetTilesWithRotationAndOffset(Rotation, Offset);

    public Shape()
    {
    }

    public Shape(Vector2I offset, ShapeResource resource)
    {
        Offset = offset;
        Resource = resource;
    }

    public void Rotate()
    {
        Rotation = (Rotation + 1) % 4;
    }

    public Godot.Collections.Array<Vector2I> GetTilesWithRotationAndOffset(int rotation, Vector2I offset)
    {
        Godot.Collections.Array<Vector2I> tiles = new();

        if (Resource == null)
        {
            return tiles;
        }

        int normalizedRotation = NormalizeRotation(rotation);
        foreach (Vector2I tile in Resource.Offsets)
        {
            tiles.Add(RotateTile(tile, normalizedRotation) + offset);
        }

        return tiles;
    }

    public Vector2I RotateTile(Vector2I tile, int rotation)
    {
        return NormalizeRotation(rotation) switch
        {
            1 => new Vector2I(-tile.Y, tile.X),
            2 => new Vector2I(-tile.X, -tile.Y),
            3 => new Vector2I(tile.Y, -tile.X),
            _ => tile,
        };
    }

    private static int NormalizeRotation(int rotation)
    {
        return ((rotation % 4) + 4) % 4;
    }
}
