# UI System — Main Menu & Pause Menu (B.6)

Owner: Member 2

## Scripts

- **SceneNames.cs** — shared scene-name string constants (`MAIN_MENU_SCENE`, `GAMEPLAY_SCENE`). **PLACEHOLDER values — confirm actual scene names with the team and update this one file.**
- **MainMenuController.cs** — `PlayGame()`, `QuitGame()`. Attach to the Main Menu scene.
- **PauseMenuController.cs** — `PauseGame()`, `ResumeGame()`, `RestartLevel()`, `GoToMainMenu()`, `QuitGame()`, plus automatic Escape-key toggle. Attach to the gameplay scene.

## Manual Unity Editor setup

### Main Menu scene
1. Create scene `MainMenu` (must match `SceneNames.MAIN_MENU_SCENE`) and add it to **File > Build Settings > Scenes In Build** (index 0, so it's the scene that launches first).
2. Create a Canvas with a "Play" Button and a "Quit" Button (UI > Button - TextMeshPro).
3. Create an empty GameObject (e.g. `MainMenuController`) and add the `MainMenuController` script to it.
4. On the "Play" button's `OnClick()` event, drag in the `MainMenuController` GameObject and select `MainMenuController.PlayGame`.
5. On the "Quit" button's `OnClick()` event, select `MainMenuController.QuitGame`.

### Gameplay scene (Pause Menu)
1. Create scene `Gallery` (must match `SceneNames.GAMEPLAY_SCENE`) and add it to Build Settings (after MainMenu).
2. Create a Canvas with a Panel (e.g. `PausePanel`) containing "Resume", "Restart", "Main Menu", and "Quit" Buttons (UI > Button - TextMeshPro). Set the Panel **inactive** by default in the Hierarchy (uncheck its checkbox) so the game doesn't start paused.
3. Create an empty GameObject (e.g. `PauseMenuController`) and add the `PauseMenuController` script to it.
4. In the Inspector, drag the `PausePanel` GameObject into the `Pause Panel` field. Leave `Pause Key` as `Escape` unless you want a different key.
5. Wire each button's `OnClick()` event to the matching method on `PauseMenuController`: Resume → `ResumeGame`, Restart → `RestartLevel`, Main Menu → `GoToMainMenu`, Quit → `QuitGame`.
6. Confirm there is an `EventSystem` in the scene (Unity creates one automatically the first time you add a Canvas — if missing: right-click Hierarchy > UI > Event System).

## Dependencies

- Requires `MAIN_MENU_SCENE` and `GAMEPLAY_SCENE` in `SceneNames.cs` to exactly match real scene file names once they're created.
- `RestartLevel()` reloads the *active* scene by name, so it works regardless of what the gameplay scene is ultimately called, as long as it's the active scene.
- Any system that needs to pause/resume gameplay (e.g. Countdown Timer should stop counting while paused) should check `PauseMenuController.IsPaused` or be driven by the same `Time.timeScale` value, since `Time.deltaTime`-based logic naturally halts when `Time.timeScale == 0`.

## Open questions

- None blocking. Scene names are placeholders only.
