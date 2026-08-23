using Godot;

public partial class GameStateHolder : Node
{
    public GameState GameState { get; set; }

    public GameState GetState()
    {
        return GameState;
    }

    public void Clear()
    {
        GameState = null;
    }
}
