using System;
using Godot;

public partial class EndScreen : PanelContainer
{
    [Signal]
    public delegate void RequestDismissLossEventHandler();

    private const int MaxNameInput = 10;

    [Export]
    public Button DismissButton { get; set; }

    [Export]
    public Label ScoreLabel { get; set; }

    [Export]
    public LineEdit NameInput { get; set; }

    private HighScores _highScores;
    private GameStateHolder _gameStateHolder;

    public void Initialize(HighScores highScores, GameStateHolder gameStateHolder)
    {
        BindServices(highScores, gameStateHolder);
        BindEvents();
        Hide();
    }

    public void BindServices(HighScores highScores, GameStateHolder gameStateHolder)
    {
        _highScores = highScores;
        _gameStateHolder = gameStateHolder;
    }

    public void BindEvents()
    {
        if (DismissButton != null)
        {
            DismissButton.Pressed -= DismissLoss;
            DismissButton.Pressed += DismissLoss;
        }
    }

    public void HandleGameLost()
    {
        Visible = true;

        if (ScoreLabel != null)
        {
            ScoreLabel.Text = _gameStateHolder.GameState.Score.ToString();
        }
    }

    public void DismissLoss()
    {
        Visible = false;

        string playerName = NameInput?.Text ?? string.Empty;
        string trimmedName = playerName[..Math.Min(playerName.Length, MaxNameInput)];
        _highScores.AddScore(new HighScore(trimmedName, _gameStateHolder.GameState.Score));
        EmitSignal(SignalName.RequestDismissLoss);
    }
}
