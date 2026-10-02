using System.Numerics;
using Raylib_cs;

namespace SpaceFleet;

// Read once per frame, then share the same input with controls and drawing.
public readonly record struct Input(
    Vector2 MousePosition,
    bool LeftPressed,
    bool LeftReleased,
    bool RightPressed,
    bool BackPressed)
{
    public static Input Read() => new(
        Raylib.GetMousePosition(),
        Raylib.IsMouseButtonPressed(MouseButton.Left),
        Raylib.IsMouseButtonReleased(MouseButton.Left),
        Raylib.IsMouseButtonPressed(MouseButton.Right),
        Raylib.IsKeyPressed(KeyboardKey.Escape));

    public bool IsHovered(Rectangle bounds) =>
        MousePosition.X >= bounds.X && MousePosition.X < bounds.X + bounds.Width &&
        MousePosition.Y >= bounds.Y && MousePosition.Y < bounds.Y + bounds.Height;

    public bool Clicked(Rectangle bounds, bool enabled = true) =>
        enabled && LeftReleased && IsHovered(bounds);
}
