using SpaceFleet.Gameplay.World;

namespace SpaceFleet.Gameplay.Entities;

public sealed class EntityManager
{
    private readonly Dictionary<Guid, Entity> entities = new();
    public IReadOnlyCollection<Entity> All => entities.Values;

    public Entity Spawn(string name, EntityType type, WorldPosition position)
    {
        var entity = new Entity(name, type, position);
        entities.Add(entity.Id, entity);
        return entity;
    }

    public Entity? Find(Guid id) => entities.TryGetValue(id, out var entity) ? entity : null;

    // Structs are values; store the changed copy back in the collection.
    public bool SetPosition(Guid id, WorldPosition position)
    {
        if (!entities.TryGetValue(id, out var entity)) return false;
        entities[id] = entity with { Position = position };
        return true;
    }

    public bool Despawn(Guid id) => entities.Remove(id);
}
