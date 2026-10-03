using System.Numerics;
using Raylib_cs;
using SpaceFleet.Gameplay;
using SpaceFleet.Gameplay.Entities;
using SpaceFleet.Rendering;

namespace SpaceFleet.Screens;

public sealed class GameplayScreen : IDisposable
{
    private readonly ContextMenu contextMenu = new();
    private readonly SpriteRenderer sprites = new();

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
            var shipDefinition = session.Entities.FindShip(entity.Id)?.Definition;
            var stationDefinition = session.Entities.FindStation(entity.Id)?.Definition;
            var spritePath = shipDefinition?.SpritePath ?? stationDefinition?.SpritePath;
            var spriteHeight = shipDefinition?.SpriteHeight ?? stationDefinition?.SpriteHeight ?? 0;
            var offset = entity.Position - ship.Position;
            // Cull distant objects in doubles before passing local values to raylib.
            // The current view uses one pixel per metre; system positions stay unchanged.
            var screenX = centre.X + offset.X;
            var screenY = centre.Y + offset.Z;
            var margin = Math.Max(32, spriteHeight);
            if (screenX < -margin || screenX > Raylib.GetScreenWidth() + margin ||
                screenY < -margin || screenY > Raylib.GetScreenHeight() + margin)
                continue;

            var position = new Vector2((float)screenX, (float)screenY);
            if (spritePath is not null)
                sprites.Draw(spritePath, spriteHeight, position);
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

    public void Dispose() => sprites.Dispose();
}
