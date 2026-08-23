using Godot;

public partial class ShapeResource : Resource
{
    [Export]
    public Godot.Collections.Array<Vector2I> Offsets { get; set; } = new();

    [Export]
    public int Color { get; set; }
}
