using System;
using Godot;

public class GridControllerTest
{
    private const string SourceClassPath = "res://features/grid/GridController.cs";

    public void TestCanMoveActiveBlocksWhenTimeIntervalNotPassedReturnsFalse()
    {
        (GridController gridController, GameState gameState) = BuildController();
        gameState.ActiveTime = 0.01f;
        gameState.LastSlideTime = 0.01f;
        gameState.CurrentActiveShape = new Shape(new Vector2I(5, 5), BuildShapeResource());

        AssertFalse(gridController.CanMoveActiveBlocks(Vector2I.Left));
    }

    public void TestCanMoveActiveBlocksWhenNoCurrentShapeReturnsFalse()
    {
        (GridController gridController, GameState gameState) = BuildController();
        gameState.ActiveTime = GameConstants.SlideTime;
        gameState.LastSlideTime = 0.0f;
        gameState.CurrentActiveShape = null;

        AssertFalse(gridController.CanMoveActiveBlocks(Vector2I.Left));
    }

    public void TestCanMoveActiveBlocksWhenSatisfiedReturnsTrue()
    {
        (GridController gridController, GameState gameState) = BuildController();
        gameState.ActiveTime = GameConstants.SlideTime + 1.0f;
        gameState.LastSlideTime = 0.0f;
        gameState.CurrentActiveShape = new Shape(new Vector2I(3, 3), BuildShapeResource());

        AssertTrue(gridController.CanMoveActiveBlocks(Vector2I.Left));
    }

    public void TestMoveActiveBlocksWhenSuccessfulUpdatesShapeInState()
    {
        (GridController gridController, GameState gameState) = BuildController();
        gameState.ActiveTime = GameConstants.SlideTime + 1.0f;
        gameState.LastSlideTime = 0.1f;
        gameState.CurrentActiveShape = new Shape(new Vector2I(3, 3), BuildShapeResource());

        gridController.MoveActiveBlocks(Vector2I.Right);

        AssertEqual(gameState.CurrentActiveShape.Offset, new Vector2I(4, 3));
        AssertApproximately(gameState.LastSlideTime, GameConstants.SlideTime + 1.0f);
    }

    public void TestMoveActiveBlocksWhenUnsuccessfulDoesNotUpdateShapeInState()
    {
        (GridController gridController, GameState gameState) = BuildController();
        gameState.ActiveTime = 0.1f;
        gameState.LastSlideTime = 0.0f;
        gameState.CurrentActiveShape = new Shape(new Vector2I(3, 3), BuildShapeResource());

        gridController.MoveActiveBlocks(Vector2I.Right);

        AssertEqual(gameState.CurrentActiveShape.Offset, new Vector2I(3, 3));
        AssertApproximately(gameState.LastSlideTime, 0.0f);
    }

    private static (GridController, GameState) BuildController()
    {
        GameState gameState = new();
        gameState.Initialize();

        GameStateHolder gameStateHolder = new()
        {
            GameState = gameState,
        };

        GridController gridController = new();
        gridController.BindServices(gameStateHolder, new ShapeLibrary(), new SfxPlayer());

        return (gridController, gameState);
    }

    private static ShapeResource BuildShapeResource()
    {
        return new ShapeResource
        {
            Color = 3,
            Offsets = new Godot.Collections.Array<Vector2I>
            {
                new(0, 0),
                new(0, 1),
                new(0, 2),
                new(-1, 0),
            },
        };
    }

    private static void AssertTrue(bool value)
    {
        if (!value)
        {
            throw new InvalidOperationException("Expected true.");
        }
    }

    private static void AssertFalse(bool value)
    {
        if (value)
        {
            throw new InvalidOperationException("Expected false.");
        }
    }

    private static void AssertEqual<T>(T actual, T expected)
    {
        if (!Equals(actual, expected))
        {
            throw new InvalidOperationException($"Expected {expected}, got {actual}.");
        }
    }

    private static void AssertApproximately(float actual, float expected)
    {
        if (Math.Abs(actual - expected) > 0.0001f)
        {
            throw new InvalidOperationException($"Expected {expected}, got {actual}.");
        }
    }
}
