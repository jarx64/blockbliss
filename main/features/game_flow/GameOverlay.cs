using Godot;

public partial class GameOverlay : Node
{
    [Export]
    public ScoreBox ScoreBox { get; set; }

    [Export]
    public NextPieceRenderer NextPieceRenderer { get; set; }

    public void Initialize(ScoreController scoreController, GridController gridController)
    {
        ScoreBox.Initialize(scoreController);

        gridController.OnNewNextShape += shape =>
            NextPieceRenderer.EmitSignal(NextPieceRenderer.SignalName.OnNewNextShape, shape);

        NextPieceRenderer.Initialize();
    }
}
