using Godot;

public partial class ScoreController : Node
{
    [Signal]
    public delegate void OnScoreUpdatedEventHandler(int value);

    private GameStateHolder _gameStateHolder;

    public void BindServices(GameStateHolder gameStateHolder)
    {
        _gameStateHolder = gameStateHolder;
    }

    public void Setup()
    {
        EmitSignal(SignalName.OnScoreUpdated, _gameStateHolder?.GameState?.Score ?? 0);
    }

    public void AddScore(int amount)
    {
        _gameStateHolder.GameState.Score += amount;
        EmitSignal(SignalName.OnScoreUpdated, _gameStateHolder.GameState.Score);
    }

    public void ResetScore()
    {
        _gameStateHolder.GameState.Score = 0;
        EmitSignal(SignalName.OnScoreUpdated, _gameStateHolder.GameState.Score);
    }
}
