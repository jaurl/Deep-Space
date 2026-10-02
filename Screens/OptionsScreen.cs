namespace SpaceFleet.Screens;

public sealed class OptionsScreen
{
    private const int BackButtonY = 390;

    public bool BackRequested(Input input) =>
        input.Clicked(Ui.ButtonBounds(BackButtonY)) || input.BackPressed;

    public void Draw(Input input)
    {
        Ui.DrawCenteredText("Options", 220, 32);
        Ui.DrawCenteredText("No options added yet", 300, 20);
        Ui.DrawButton("Back", BackButtonY, input);
    }
}
