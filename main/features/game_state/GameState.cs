using Godot;

public partial class GameState : RefCounted
{
    public enum GameStatus
    {
        NotStarted,
        Active,
        Paused,
        Lost,
    }

    public GameStatus Status { get; set; } = GameStatus.NotStarted;
    public GameStatus LastStatus { get; set; } = GameStatus.NotStarted;
    public float ActiveTime { get; set; }
    public float LastDropTime { get; set; }
    public float LastSlideTime { get; set; }
    public Shape CurrentActiveShape { get; set; }
    public Shape NextActiveShape { get; set; }
    public float Gravity { get; set; } = 0.5f;
    public bool SpeedUp { get; set; }
    public int Score { get; set; }
    public Godot.Collections.Array<int> Grid { get; set; } = new();

    public float DifficultyCoefficient => 1.0f + (ActiveTime / GameConstants.DifficultyRamp);

    public float TotalGravity => SpeedUp
        ? Gravity / DifficultyCoefficient / GameConstants.TileDropSpeedup
        : Gravity / DifficultyCoefficient;

    public void Initialize()
    {
        Status = GameStatus.NotStarted;
        LastStatus = GameStatus.NotStarted;
        ActiveTime = 0.0f;
        LastDropTime = 0.0f;
        LastSlideTime = 0.0f;
        CurrentActiveShape = null;
        NextActiveShape = null;
        SpeedUp = false;
        Score = 0;

        Grid.Clear();
        for (int i = 0; i < GameConstants.TileCount; i++)
        {
            Grid.Add((int)GameConstants.Tile.None);
        }
    }
}
