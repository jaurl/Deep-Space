using Raylib_cs;
using SpaceFleet.Gameplay;
using SpaceFleet.Screens;

namespace SpaceFleet;

public sealed class Game
{
    private const int WindowWidth = 1280;
    private const int WindowHeight = 720;
    private const int TargetFramesPerSecond = 60;

    private readonly NewGameSettings newGameSettings = new();
    private readonly MenuScreen menuScreen = new();
    private readonly OptionsScreen optionsScreen = new();
    private readonly GameplayScreen gameplayScreen = new();

    private GameState state = GameState.Menu;
    private GameSession? session;
    private Input input;

    public void Run()
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "Deep Space");
        Raylib.SetTargetFPS(TargetFramesPerSecond);
        Raylib.SetExitKey(KeyboardKey.Null);

        try
        {
            using var audio = new Audio();
            while (!Raylib.WindowShouldClose() && state != GameState.Exit)
            {
                audio.Update();
                input = Input.Read();
                Update();
                Draw();
            }
        }
        finally
        {
            Raylib.CloseWindow();
        }
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.Menu:
                HandleMenuAction(menuScreen.Update(input, canContinue: session is not null));
                break;
            case GameState.Playing:
                if (gameplayScreen.BackRequested(input))
                    state = GameState.Menu;
                break;
            case GameState.Options:
                if (optionsScreen.BackRequested(input))
                    state = GameState.Menu;
                break;
        }
    }

    private void HandleMenuAction(MenuAction action)
    {
        switch (action)
        {
            case MenuAction.NewGame:
                StartNewGame();
                break;
            case MenuAction.Continue:
                ContinueGame();
                break;
            case MenuAction.Options:
                state = GameState.Options;
                break;
            case MenuAction.Exit:
                state = GameState.Exit;
                break;
        }
    }

    private void StartNewGame()
    {
        session = GameSession.CreateNew(newGameSettings);
        gameplayScreen.Reset();
        state = GameState.Playing;
    }

    private void ContinueGame()
    {
        if (session is not null)
            state = GameState.Playing;
    }

    private void Draw()
    {
        if (state == GameState.Exit) return;

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        switch (state)
        {
            case GameState.Menu:
                menuScreen.Draw(input, canContinue: session is not null);
                break;
            case GameState.Playing:
                gameplayScreen.Draw(session!, input);
                break;
            case GameState.Options:
                optionsScreen.Draw(input);
                break;
        }

        Raylib.EndDrawing();
    }
}
