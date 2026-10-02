namespace SpaceFleet.Gameplay.Player;

public sealed class Player
{
    public string Name { get; }
    public decimal Credits { get; set; }
    public Guid ActiveShipId { get; set; }

    public Player(string name, decimal credits, Guid activeShipId)
    {
        Name = name;
        Credits = credits;
        ActiveShipId = activeShipId;
    }
}
