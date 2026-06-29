# Changelog

All notable changes to The Silent Gallery are documented here.

## [Unreleased]

### Added
- Initial project setup: Git repository, remote, `.gitignore`, `CLAUDE.md`, and `Assets/Scripts/` folder structure (`ClueSystem/`, `TimerSystem/`, `UISystem/`, `InteractionSystem/`).
 feature/win-lose-end-screen
- Win/Lose End Screen (B.7): `GameOutcomeManager`, `EndScreenController`. Depends on Clue Reveal System, Countdown Timer, and UI System branches being merged first. See `Assets/Scripts/WinLoseSystem/README.md`.

feature/clue-reveal-system
- Clue Reveal System (B.3): `ClueData` ScriptableObject, `ClueManager`, `ClueObject`, `ClueDisplayUI`, plus 3 placeholder clue text entries. Win condition clue count is a placeholder constant pending team decision. See `Assets/Scripts/ClueSystem/README.md`.

feature/countdown-timer
- Countdown Timer (B.5): `CountdownTimer` (pause-aware via Time.timeScale), `TimerDisplayUI`. Timer duration is a placeholder pending team decision. See `Assets/Scripts/TimerSystem/README.md`.

feature/ui-main-pause-menu
- UI System (B.6): `MainMenuController`, `PauseMenuController`, and shared `SceneNames` constants. See `Assets/Scripts/UISystem/README.md` for Unity Editor wiring steps.
 feature/interaction-system
- Interaction System (B.2): `IInteractable` interface, `InteractionController` (raycast + key press), `InteractionPromptUI`. See `Assets/Scripts/InteractionSystem/README.md`.

- Player Movement (B.1): custom `FirstPersonController` (CharacterController-based walk, mouse look, jump, gravity). See `Assets/Scripts/PlayerMovement/README.md`.
 main
 main
 main
 main
 main
