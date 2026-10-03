# Deep Space architecture review and growth plan

Reviewed 3 October 2026. This is a proposed roadmap; only the requested station enlargement is implemented in this update.

## Keep the existing foundation

The current separation is useful: Game owns the window and application states, GameSession owns a new game's live data, EntityManager owns spawned objects, shared definitions describe ship/station types, and screens/renderers own presentation and native textures. Keep one C# project while these boundaries develop.

Keep WorldPosition in double-precision metres. Subtract the camera's world position in doubles before converting nearby positions to raylib floats. The existing AU-scale coordinates and dictionaries are adequate for the demo. Static authored maps should remain separate from live session state and saved games.

## Issues to address before the next features

| Priority | Finding | Change to make |
| --- | --- | --- |
| Before ship switching, docking or destruction | Player.ChangeShip accepts a ship from another session. EntityManager.Despawn can remove the active ship, after which GameplayScreen.Draw throws. | Route switching/removal through GameSession. Validate that the ship exists in this session and belongs to the pilot. Define a docked/no-ship state and handle it in the screen. |
| Before zoom or movement | GameplayScreen adds metre offsets directly to screen pixels; SpriteHeight is also treated as both metres and pixels. | Add a WorldCamera conversion and distinguish sprite dimensions from physical radius and interaction ranges. |
| Before movement | Game.Update handles menus and popup input, but has no world simulation update. | Add a fixed simulation clock and GameSession.Update(stepSeconds). Pause the simulation while in menus as the initial policy. Keep drawing and audio running every frame. |
| Before more map content | SystemDefinition.Load validates ID/radius, but missing positions become zero, missing type defaults to Ship, and the entity list can be null. | Validate required fields, finite coordinates and definition references. Add stable authored spawn IDs, then validate their uniqueness. Include filename/entity details in errors. |
| Before context actions | ContextMenu closes after a click without returning an action or identifying a target. | Let the screen return a command with an entity ID. Consume popup clicks before world input; the session validates and executes commands. |

Entity position/type are currently stored in Entity, while Ship and Station store their ID, name and definition. Preserve one authoritative position in EntityManager. Avoid adding a second independent position to Ship. Consolidate redundant names if renaming is introduced. Apply spawn/despawn changes at a defined point in a simulation tick so systems do not mutate a collection during enumeration.

The station is now 960 metres high, four times its previous 240. It remains 350 metres left of the starting ship. At today's one-pixel-per-metre view it extends beyond the window and its sprite footprint overlaps the player's area. There is no collision model yet; future physical radius and docking range must be chosen explicitly.

## Milestone 1: navigation demo

Build a small playable route between the station, nearby point and distant point.

1. Add WorldCamera with double world centre, zoom, WorldToScreen and ScreenToWorld. Keep the existing top-down X/Z view. Add camera-aware culling and draw large stations behind ships.
2. Introduce a fixed update step, with a bounded number of catch-up ticks after a slow frame. Process input once per rendered frame so a click cannot issue repeated orders across several ticks.
3. Add ship movement state: heading, velocity, destination and current travel mode. Define normal speed in metres/second and warp speed in AU/second; convert to metres internally. Separate travel state from its visual effects.
4. Add selection and a small command type such as Approach(entityId), WarpTo(entityId) and Stop. ContextMenu displays available actions; GameplayScreen submits commands; GameSession validates them; MovementSystem updates position.
5. Start with simple acceleration/braking and explicit arrival distance. Warp should stop at a chosen offset rather than at the target centre. Show remaining distance and travel state in the HUD. A simple target list can expose the distant point while it is outside the local view.

Done when the player can approach the nearby point, warp to the distant point, stop and return, with travel independent of render frame rate. Check near/far precision, arrival without overshoot, and popup clicks not also issuing world orders.

## Milestone 2: one complete gameplay loop

Validate controlled-ship membership and lifecycle first. Then add docking, one asteroid, mining, cargo capacity and unloading at the station. Finish this small loop before adding combat or a larger economy.

MovementSystem, DockingSystem and MiningSystem operate on session data. UI displays that data and issues commands. Shared ship definitions hold type-level values; individual ships hold changing values such as cargo, hull and orders. Station definitions describe a station type; station instances hold changing inventories/services when needed.

Add save/load once this loop exists. Use explicit save records with a format version, system/definition IDs and preserved runtime entity IDs. Give authored spawns stable IDs so map objects can be reconciled with saved changes. Store player saves separately from resources/systems. Validate references on load and write saves through a temporary file before replacing the previous save.

Done when a player can mine, dock, unload, save, restart and continue the same session. New Game must still create fresh state.

## Milestone 3: multiple pilots and more content

Separate the human player's shared fleet/credits from individual Pilot records. Each pilot has an assigned ship; active pilot, selected entity and camera focus are separate IDs. Switch control without recreating ships or losing their orders.

Player commands and AI should use the same order execution: Move, Warp, Mine, Dock and later Escort/Attack. AI chooses orders; it should not implement another movement or mining engine. Begin with simple queued orders rather than a broad behaviour framework.

When more types arrive, replace hard-coded FromId switches with a validated definition catalogue loaded once. Keep maps authored. Add SystemState for each loaded map and a clear transfer operation for ships moving between systems. Decide which inactive systems need simulation before keeping every system running at full frequency.

Done when two pilots can work independently, control can switch between them, and ships can travel between two authored systems while retaining identity and cargo.

## File responsibilities as features arrive

| Location | Responsibility |
| --- | --- |
| Game.cs | Window lifetime, application state, frame loop and simulation clock |
| Input.cs | Snapshot of physical input |
| Screens/ | Menus, HUD, selection and converting input into commands |
| Rendering/ | Camera conversion, sprite/marker drawing and texture lifetime |
| Gameplay/GameSession.cs | Live game state, command validation and coordinating updates |
| Gameplay/Entities/ | Identity, positions, typed lookup and spawn/despawn |
| Gameplay/Ships/, Stations/, Player/ | Type definitions and runtime state; pilots/fleet when introduced |
| Gameplay/Commands/ | Small typed orders shared by UI and AI |
| Gameplay/Systems/ | Movement, docking, mining and later combat rules |
| Gameplay/World/ | Authored map loading and per-system runtime state |
| Persistence/ | Versioned save records, loading and writing |

Add these files when their first feature needs them; the empty AI/Combat/Mining/Exploration folders need no framework yet. Pure gameplay calculations should remain usable without opening a raylib window. Move meaningful simulation checks into a repository test project as gameplay arrives, and add build/test automation so a fresh checkout can be verified.

## Optimise when there is evidence

Start with the current dictionary lookups and simple loops. Measure update/draw time with representative ship counts before introducing spatial indexing, lower-frequency remote simulation or additional threading. A full ECS, dependency-injection container, global event bus and separate assemblies are unnecessary for the current demo.

Recommended next work: WorldCamera, then the fixed update and Approach command. That gives an immediate playable improvement while establishing the same movement path future AI pilots will use.
