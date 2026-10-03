using System.Text.Json;
using System.Text.Json.Serialization;
using SpaceFleet.Gameplay.Entities;

namespace SpaceFleet.Gameplay.World;

// Authored map data; runtime positions and entity IDs belong to the session.
public sealed class SystemDefinition
{
    public string Id { get; init; } = "";
    public string Name { get; init; } = "";
    public WorldPosition Centre { get; init; }
    public double RadiusMetres { get; init; }
    public double DiameterAu => 2 * RadiusMetres / WorldPosition.MetresPerAu;
    public WorldPosition PlayerSpawn { get; init; }
    public List<EntitySpawn> Entities { get; init; } = new();

    public static SystemDefinition Load(string systemId)
    {
        if (string.IsNullOrWhiteSpace(systemId) ||
            systemId.Any(c => !char.IsAsciiLetterOrDigit(c) && c != '-' && c != '_'))
            throw new ArgumentException("System ID must contain only letters, numbers, '-' or '_'.", nameof(systemId));

        var path = Path.Combine(AppContext.BaseDirectory, "resources", "systems", systemId + ".json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter<EntityType>(JsonNamingPolicy.CamelCase, allowIntegerValues: false) }
        };
        var definition = JsonSerializer.Deserialize<SystemDefinition>(File.ReadAllText(path), options)
            ?? throw new InvalidDataException($"Invalid system definition: {path}");
        if (definition.Id != systemId)
            throw new InvalidDataException($"System ID in {path} does not match {systemId}.");
        if (!double.IsFinite(definition.RadiusMetres) || definition.RadiusMetres <= 0)
            throw new InvalidDataException($"System radius in {path} must be positive and finite.");
        return definition;
    }
}

public sealed class EntitySpawn
{
    public string Name { get; init; } = "";
    public EntityType Type { get; init; }
    public string? DefinitionId { get; init; }
    public WorldPosition Position { get; init; }
}
