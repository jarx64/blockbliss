using Godot;

public partial class PreviewRenderer : Node2D
{
    private readonly PackedScene _previewRendererTile = ResourceLoader.Load<PackedScene>("res://features/grid/preview_renderer.tscn");

    private GridController _gridController;
    private GameStateHolder _gameStateHolder;

    private GameState GameState => _gameStateHolder?.GameState;

    public override void _Ready()
    {
        Position = new Vector2(
            (GetWindow().Size.X / 2.0f) - (GameConstants.Width * GameConstants.TileSize / 2.0f),
            GameConstants.TileSize / 2.0f);
    }

    public override void _Process(double delta)
    {
        Clear();

        if (GameState?.CurrentActiveShape != null)
        {
            Render();
        }
    }

    public void BindServices(GridController gridController, GameStateHolder gameStateHolder)
    {
        _gridController = gridController;
        _gameStateHolder = gameStateHolder;
    }

    public void Render()
    {
        if (_gridController == null)
        {
            return;
        }

        foreach (Vector2I tileAddress in _gridController.GetDropPreviewTiles())
        {
            Sprite2D previewTile = _previewRendererTile.Instantiate<Sprite2D>();
            AddChild(previewTile);
            previewTile.Position = Grid.AddressToPosition(tileAddress);
        }
    }

    public void Clear()
    {
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }
    }
}
