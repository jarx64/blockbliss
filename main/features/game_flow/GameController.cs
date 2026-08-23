using Godot;

public partial class GameController : Node
{
    [Signal]
    public delegate void OnGameLossEventHandler();

    private ScoreController _scoreController;
    private GridController _gridController;
    private GameStateHolder _gameStateHolder;
    private GridRenderer _renderer;
    private SfxPlayer _sfx;
    private MusicPlayer _music;

    private GameState State => _gameStateHolder.GameState;

    public void BindServices(
        ScoreController scoreController,
        GridController gridController,
        GameStateHolder gameStateHolder,
        GridRenderer renderer,
        SfxPlayer sfx,
        MusicPlayer music)
    {
        _scoreController = scoreController;
        _gridController = gridController;
        _gameStateHolder = gameStateHolder;
        _renderer = renderer;
        _sfx = sfx;
        _music = music;
    }

    public void Start()
    {
        _scoreController.ResetScore();
        State.Status = GameState.GameStatus.Active;
        _gridController.GenerateNewActiveTileShape();
        _gridController.GenerateNewActiveTileShape();
    }

    public void TogglePause()
    {
        if (State.Status == GameState.GameStatus.Paused)
        {
            State.Status = State.LastStatus;
        }
        else
        {
            State.LastStatus = State.Status;
            State.Status = GameState.GameStatus.Paused;
        }
    }

    public void UpdateGameState()
    {
        for (int i = 0; i < GameConstants.Width; i++)
        {
            if (State.Grid[i] != 0)
            {
                State.Status = GameState.GameStatus.Lost;
            }
        }
    }

    public override void _Process(double delta)
    {
        GameState state = State;
        if (state == null)
        {
            return;
        }

        if (Input.IsActionJustPressed("pause"))
        {
            TogglePause();
        }

        if (state.Status == GameState.GameStatus.Active)
        {
            state.ActiveTime += (float)delta;

            if (state.ActiveTime - state.LastDropTime > state.TotalGravity)
            {
                state.LastDropTime = state.ActiveTime;

                if (state.CurrentActiveShape != null)
                {
                    if (_gridController.IsCurrentShapeTouchingGround())
                    {
                        _sfx.RequestSound(SfxPlayer.SoundKey.Impact);
                        _gridController.ConvertActiveTilesToGrid();
                        _gridController.CheckAndClearRows();
                        UpdateGameState();
                        _gridController.GenerateNewActiveTileShape();
                    }
                    else
                    {
                        state.CurrentActiveShape.Offset += new Vector2I(0, 1);
                    }
                }
            }

            if (Input.IsActionPressed("right"))
            {
                _gridController.MoveActiveBlocks(new Vector2I(1, 0));
            }
            else if (Input.IsActionPressed("left"))
            {
                _gridController.MoveActiveBlocks(new Vector2I(-1, 0));
            }

            state.SpeedUp = Input.IsActionPressed("down");

            if (Input.IsActionJustPressed("up"))
            {
                _gridController.RotateActiveBlocks();
            }

            _renderer.Update();
        }
        else if (state.Status == GameState.GameStatus.Lost)
        {
            EmitSignal(SignalName.OnGameLoss);
        }
    }
}
