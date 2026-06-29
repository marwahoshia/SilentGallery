# Changelog

All notable changes to The Silent Gallery are documented here.

## [Unreleased]

### Added
- Initial project setup: Git repository, remote, `.gitignore`, `CLAUDE.md`, and `Assets/Scripts/` folder structure (`ClueSystem/`, `TimerSystem/`, `UISystem/`, `InteractionSystem/`).
feature/ui-main-pause-menu
- UI System (B.6): `MainMenuController`, `PauseMenuController`, and shared `SceneNames` constants. See `Assets/Scripts/UISystem/README.md` for Unity Editor wiring steps.
 feature/interaction-system
- Interaction System (B.2): `IInteractable` interface, `InteractionController` (raycast + key press), `InteractionPromptUI`. See `Assets/Scripts/InteractionSystem/README.md`.

- Player Movement (B.1): custom `FirstPersonController` (CharacterController-based walk, mouse look, jump, gravity). See `Assets/Scripts/PlayerMovement/README.md`.
 main
 main
