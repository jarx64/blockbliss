using Godot;

public partial class HighScoresMenu : PanelContainer
{
    [Signal]
    public delegate void RequestDismissEventHandler();

    [Export]
    public Button BackButton { get; set; }

    [Export]
    public Control ScoresParent { get; set; }

    private HighScores _highScores;

    public void Initialize(HighScores highScores)
    {
        BindEvents();
        BindServices(highScores);
        LoadScores();
        Hide();
    }

    public void BindEvents()
    {
        if (BackButton != null)
        {
            BackButton.Pressed -= HandleBackPressed;
            BackButton.Pressed += HandleBackPressed;
        }
    }

    public void BindServices(HighScores highScores)
    {
        _highScores = highScores;
    }

    public void ShowMenu()
    {
        LoadScores();
        Show();
    }

    public void HandleBackPressed()
    {
        Visible = false;
        EmitSignal(SignalName.RequestDismiss);
    }

    public void LoadScores()
    {
        if (ScoresParent == null || _highScores == null)
        {
            return;
        }

        foreach (Node child in ScoresParent.GetChildren())
        {
            child.QueueFree();
        }

        foreach (HighScore score in _highScores.Scores)
        {
            Label scoreLabel = new()
            {
                Text = score.Name.PadRight(12, '.') + "........" + score.Score.ToString().PadLeft(10, '0'),
            };

            ScoresParent.AddChild(scoreLabel);
        }
    }
}
