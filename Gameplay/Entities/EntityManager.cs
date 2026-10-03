using SpaceFleet.Gameplay.World;
using SpaceFleet.Gameplay.Ships;
using SpaceFleet.Gameplay.Stations;

namespace SpaceFleet.Gameplay.Entities;

public sealed class EntityManager
{
    private readonly Dictionary<Guid, Entity> entities = new();
    private readonly Dictionary<Guid, Ship> ships = new();
    private readonly Dictionary<Guid, Station> stations = new();
    public IReadOnlyCollection<Entity> All => entities.Values;

    public Entity SpawnInterestPoint(string name, WorldPosition position) =>
        AddEntity(name, EntityType.InterestPoint, position);

    public Ship SpawnShip(ShipDefinition definition, WorldPosition position, string? name = null)
    {
        var entity = AddEntity(name ?? definition.Name, EntityType.Ship, position);
        var ship = new Ship(entity.Id, entity.Name, definition);
        ships.Add(ship.Id, ship);
        return ship;
    }

    public Station SpawnStation(StationDefinition definition, WorldPosition position, string? name = null)
    {
        var entity = AddEntity(name ?? definition.Name, EntityType.Station, position);
        var station = new Station(entity.Id, entity.Name, definition);
        stations.Add(station.Id, station);
        return station;
    }

    private Entity AddEntity(string name, EntityType type, WorldPosition position)
    {
        var entity = new Entity(name, type, position);
        entities.Add(entity.Id, entity);
        return entity;
    }

    public Entity? Find(Guid id) => entities.TryGetValue(id, out var entity) ? entity : null;
    public Ship? FindShip(Guid id) => ships.GetValueOrDefault(id);
    public Station? FindStation(Guid id) => stations.GetValueOrDefault(id);

    // Structs are values; store the changed copy back in the collection.
    public bool SetPosition(Guid id, WorldPosition position)
    {
        if (!entities.TryGetValue(id, out var entity)) return false;
        entities[id] = entity with { Position = position };
        return true;
    }

    public bool Despawn(Guid id)
    {
        ships.Remove(id);
        stations.Remove(id);
        return entities.Remove(id);
    }
}
