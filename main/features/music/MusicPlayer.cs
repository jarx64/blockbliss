using Godot;

public partial class MusicPlayer : AudioStreamPlayer2D
{
    [Export]
    public AudioStream MenuTrack { get; set; }

    [Export]
    public AudioStream LossTrack { get; set; }

    [Export]
    public Godot.Collections.Array<AudioStream> GameTracks { get; set; } = new();

    public void PlayMenuTrack()
    {
        LoopFinishedWith(PlayMenuTrack);
        Stream = MenuTrack;
        Play();
    }

    public void PlayLossTrack()
    {
        LoopFinishedWith(PlayMenuTrack);
        Stream = LossTrack;
        Play();
    }

    public void PlayGameTrack()
    {
        if (GameTracks.Count == 0)
        {
            return;
        }

        LoopFinishedWith(PlayGameTrack);
        Stream = GameTracks[RandomIndex(GameTracks.Count)];
        Play();
    }

    private void LoopFinishedWith(System.Action handler)
    {
        Finished -= PlayMenuTrack;
        Finished -= PlayGameTrack;
        Finished += handler;
    }

    private static int RandomIndex(int count)
    {
        return (int)(GD.Randi() % (uint)count);
    }
}
