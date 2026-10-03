namespace SpaceFleet.Gameplay.Ships;

// Shared by every instance of this ship type. Sprite height is in world metres.
public sealed record ShipDefinition(string Id, string Name, string SpritePath, float SpriteHeight)
{
    public static ShipDefinition TestShip { get; } = new(
        "testShip", "Test Ship", "resources/Sprites/Ships/TerranShip.png", 96);

    public static ShipDefinition FromId(string? id) => id switch
    {
        "testShip" => TestShip,
        _ => throw new ArgumentException($"Unknown ship definition: {id}", nameof(id))
    };
}
