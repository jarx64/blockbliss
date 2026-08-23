using Godot;

public partial class SfxPlayer : AudioStreamPlayer2D
{
    public enum SoundKey
    {
        Select,
        Impact,
        Clear,
    }

    [Export]
    public Godot.Collections.Array<AudioStream> Sounds { get; set; } = new();

    public void RequestSound(SoundKey key)
    {
        int index = (int)key;
        if (index < 0 || index >= Sounds.Count)
        {
            return;
        }

        Stream = Sounds[index];
        PitchScale = (float)GD.RandRange(0.8, 1.2);
        Play();
    }
}
