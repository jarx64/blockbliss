using Godot;

public partial class MenuContext : Node
{
    [Signal]
    public delegate void RequestQuitEventHandler();

    [Signal]
    public delegate void RequestStartGameEventHandler();

    [Signal]
    public delegate void RequestScoresEventHandler();

    [Export]
    public MenuOverlay Overlay { get; set; }

    [Export]
    public PackedScene CameraPacked { get; set; }

    private SfxPlayer _sfx;
    private MusicPlayer _music;
    private HighScores _highScores;
    private GameStateHolder _gameStateHolder;
    private GameCamera _camera;

    public void BuildServices()
    {
        _highScores = new HighScores();
        AddChild(_highScores);

        _camera = CameraPacked.Instantiate<GameCamera>();
        AddChild(_camera);
    }

    public void BindServices(SfxPlayer sfx, MusicPlayer music, GameStateHolder gameStateHolder)
    {
        _sfx = sfx;
        _music = music;
        _gameStateHolder = gameStateHolder;
    }

    public void Setup()
    {
        Overlay.Initialize(_music, _sfx, _highScores, _gameStateHolder);
        _camera.Initialize();

        if (_gameStateHolder.GameState != null && _gameStateHolder.GameState.Status == GameState.GameStatus.Lost)
        {
            _music.PlayLossTrack();
        }
        else
        {
            _music.PlayMenuTrack();
        }
    }

    public void ConnectSignals()
    {
        Overlay.MainMenu.RequestStartGame += () => EmitSignal(SignalName.RequestStartGame);
        Overlay.MainMenu.RequestQuit += () => EmitSignal(SignalName.RequestQuit);
        Overlay.OnDismissLoss += HandleDismissLoss;
        Overlay.HighScoresMenu.RequestDismiss += Overlay.HandleDismissHighscores;
    }

    public void HandleDismissLoss()
    {
        _gameStateHolder.Clear();
        _music.PlayMenuTrack();
        Overlay.Update();
    }
}
