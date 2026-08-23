using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

public partial class HighScores : Node
{
    private const int TopScoreCount = 3;
    private const string ScoresPath = "user://scores.json";

    public Godot.Collections.Array<HighScore> Scores { get; set; } = new();

    public override void _Ready()
    {
        LoadScores();
    }

    public void LoadScores()
    {
        Scores.Clear();

        using FileAccess file = FileAccess.Open(ScoresPath, FileAccess.ModeFlags.Read);
        if (file != null)
        {
            string jsonString = file.GetLine();
            HighScoreDto[] scoreDtos = JsonSerializer.Deserialize<HighScoreDto[]>(jsonString) ?? System.Array.Empty<HighScoreDto>();

            foreach (HighScoreDto scoreDto in scoreDtos)
            {
                Scores.Add(new HighScore(scoreDto.Name, scoreDto.Score));
            }
        }

        TrimAndSortScores();
    }

    public void SaveScores()
    {
        HighScoreDto[] scoreDtos = Scores
            .Where(score => score != null)
            .Select(score => new HighScoreDto { Name = score.Name, Score = score.Score })
            .ToArray();

        string jsonString = JsonSerializer.Serialize(scoreDtos);

        using FileAccess file = FileAccess.Open(ScoresPath, FileAccess.ModeFlags.Write);
        file?.StoreLine(jsonString);
    }

    public void AddScore(HighScore value)
    {
        Scores.Add(value);
        TrimAndSortScores();
        SaveScores();
    }

    public HighScore VariantDictionaryToHighScore(Godot.Collections.Dictionary dictionary)
    {
        if (!dictionary.ContainsKey("name") || !dictionary.ContainsKey("score"))
        {
            return null;
        }

        return new HighScore(dictionary["name"].AsString(), dictionary["score"].AsInt32());
    }

    private void TrimAndSortScores()
    {
        HighScore[] sortedScores = Scores
            .Where(score => score != null)
            .OrderByDescending(score => score.Score)
            .Take(TopScoreCount)
            .ToArray();

        Scores.Clear();
        foreach (HighScore score in sortedScores)
        {
            Scores.Add(score);
        }
    }

    private sealed class HighScoreDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public int Score { get; set; }
    }
}
