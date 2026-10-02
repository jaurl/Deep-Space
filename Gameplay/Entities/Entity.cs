using SpaceFleet.Gameplay.World;

namespace SpaceFleet.Gameplay.Entities;

public readonly record struct Entity
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public EntityType Type { get; }
    public WorldPosition Position { get; init; }

    public Entity(string name, EntityType type, WorldPosition position)
    {
        Name = name;
        Type = type;
        Position = position;
    }
}
