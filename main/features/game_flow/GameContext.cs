using Godot;

public partial class GameContext : Node
{
    [Signal]
    public delegate void OnGameLossEventHandler();

    [Export]
    public GridRenderer Renderer { get; set; }

    [Export]
    public PreviewRenderer PreviewRenderer { get; set; }

    [Export]
    public GameOverlay Overlay { get; set; }

    [Export]
    public PackedScene ShapeLibraryPacked { get; set; }

    [Export]
    public PackedScene CameraPacked { get; set; }

    private GameController _gameController;
    private GridController _gridController;
    private ScoreController _scoreController;
    private SfxPlayer _sfx;
    private MusicPlayer _music;
    private ShapeLibrary _shapeLibrary;
    private GameStateHolder _gameStateHolder;
    private GameCamera _camera;

    public void BuildServices()
    {
        _gameController = new GameController();
        AddChild(_gameController);

        _gridController = new GridController();
        AddChild(_gridController);

        _scoreController = new ScoreController();
        AddChild(_scoreController);

        _shapeLibrary = ShapeLibraryPacked.Instantiate<ShapeLibrary>();
        AddChild(_shapeLibrary);

        _camera = CameraPacked.Instantiate<GameCamera>();
        AddChild(_camera);
    }

    public void BindServices(SfxPlayer sfx, MusicPlayer music, GameStateHolder gameStateHolder)
    {
        _sfx = sfx;
        _music = music;
        _gameStateHolder = gameStateHolder;

        _gameController.BindServices(_scoreController, _gridController, gameStateHolder, Renderer, _sfx, _music);
        _gridController.BindServices(gameStateHolder, _shapeLibrary, _sfx);
        _scoreController.BindServices(gameStateHolder);
        _camera.Initialize();

        _gridController.OnRowClear += _camera.ShakeScreen;
        _gameController.OnGameLoss += () => EmitSignal(SignalName.OnGameLoss);
        _gridController.RequestAddScore += _scoreController.AddScore;

        Renderer.BindServices(_gridController, _gameStateHolder);
        PreviewRenderer.BindServices(_gridController, _gameStateHolder);
        Overlay.Initialize(_scoreController, _gridController);
    }

    public void HandleStartNewGame()
    {
        _gameStateHolder.GameState = new GameState();
        _gameStateHolder.GameState.Initialize();
        _gameController.Start();

        Renderer.Initialize();
        Renderer.Update();

        _scoreController.ResetScore();
        _music.PlayGameTrack();
    }
}
