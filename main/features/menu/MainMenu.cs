using Godot;

public partial class MainMenu : PanelContainer
{
    [Signal]
    public delegate void RequestStartGameEventHandler();

    [Signal]
    public delegate void RequestQuitEventHandler();

    [Signal]
    public delegate void RequestScoresEventHandler();

    [Export]
    public Button StartButton { get; set; }

    [Export]
    public Button ScoresButton { get; set; }

    [Export]
    public Button QuitButton { get; set; }

    private MusicPlayer _music;
    private SfxPlayer _sfx;

    public void Initialize(MusicPlayer music, SfxPlayer sfx)
    {
        BindServices(music, sfx);
        BindEvents();
    }

    public void BindServices(MusicPlayer music, SfxPlayer sfx)
    {
        _music = music;
        _sfx = sfx;
    }

    public void BindEvents()
    {
        if (StartButton != null)
        {
            StartButton.Pressed -= OnPressStart;
            StartButton.Pressed += OnPressStart;
        }

        if (ScoresButton != null)
        {
            ScoresButton.Pressed -= OnPressScores;
            ScoresButton.Pressed += OnPressScores;
        }

        if (QuitButton != null)
        {
            QuitButton.Pressed -= OnPressQuit;
            QuitButton.Pressed += OnPressQuit;
        }
    }

    public void OnPressStart()
    {
        _sfx.RequestSound(SfxPlayer.SoundKey.Select);
        EmitSignal(SignalName.RequestStartGame);
    }

    public void OnPressScores()
    {
        _sfx.RequestSound(SfxPlayer.SoundKey.Select);
        EmitSignal(SignalName.RequestScores);
        Hide();
    }

    public void OnPressQuit()
    {
        _sfx.RequestSound(SfxPlayer.SoundKey.Select);
        EmitSignal(SignalName.RequestQuit);
    }

    public void HideMenu()
    {
        Visible = false;
    }

    public void ShowMenu()
    {
        Visible = true;
    }
}
