namespace SpaceFleet.Gameplay.Ships;

public sealed class Ship
{
    public Guid Id { get; }
    public string Name { get; }
    public ShipDefinition Definition { get; }

    internal Ship(Guid id, string name, ShipDefinition definition)
    {
        Id = id;
        Name = name;
        Definition = definition;
    }
}
