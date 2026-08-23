using Godot;

public partial class ScoreBox : PanelContainer
{
    [Export]
    public Label ScoreDisplayLabel { get; set; }

    private ScoreController _scoreController;

    public void Initialize(ScoreController scoreController)
    {
        BindServices(scoreController);
        BindEvents();
    }

    public void BindServices(ScoreController scoreController)
    {
        _scoreController = scoreController;
    }

    public void BindEvents()
    {
        if (_scoreController != null)
        {
            _scoreController.OnScoreUpdated -= UpdateScore;
            _scoreController.OnScoreUpdated += UpdateScore;
        }
    }

    public void UpdateScore(int value)
    {
        if (ScoreDisplayLabel != null)
        {
            ScoreDisplayLabel.Text = value.ToString().PadLeft(8, '0');
        }
    }
}
