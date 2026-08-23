using Godot;

public partial class ShapeLibrary : Node
{
    [Export]
    public Godot.Collections.Array<ShapeResource> Shapes { get; set; } = new();

    public ShapeResource GetRandomShape()
    {
        if (Shapes.Count == 0)
        {
            return null;
        }

        return Shapes[(int)(GD.Randi() % (uint)Shapes.Count)];
    }
}
