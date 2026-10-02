using Raylib_cs;

namespace SpaceFleet.Screens;

public sealed class ContextMenu
{
    private static readonly string[] Items = ["context 1", "context 2", "context 3"];
    private const int Width = 180;
    private const int RowHeight = 32;
    private const int Padding = 6;
    private static int Height => Items.Length * RowHeight + Padding * 2;

    private Rectangle bounds;
    public bool IsOpen { get; private set; }

    public void Update(Input input, int screenWidth, int screenHeight)
    {
        if (input.RightPressed)
        {
            bounds = new Rectangle(
                Math.Clamp(input.MousePosition.X, 0, Math.Max(0, screenWidth - Width)),
                Math.Clamp(input.MousePosition.Y, 0, Math.Max(0, screenHeight - Height)),
                Width, Height);
            IsOpen = true;
            return;
        }

        if (!IsOpen) return;

        if (input.BackPressed || (input.LeftPressed && !input.IsHovered(bounds)))
        {
            Close();
            return;
        }

        for (var i = 0; i < Items.Length; i++)
        {
            if (!input.Clicked(RowBounds(i))) continue;
            // Add each option's action here when gameplay commands are ready.
            Close();
            return;
        }
    }

    public void Draw(Input input)
    {
        if (!IsOpen) return;

        Raylib.DrawRectangleRec(bounds, new Color(24, 24, 24, 255));
        Raylib.DrawRectangleLinesEx(bounds, 1, Color.Gray);

        for (var i = 0; i < Items.Length; i++)
        {
            var row = RowBounds(i);
            if (input.IsHovered(row))
                Raylib.DrawRectangleRec(row, new Color(42, 62, 78, 255));
            Raylib.DrawText(Items[i], (int)row.X + 10, (int)row.Y + 7, 18, Color.White);
        }
    }

    public void Close() => IsOpen = false;

    private Rectangle RowBounds(int index) =>
        new(bounds.X + Padding, bounds.Y + Padding + index * RowHeight,
            Width - Padding * 2, RowHeight);
}
