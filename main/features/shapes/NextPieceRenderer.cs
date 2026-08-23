using Godot;

public partial class NextPieceRenderer : PanelContainer
{
    [Signal]
    public delegate void OnNewNextShapeEventHandler(Shape shape);

    [Export]
    public Control PieceFrame { get; set; }

    [Export]
    public Texture2D EmptyCellSprite { get; set; }

    [Export]
    public Texture2D TileSprite { get; set; }

    private const int Width = 5;
    private const int Height = 5;
    private const int Offset = 2;
    private static readonly Vector2 FrameOffset = new(16.0f, 0.0f);

    public void Initialize()
    {
        BindListeners();
    }

    public void BindListeners()
    {
        OnNewNextShape -= RenderNextPiece;
        OnNewNextShape += RenderNextPiece;
    }

    public void RenderNextPiece(Shape shape)
    {
        Clear();

        if (shape == null || PieceFrame == null)
        {
            return;
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Sprite2D cell = new();
                PieceFrame.AddChild(cell);
                cell.Position = (new Vector2(x, y) * GameConstants.TileSize)
                    + new Vector2(0.0f, 0.5f * GameConstants.TileSize)
                    + FrameOffset;

                if (ShapeHasTileAtPosition(shape, x - Offset, y - Offset))
                {
                    cell.Texture = TileSprite;
                    cell.Modulate = Grid.TileToColor(shape.Resource.Color);
                }
                else
                {
                    cell.Texture = EmptyCellSprite;
                }
            }
        }
    }

    public bool ShapeHasTileAtPosition(Shape piece, int x, int y)
    {
        if (piece?.Resource == null)
        {
            return false;
        }

        foreach (Vector2I tile in piece.Resource.Offsets)
        {
            if (tile.X == x && tile.Y == y)
            {
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        if (PieceFrame == null)
        {
            return;
        }

        foreach (Node child in PieceFrame.GetChildren())
        {
            child.QueueFree();
        }
    }
}
