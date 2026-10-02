namespace SpaceFleet.Gameplay;

public sealed class NewGameSettings
{
    public string PlayerName { get; init; } = "Pilot";
    public decimal StartingCredits { get; init; } = 1000;
    public string StartingShipName { get; init; } = "Starter Ship";
    public string StartingSystemId { get; init; } = "starter";
}
