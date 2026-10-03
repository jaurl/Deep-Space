namespace SpaceFleet.Gameplay.Stations;

public sealed class Station
{
    public Guid Id { get; }
    public string Name { get; }
    public StationDefinition Definition { get; }

    internal Station(Guid id, string name, StationDefinition definition)
    {
        Id = id;
        Name = name;
        Definition = definition;
    }
}
