public static class GameConstants
{
    public const int Height = 20;
    public const int Width = 10;
    public const int TileCount = Height * Width;
    public const float TileDropSpeedup = 12.0f;
    public const int TileSize = 32;
    public const float DifficultyRamp = 25.0f;
    public const float SlideTime = 0.1f;

    public enum GameStatus
    {
        NotStarted,
        Active,
        Paused,
        Finished,
    }

    public enum Tile
    {
        None,
        Red,
        Blue,
        Yellow,
    }
}
