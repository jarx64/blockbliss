using Godot;

public partial class MenuOverlay : Node
{
    [Signal]
    public delegate void OnDismissLossEventHandler();

    [Signal]
    public delegate void OnDismissHighscoresEventHandler();

    [Export]
    public MainMenu MainMenu { get; set; }

    [Export]
    public HighScoresMenu HighScoresMenu { get; set; }

    [Export]
    public EndScreen EndScreen { get; set; }

    private MusicPlayer _music;
    private GameStateHolder _gameStateHolder;

    public void Initialize(MusicPlayer music, SfxPlayer sfx, HighScores highScores, GameStateHolder gameStateHolder)
    {
        _music = music;
        _gameStateHolder = gameStateHolder;

        MainMenu.Initialize(music, sfx);
        HighScoresMenu.Initialize(highScores);
        EndScreen.Initialize(highScores, gameStateHolder);

        MainMenu.RequestScores += HighScoresMenu.ShowMenu;
        EndScreen.RequestDismissLoss += () => EmitSignal(SignalName.OnDismissLoss);
        OnDismissHighscores -= HandleDismissHighscores;
        OnDismissHighscores += HandleDismissHighscores;

        Update();
    }

    public void HandleDismissHighscores()
    {
        HighScoresMenu.Hide();
        MainMenu.ShowMenu();
    }

    public void Update()
    {
        if (_gameStateHolder.GameState != null && _gameStateHolder.GameState.Status == GameState.GameStatus.Lost)
        {
            EndScreen.Show();
            MainMenu.Hide();
            HighScoresMenu.Hide();
        }
        else
        {
            EndScreen.Hide();
            MainMenu.Show();
            HighScoresMenu.Hide();
        }
    }
}
