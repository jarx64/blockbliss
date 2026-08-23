using Godot;

public partial class Backdrop : Sprite2D
{
    [Export]
    public Godot.Collections.Array<Color> Colors { get; set; } = new();

    [Export]
    public float TransitionTime { get; set; }

    [Export]
    public float TransitionTimeFudge { get; set; }

    public Tween CurrentTween { get; private set; }

    public override void _Ready()
    {
        TweenRandom();
    }

    public void TweenRandom()
    {
        if (Colors.Count == 0)
        {
            return;
        }

        CurrentTween = GetTree().CreateTween();
        CurrentTween.TweenProperty(
            this,
            "modulate",
            Colors[RandomIndex(Colors.Count)],
            GD.RandRange(TransitionTime - TransitionTimeFudge, TransitionTime + TransitionTimeFudge));
        CurrentTween.TweenCallback(Callable.From(TweenRandom));
    }

    private static int RandomIndex(int count)
    {
        return (int)(GD.Randi() % (uint)count);
    }
}
