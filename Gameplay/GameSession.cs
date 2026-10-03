using SpaceFleet.Gameplay.Entities;
using SpaceFleet.Gameplay.World;
using SpaceFleet.Gameplay.Ships;
using SpaceFleet.Gameplay.Stations;
using PlayerCharacter = SpaceFleet.Gameplay.Player.Player;

namespace SpaceFleet.Gameplay;

public sealed class GameSession
{
    public Guid Id { get; } = Guid.NewGuid();
    public PlayerCharacter Player { get; }
    public SystemDefinition System { get; }
    public EntityManager Entities { get; }

    private GameSession(PlayerCharacter player, SystemDefinition system, EntityManager entities)
    {
        Player = player;
        System = system;
        Entities = entities;
    }

    public static GameSession CreateNew(NewGameSettings settings)
    {
        var system = SystemDefinition.Load(settings.StartingSystemId);
        var entities = SpawnSystemEntities(system);
        var player = CreatePlayer(settings, system, entities);

        return new GameSession(player, system, entities);
    }

    private static EntityManager SpawnSystemEntities(SystemDefinition system)
    {
        var entities = new EntityManager();
        foreach (var spawn in system.Entities)
        {
            switch (spawn.Type)
            {
                case EntityType.InterestPoint:
                    entities.SpawnInterestPoint(spawn.Name, spawn.Position);
                    break;
                case EntityType.Ship:
                    entities.SpawnShip(ShipDefinition.FromId(spawn.DefinitionId), spawn.Position, spawn.Name);
                    break;
                case EntityType.Station:
                    entities.SpawnStation(StationDefinition.FromId(spawn.DefinitionId), spawn.Position, spawn.Name);
                    break;
                default:
                    throw new InvalidDataException($"Unsupported entity type: {spawn.Type}");
            }
        }

        return entities;
    }

    private static PlayerCharacter CreatePlayer(
        NewGameSettings settings, SystemDefinition system, EntityManager entities)
    {
        var definition = ShipDefinition.FromId(settings.StartingShipTypeId);
        var ship = entities.SpawnShip(definition, system.PlayerSpawn, settings.StartingShipName);
        return new PlayerCharacter(settings.PlayerName, settings.StartingCredits, ship);
    }
}
