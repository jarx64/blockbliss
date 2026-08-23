public partial class HighScore : Godot.RefCounted
{
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }

    public HighScore()
    {
    }

    public HighScore(string name, int score)
    {
        Name = name;
        Score = score;
    }

    public Godot.Collections.Dictionary ToDictionary()
    {
        return new Godot.Collections.Dictionary
        {
            ["name"] = Name,
            ["score"] = Score,
        };
    }
}
