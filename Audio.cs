using Raylib_cs;

namespace SpaceFleet;

public sealed class Audio : IDisposable
{
    private Music mainTheme;
    private bool loaded;
    private bool disposed;

    public Audio()
    {
        Raylib.InitAudioDevice();
        if (!Raylib.IsAudioDeviceReady())
        {
            Console.Error.WriteLine("Audio device unavailable.");
            return;
        }

        var path = Path.Combine(AppContext.BaseDirectory, "resources", "music", "MainTheme.mp3");
        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"Music file missing: {path}");
            return;
        }

        mainTheme = Raylib.LoadMusicStream(path);
        loaded = Raylib.IsMusicValid(mainTheme);
        if (loaded)
        {
            mainTheme.Looping = true;
            Raylib.PlayMusicStream(mainTheme);
        }
        else Console.Error.WriteLine("Could not load MainTheme.mp3.");
    }

    public void Update()
    {
        if (loaded && !disposed) Raylib.UpdateMusicStream(mainTheme);
    }

    public void Dispose()
    {
        if (disposed) return;
        if (loaded) Raylib.UnloadMusicStream(mainTheme);
        Raylib.CloseAudioDevice();
        disposed = true;
    }
}
