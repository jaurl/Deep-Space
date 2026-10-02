using System.Numerics;
using Raylib_cs;
using SpaceFleet.Gameplay;
using SpaceFleet.Gameplay.Entities;

namespace SpaceFleet.Screens;

public sealed class GameplayScreen
{
    private readonly ContextMenu contextMenu = new();

    public void Reset() => contextMenu.Close();

    public bool BackRequested(Input input)
    {
        // Escape dismisses the popup first; the next Escape returns to the menu.
        if (input.BackPressed && !contextMenu.IsOpen) return true;
        contextMenu.Update(input, Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        return false;
    }

    public void Draw(GameSession session, Input input)
    {
        var ship = session.Entities.Find(session.Player.ActiveShipId)
            ?? throw new InvalidOperationException("The player's active ship is missing.");
        var centre = new Vector2(Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f);

        // Start with a top-down view centred on the player's ship.
        foreach (var entity in session.Entities.All)
        {
            var offset = entity.Position - ship.Position;
            // Cull distant objects in doubles before passing local values to raylib.
            // The current view uses one pixel per metre; system positions stay unchanged.
            var screenX = centre.X + offset.X;
            var screenY = centre.Y + offset.Z;
            if (screenX < -32 || screenX > Raylib.GetScreenWidth() + 32 ||
                screenY < -32 || screenY > Raylib.GetScreenHeight() + 32)
                continue;

            var position = new Vector2((float)screenX, (float)screenY);
            if (entity.Type == EntityType.Ship)
                Raylib.DrawTriangle(position + new Vector2(0, -18),
                    position + new Vector2(-12, 12), position + new Vector2(12, 12),
                    entity.Id == ship.Id ? Color.SkyBlue : Color.Gray);
            else if (entity.Type == EntityType.InterestPoint)
            {
                Raylib.DrawCircleLines((int)position.X, (int)position.Y, 8, Color.Gray);
                Raylib.DrawText(entity.Name, (int)position.X + 14, (int)position.Y - 8, 16, Color.Gray);
            }
        }

        Raylib.DrawText(session.System.Name, 20, 20, 24, Color.White);
        Raylib.DrawText($"{session.Player.Name} | {ship.Name} | Credits: {session.Player.Credits}",
            20, 52, 18, Color.LightGray);
        Raylib.DrawText("Right-click: context menu | Esc: back", 20,
            Raylib.GetScreenHeight() - 32, 18, Color.Gray);
        contextMenu.Draw(input);
    }
}
