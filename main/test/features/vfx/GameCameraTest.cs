using System;
using Godot;

public class GameCameraTest
{
    private const string SourceClassPath = "res://features/vfx/GameCamera.cs";

    public void TestInitialize()
    {
        GameCamera camera = new();
        camera.Initialize();

        if (camera.Position != camera.HomePosition)
        {
            throw new InvalidOperationException("Expected camera position to match home position after initialization.");
        }

        camera.QueueFree();
    }
}
