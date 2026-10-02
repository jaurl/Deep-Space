using SpaceFleet.Gameplay.Entities;
using SpaceFleet.Gameplay.World;
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
            entities.Spawn(spawn.Name, spawn.Type, spawn.Position);

        return entities;
    }

    private static PlayerCharacter CreatePlayer(
        NewGameSettings settings, SystemDefinition system, EntityManager entities)
    {
        var ship = entities.Spawn(settings.StartingShipName, EntityType.Ship, system.PlayerSpawn);
        return new PlayerCharacter(settings.PlayerName, settings.StartingCredits, ship.Id);
    }
}
