using Godot;

public partial class GameCamera : Camera2D
{
    [Signal]
    public delegate void RequestShakeScreenEventHandler(int row, Godot.Collections.Array<int> values);

    private const float MaxShake = 5.0f;

    public Vector2 HomePosition { get; private set; }
    public Tween CurrentShake { get; private set; }

    public void Initialize()
    {
        HomePosition = new Vector2(GetWindow().Size.X / 2.0f, GetWindow().Size.Y / 2.0f);
        Position = HomePosition;
        RequestShakeScreen += ShakeScreen;
    }

    public void ShakeScreen(int row, Godot.Collections.Array<int> values)
    {
        if (CurrentShake == null || !CurrentShake.IsRunning())
        {
            CurrentShake = GetTree().CreateTween();
            CurrentShake.TweenProperty(this, "position", RandomShakePosition(), 0.05);
            CurrentShake.TweenProperty(this, "position", RandomShakePosition(), 0.05);
            CurrentShake.TweenProperty(this, "position", RandomShakePosition(), 0.05);
            CurrentShake.TweenProperty(this, "position", RandomShakePosition(), 0.05);
            CurrentShake.TweenProperty(this, "position", HomePosition, 0.05);
        }
    }

    private Vector2 RandomShakePosition()
    {
        return HomePosition + new Vector2(
            (float)GD.RandRange(-MaxShake, MaxShake),
            (float)GD.RandRange(-MaxShake, MaxShake));
    }
}
