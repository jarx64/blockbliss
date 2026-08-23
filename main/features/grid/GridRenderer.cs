using Godot;

public partial class GridRenderer : Node2D
{
    private readonly PackedScene _tileRendererScene = ResourceLoader.Load<PackedScene>("res://features/grid/block_renderer.tscn");
    private readonly PackedScene _gridCellRendererScene = ResourceLoader.Load<PackedScene>("res://features/grid/grid_cell_renderer.tscn");
    private readonly PackedScene _destroyTileRendererParticles = ResourceLoader.Load<PackedScene>("res://features/vfx/destroy_particles.tscn");

    public Godot.Collections.Array<Sprite2D> RenderedTileRenderers { get; } = new();
    public Godot.Collections.Array<Sprite2D> DroppingTileRenderers { get; } = new();

    private GridController _gridController;
    private GameStateHolder _gameStateHolder;

    private GameState GameState => _gameStateHolder?.GameState;

    public void BindServices(GridController gridController, GameStateHolder gameStateHolder)
    {
        _gameStateHolder = gameStateHolder;
        _gridController = gridController;
    }

    public void Initialize()
    {
        Position = new Vector2(
            (GetWindow().Size.X / 2.0f) - (GameConstants.Width * GameConstants.TileSize / 2.0f),
            GameConstants.TileSize / 2.0f);

        EnsureRenderedTileSlots();
        DrawGrid();

        if (_gridController != null)
        {
            _gridController.OnRowClear += GenerateClearParticles;
        }
    }

    public void ClearGrid()
    {
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }

        for (int i = 0; i < RenderedTileRenderers.Count; i++)
        {
            RenderedTileRenderers[i]?.QueueFree();
            RenderedTileRenderers[i] = null;
        }

        foreach (Sprite2D tileRenderer in DroppingTileRenderers)
        {
            tileRenderer?.QueueFree();
        }

        DroppingTileRenderers.Clear();
    }

    public void DrawGrid()
    {
        ClearGrid();

        for (int x = 0; x < GameConstants.Width; x++)
        {
            for (int y = 0; y < GameConstants.Height; y++)
            {
                Node2D cell = _gridCellRendererScene.Instantiate<Node2D>();
                AddChild(cell);
                cell.Position = Grid.AddressToPosition(new Vector2I(x, y));
            }
        }
    }

    public void GenerateClearParticles(int row, Godot.Collections.Array<int> values)
    {
        for (int i = 0; i < GameConstants.Width; i++)
        {
            GpuParticles2D particles = _destroyTileRendererParticles.Instantiate<GpuParticles2D>();
            AddChild(particles);
            particles.Emitting = true;
            particles.Position = Grid.IndexToPosition(row * GameConstants.Width + i);
            particles.Modulate = Grid.TileToColor(values[i]);
            particles.Finished += particles.QueueFree;
        }
    }

    public void Update()
    {
        GameState gameState = GameState;
        if (gameState == null || gameState.Grid.Count < GameConstants.TileCount)
        {
            return;
        }

        EnsureRenderedTileSlots();

        if (gameState.CurrentActiveShape != null)
        {
            float percentageOfNextRowDropped = (gameState.ActiveTime - gameState.LastDropTime) / gameState.TotalGravity;
            bool grounded = _gridController.IsCurrentShapeTouchingGround();

            if (DroppingTileRenderers.Count != gameState.CurrentActiveShape.Tiles.Count)
            {
                foreach (Sprite2D droppingTileRenderer in DroppingTileRenderers)
                {
                    droppingTileRenderer?.QueueFree();
                }

                DroppingTileRenderers.Clear();

                foreach (Vector2I tile in gameState.CurrentActiveShape.Tiles)
                {
                    Sprite2D tileRenderer = _tileRendererScene.Instantiate<Sprite2D>();
                    AddChild(tileRenderer);
                    DroppingTileRenderers.Add(tileRenderer);
                }
            }

            for (int tileIndex = 0; tileIndex < gameState.CurrentActiveShape.Tiles.Count; tileIndex++)
            {
                Sprite2D droppingTileRenderer = DroppingTileRenderers[tileIndex];
                Color color = Grid.TileToColor(gameState.CurrentActiveShape.Resource.Color);

                if (droppingTileRenderer != null)
                {
                    Vector2 dropOffset = grounded
                        ? Vector2.Zero
                        : new Vector2(0.0f, percentageOfNextRowDropped * GameConstants.TileSize);

                    droppingTileRenderer.Position = Grid.AddressToPosition(gameState.CurrentActiveShape.Tiles[tileIndex]) + dropOffset;
                    droppingTileRenderer.Modulate = color;
                }
            }
        }

        for (int i = 0; i < GameConstants.TileCount; i++)
        {
            Sprite2D renderedTileRenderer = RenderedTileRenderers[i];
            int value = gameState.Grid[i];

            if (renderedTileRenderer == null && value > 0)
            {
                Sprite2D tileRenderer = _tileRendererScene.Instantiate<Sprite2D>();
                AddChild(tileRenderer);
                RenderedTileRenderers[i] = tileRenderer;
                tileRenderer.Position = Grid.IndexToPosition(i);
                tileRenderer.Modulate = Grid.TileToColor(value);
            }
            else if (renderedTileRenderer != null && value == 0)
            {
                renderedTileRenderer.QueueFree();
                RenderedTileRenderers[i] = null;
            }
            else if (renderedTileRenderer != null && renderedTileRenderer.Modulate != Grid.TileToColor(value))
            {
                renderedTileRenderer.Modulate = Grid.TileToColor(value);
            }
        }
    }

    private void EnsureRenderedTileSlots()
    {
        while (RenderedTileRenderers.Count < GameConstants.TileCount)
        {
            RenderedTileRenderers.Add(null);
        }
    }
}
