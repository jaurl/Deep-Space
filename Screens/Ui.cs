using Raylib_cs;

namespace SpaceFleet.Screens;

internal static class Ui
{
    private const int ButtonWidth = 260;
    private const int ButtonHeight = 50;
    private const int ButtonFontSize = 24;

    public static void DrawButton(string label, int y, Input input, bool enabled = true)
    {
        var bounds = ButtonBounds(y);
        var hovered = enabled && input.IsHovered(bounds);
        Raylib.DrawRectangleRec(bounds, hovered ? Color.DarkGray : new Color(24, 24, 24, 255));
        Raylib.DrawRectangleLinesEx(bounds, 1, enabled ? Color.Gray : Color.DarkGray);
        Raylib.DrawText(label,
            (int)(bounds.X + (bounds.Width - Raylib.MeasureText(label, ButtonFontSize)) / 2),
            y + (ButtonHeight - ButtonFontSize) / 2,
            ButtonFontSize, enabled ? Color.White : Color.Gray);
    }

    public static void DrawCenteredText(string text, int y, int fontSize) =>
        Raylib.DrawText(text, (Raylib.GetScreenWidth() - Raylib.MeasureText(text, fontSize)) / 2,
            y, fontSize, Color.White);

    public static Rectangle ButtonBounds(int y) =>
        new((Raylib.GetScreenWidth() - ButtonWidth) / 2f, y, ButtonWidth, ButtonHeight);
}
