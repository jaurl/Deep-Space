# Deep Space

C# / .NET 10 with Raylib-cs 8.1.0 (raylib 6.0).

Double-click Run.cmd, or run `dotnet run --no-restore` in this folder.

The menu has New Game, Continue, Options and Exit. New Game always creates a fresh session. Continue resumes the current session and is disabled until one exists. Saving and loading are not implemented yet.

Program.cs starts Game.cs, which owns the window, state transitions, update and drawing loops. Screens/ contains the menu, options and gameplay views, with shared UI helpers. Input is processed before drawing. Escape returns from gameplay or options to the menu. Options is currently a placeholder.

New Game follows Game.StartNewGame() -> GameSession.CreateNew(): load the system, spawn its entities, create the player and ship, then enter gameplay. Starting values are in Gameplay/NewGameSettings.cs. Continue reuses the current session.

Add assets under resources/, including music under resources/music/. Assets are copied to build and publish output automatically.

Input.cs reads mouse and keyboard controls once per frame. Screens use this shared input for clicks and hover states. Right-click in gameplay opens a context menu with three placeholder options. Clicking an option or outside the box closes it. Escape closes the popup first, then returns to the main menu. The popup stays inside the window when opened near an edge.

## Gameplay structure

- Gameplay/Player: player name, credits and active ship ID.
- Gameplay/Entities: Entity value struct, EntityType enum, and session-owned collection with spawn, find, set-position and despawn operations. Update structs through EntityManager.SetPosition so the changed value is stored.
- Gameplay/World: loads authored system definitions from resources/systems/.
- Gameplay/AI, Combat, Mining, Exploration: reserved folders for future features.

NewGameSettings holds the initial player name, credits, ship name and system ID. These are provisional starting values. New Game loads starter.json, spawns its authored entities and the player's ship, and replaces the old session. Continue keeps the existing session. The gameplay screen draws the player ship in black space with system and player details. Movement has not been added yet.

System maps are fixed JSON definitions, not procedurally generated. Edit playerSpawn and entities in resources/systems/starter.json to author the layout. Each entity entry contains name, type (ship or interestPoint) and position (x, y, z). Runtime entity positions are separate from the original spawn positions.

## Distance reference

WorldPosition stores double-precision metre coordinates. One AU is 149,597,870,700 metres. The current local view remains one pixel per metre and subtracts the player's position before rendering. Objects outside the local viewport are omitted from drawing, but remain in the entity collection.

starter.json contains two passive interest points: one 300 metres from the player, and one 39.8070500839 AU away. The player spawn uses Jita IV - Moon 4 - Caldari Navy Assembly Plant's coordinates; the distant point uses Jita's Perimeter stargate coordinates. The points have no interactions or behaviour.

Reference coordinates were retrieved from EVE's public ESI API on 3 October 2026:

- [Jita system](https://esi.evetech.net/latest/universe/systems/30000142/?datasource=tranquility)
- [Jita 4-4 station](https://esi.evetech.net/latest/universe/stations/60003760/?datasource=tranquility)
- [Perimeter gate in Jita](https://esi.evetech.net/latest/universe/stargates/50001249/?datasource=tranquility)
- [Coordinate conventions](https://developers.eveonline.com/docs/guides/map-data/)

The demo uses a planning radius of 42.1 AU around centre (0, 0, 0), for a diameter of 84.2 AU. This rounds up Jita's currently mapped outer gates at approximately 42.0512 AU from its star. It is our chosen spherical extent, not a claim about EVE having a hard spherical boundary. Radius is map metadata for future planning; no boundary or movement behaviour is implemented.

Build: `dotnet build --no-restore`.

The raylib NuGet package is bundled in packages/ for offline restore. If you move this project or delete obj/, run `dotnet restore --source packages -p:NuGetAudit=false` once before launching.
