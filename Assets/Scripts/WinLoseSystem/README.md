# Win/Lose End Screen (B.7)

Owner: Member 3

## Scripts

- **GameOutcomeManager.cs** — listens to `ClueManager.OnAllRequiredCluesRevealed` (win) and `CountdownTimer.OnTimerExpired` (lose), raises a single `OnGameWon` / `OnGameLost` event, guarded so only one can ever fire.
- **EndScreenController.cs** — shows the Win or Lose panel, freezes gameplay (`Time.timeScale = 0`), unlocks the cursor so end-screen buttons are clickable, and handles `RestartLevel()` / `ReturnToMainMenu()`.

## ⚠️ Build dependency — this branch does not compile alone

This system references types from two other feature branches that haven't been merged to `main` yet:

- `SilentGallery.ClueSystem.ClueManager` (branch `feature/clue-reveal-system`)
- `SilentGallery.TimerSystem.CountdownTimer` (branch `feature/countdown-timer`)
- `SilentGallery.UISystem.SceneNames` (branch `feature/ui-main-pause-menu`)

This is expected for a feature-branch workflow — merge `feature/clue-reveal-system`, `feature/countdown-timer`, and `feature/ui-main-pause-menu` into `main` (or into this branch) before opening this scene in Unity, otherwise you'll see compiler errors for missing types.

## Manual Unity Editor setup

1. Create an empty GameObject `GameOutcomeManager` in the gameplay scene and add the `GameOutcomeManager` script. Assign the scene's `ClueManager` and `CountdownTimer` GameObjects to its fields.
2. In the gameplay Canvas, create two Panels: `WinPanel` and `LosePanel` (each with a message, e.g. "You escaped!" / "Time's up!", and "Restart" + "Main Menu" buttons). Set both **inactive** by default.
3. Create a UI GameObject (e.g. `EndScreenController`), add the `EndScreenController` script, and assign `GameOutcomeManager`, `WinPanel`, and `LosePanel`.
4. Wire each panel's "Restart" button to `EndScreenController.RestartLevel`, and each "Main Menu" button to `EndScreenController.ReturnToMainMenu`.

## Dependencies

- Clue Reveal System (B.3) — win trigger.
- Countdown Timer (B.5) — lose trigger.
- UI System (B.6) — reuses `SceneNames.MAIN_MENU_SCENE` for the Main Menu button.
