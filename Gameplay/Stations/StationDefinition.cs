namespace SpaceFleet.Gameplay.Stations;

// Shared by every instance of this station type. Sprite height is in world metres.
public sealed record StationDefinition(string Id, string Name, string SpritePath, float SpriteHeight)
{
    public static StationDefinition TestStation { get; } = new(
        "testStation", "Test Station", "resources/Sprites/Stations/TerranStation.png", 960);

    public static StationDefinition FromId(string? id) => id switch
    {
        "testStation" => TestStation,
        _ => throw new ArgumentException($"Unknown station definition: {id}", nameof(id))
    };
}
