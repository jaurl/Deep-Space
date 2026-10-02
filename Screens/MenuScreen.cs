namespace SpaceFleet.Screens;

public enum MenuAction
{
    None,
    NewGame,
    Continue,
    Options,
    Exit
}

public sealed class MenuScreen
{
    private static readonly (string Label, int Y, MenuAction Action)[] Buttons =
    [
        ("New Game", 260, MenuAction.NewGame),
        ("Continue", 325, MenuAction.Continue),
        ("Options", 390, MenuAction.Options),
        ("Exit", 455, MenuAction.Exit)
    ];

    public MenuAction Update(Input input, bool canContinue)
    {
        foreach (var button in Buttons)
        {
            var enabled = button.Action != MenuAction.Continue || canContinue;
            if (input.Clicked(Ui.ButtonBounds(button.Y), enabled))
                return button.Action;
        }

        return MenuAction.None;
    }

    public void Draw(Input input, bool canContinue)
    {
        Ui.DrawCenteredText("DEEP SPACE", 150, 40);
        foreach (var button in Buttons)
        {
            var enabled = button.Action != MenuAction.Continue || canContinue;
            Ui.DrawButton(button.Label, button.Y, input, enabled);
        }
    }
}
