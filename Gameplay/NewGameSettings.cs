namespace SpaceFleet.Gameplay;

public sealed class NewGameSettings
{
    public string PlayerName { get; init; } = "Pilot";
    public decimal StartingCredits { get; init; } = 1000;
    public string StartingShipName { get; init; } = "Test Ship";
    public string StartingShipTypeId { get; init; } = "testShip";
    public string StartingSystemId { get; init; } = "starter";
}
