using System.Linq;
using Godot;

public partial class GridController : Node
{
    [Signal]
    public delegate void OnNewNextShapeEventHandler(Shape shape);

    [Signal]
    public delegate void OnRowClearEventHandler(int row, Godot.Collections.Array<int> values);

    [Signal]
    public delegate void RequestAddScoreEventHandler(int amount);

    private GameStateHolder _gameStateHolder;
    private ShapeLibrary _shapeLibrary;
    private SfxPlayer _sfx;

    private GameState State => _gameStateHolder.GetState();

    public void BindServices(GameStateHolder gameStateHolder, ShapeLibrary shapeLibrary, SfxPlayer sfx)
    {
        _gameStateHolder = gameStateHolder;
        _shapeLibrary = shapeLibrary;
        _sfx = sfx;
    }

    public void MoveActiveBlocks(Vector2I direction)
    {
        if (CanMoveActiveBlocks(direction))
        {
            State.CurrentActiveShape.Offset += direction;
            State.LastSlideTime = State.ActiveTime;
        }
    }

    public bool CanMoveActiveBlocks(Vector2I direction)
    {
        GameState state = State;
        Shape activeShape = state.CurrentActiveShape;

        if (state.ActiveTime <= state.LastSlideTime + GameConstants.SlideTime || activeShape == null)
        {
            return false;
        }

        return !activeShape
            .GetTilesWithRotationAndOffset(activeShape.Rotation, activeShape.Offset + direction)
            .Any(IsPositionIllegal);
    }

    public void RotateActiveBlocks()
    {
        if (CanRotateActiveBlocks())
        {
            State.CurrentActiveShape.Rotate();
        }
    }

    public bool CanRotateActiveBlocks()
    {
        Shape activeShape = State.CurrentActiveShape;
        if (activeShape == null)
        {
            return false;
        }

        return !activeShape
            .GetTilesWithRotationAndOffset(activeShape.Rotation + 1, activeShape.Offset)
            .Any(IsPositionIllegal);
    }

    public bool IsPositionIllegal(Vector2I tile)
    {
        return tile.X >= GameConstants.Width
            || tile.X < 0
            || tile.Y >= GameConstants.Height
            || tile.Y < 0
            || State.Grid[Grid.Vector2IToIndex(tile)] > 0;
    }

    public bool IsTouchingGround(Vector2I tile)
    {
        if (tile.Y >= GameConstants.Height - 1)
        {
            return true;
        }

        int tileIndex = Grid.Vector2IToIndex(tile);
        int belowIndex = Grid.GetIndexBelowIndex(tileIndex);
        return State.Grid[belowIndex] != 0;
    }

    public bool IsCurrentShapeTouchingGround()
    {
        return State.CurrentActiveShape?.Tiles.Any(IsTouchingGround) ?? false;
    }

    public Godot.Collections.Array<Vector2I> GetDropPreviewTiles()
    {
        Godot.Collections.Array<Vector2I> tiles = State.CurrentActiveShape.Tiles;
        int dropY = GameConstants.Height - 1;

        for (int y = 0; y < GameConstants.Height; y++)
        {
            foreach (Vector2I tile in tiles)
            {
                if (IsTouchingGround(tile + new Vector2I(0, y)))
                {
                    dropY = y;
                    break;
                }
            }

            if (dropY < GameConstants.Height - 1)
            {
                break;
            }
        }

        Godot.Collections.Array<Vector2I> previewTiles = new();
        foreach (Vector2I tile in tiles)
        {
            previewTiles.Add(tile + new Vector2I(0, dropY));
        }

        return previewTiles;
    }

    public void ConvertActiveTilesToGrid()
    {
        Shape activeShape = State.CurrentActiveShape;
        if (activeShape == null)
        {
            return;
        }

        foreach (Vector2I tile in activeShape.Tiles)
        {
            int tileIndex = Grid.Vector2IToIndex(tile);
            State.Grid[tileIndex] = activeShape.Resource.Color;
        }

        State.CurrentActiveShape = null;
    }

    public void GenerateNewActiveTileShape()
    {
        State.CurrentActiveShape = State.NextActiveShape;
        ShapeResource shape = _shapeLibrary.GetRandomShape();

        if (shape != null)
        {
            State.NextActiveShape = new Shape(new Vector2I(5, 0), shape);
            EmitSignal(SignalName.OnNewNextShape, State.NextActiveShape);
        }
    }

    public bool CanClearRow(int row)
    {
        int startIndex = row * GameConstants.Width;

        for (int i = 0; i < GameConstants.Width; i++)
        {
            if (State.Grid[i + startIndex] <= 0)
            {
                return false;
            }
        }

        return true;
    }

    public void CheckAndClearRows()
    {
        for (int i = 0; i < GameConstants.Height; i++)
        {
            if (CanClearRow(i))
            {
                _sfx.RequestSound(SfxPlayer.SoundKey.Clear);
                ClearRow(i);
            }
        }
    }

    public void ClearRow(int row)
    {
        Godot.Collections.Array<int> oldRow = new();
        int rowStart = row * GameConstants.Width;

        for (int i = 0; i < GameConstants.Width; i++)
        {
            oldRow.Add(State.Grid[rowStart + i]);
        }

        EmitSignal(SignalName.OnRowClear, row, oldRow);

        Godot.Collections.Array<int> newGrid = new();
        for (int i = 0; i < GameConstants.Width; i++)
        {
            newGrid.Add((int)GameConstants.Tile.None);
        }

        for (int i = 0; i < rowStart; i++)
        {
            newGrid.Add(State.Grid[i]);
        }

        for (int i = rowStart + GameConstants.Width; i < GameConstants.TileCount; i++)
        {
            newGrid.Add(State.Grid[i]);
        }

        State.Grid = newGrid;
        EmitSignal(SignalName.RequestAddScore, 100);
    }
}
