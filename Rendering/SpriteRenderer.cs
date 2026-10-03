using System.Numerics;
using Raylib_cs;

namespace SpaceFleet.Rendering;

public sealed class SpriteRenderer : IDisposable
{
    private readonly Dictionary<string, Texture2D> textures = new();

    public void Draw(string spritePath, float height, Vector2 centre)
    {
        var texture = GetTexture(spritePath);
        var width = height * texture.Width / texture.Height;
        var source = new Rectangle(0, 0, texture.Width, texture.Height);
        var destination = new Rectangle(centre.X, centre.Y, width, height);
        Raylib.DrawTexturePro(texture, source, destination,
            new Vector2(width / 2, height / 2), 0, Color.White);
    }

    private Texture2D GetTexture(string spritePath)
    {
        if (textures.TryGetValue(spritePath, out var texture)) return texture;

        var fullPath = Path.Combine(AppContext.BaseDirectory, spritePath);
        if (!File.Exists(fullPath)) throw new FileNotFoundException("Sprite file missing.", fullPath);
        texture = Raylib.LoadTexture(fullPath);
        if (!Raylib.IsTextureValid(texture))
            throw new InvalidDataException($"Could not load sprite: {fullPath}");

        Raylib.SetTextureFilter(texture, TextureFilter.Bilinear);
        textures.Add(spritePath, texture);
        return texture;
    }

    public void Dispose()
    {
        foreach (var texture in textures.Values) Raylib.UnloadTexture(texture);
        textures.Clear();
    }
}
