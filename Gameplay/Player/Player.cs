using SpaceFleet.Gameplay.Ships;

namespace SpaceFleet.Gameplay.Player;

public sealed class Player
{
    public string Name { get; }
    public decimal Credits { get; set; }
    public Guid ActiveShipId { get; private set; }

    public Player(string name, decimal credits, Ship startingShip)
    {
        Name = name;
        Credits = credits;
        ChangeShip(startingShip);
    }

    public void ChangeShip(Ship ship)
    {
        ArgumentNullException.ThrowIfNull(ship);
        ActiveShipId = ship.Id;
    }
}
