using Godot;

public partial class RootContext : Node
{
    [Export]
    public PackedScene MenuScenePacked { get; set; }

    [Export]
    public PackedScene GameScenePacked { get; set; }

    [Export]
    public PackedScene MusicPlayerPacked { get; set; }

    [Export]
    public PackedScene SfxPlayerPacked { get; set; }

    public Node CurrentScene { get; private set; }

    private SfxPlayer _sfx;
    private MusicPlayer _music;
    private GameStateHolder _gameStateHolder;

    public override void _Ready()
    {
        BuildServices();
        BindServices();
        GoToMenu();
    }

    public void BuildServices()
    {
        _sfx = SfxPlayerPacked.Instantiate<SfxPlayer>();
        AddChild(_sfx);

        _music = MusicPlayerPacked.Instantiate<MusicPlayer>();
        AddChild(_music);

        _gameStateHolder = new GameStateHolder();
        AddChild(_gameStateHolder);
    }

    public void BindServices()
    {
    }

    public void GoToMenu()
    {
        CurrentScene?.QueueFree();
        CurrentScene = MenuScenePacked.Instantiate();
        AddChild(CurrentScene);

        if (CurrentScene is MenuContext menuScene)
        {
            menuScene.BuildServices();
            menuScene.BindServices(_sfx, _music, _gameStateHolder);
            menuScene.ConnectSignals();
            menuScene.Setup();

            menuScene.RequestQuit += HandleRequestQuit;
            menuScene.RequestStartGame += HandleRequestStartGame;
        }
    }

    public void HandleRequestStartGame()
    {
        CurrentScene?.QueueFree();
        CurrentScene = GameScenePacked.Instantiate();
        AddChild(CurrentScene);

        if (CurrentScene is GameContext gameScene)
        {
            gameScene.BuildServices();
            gameScene.BindServices(_sfx, _music, _gameStateHolder);
            gameScene.HandleStartNewGame();

            gameScene.OnGameLoss += HandleLoss;
        }
    }

    public void HandleRequestQuit()
    {
        GetTree().Quit();
    }

    public void HandleLoss()
    {
        GoToMenu();
    }
}
