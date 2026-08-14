![Unity](https://img.shields.io/badge/Unity-2022.3.62f3-black?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Windows-blue)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)

# The Silent Gallery

A first-person mystery set in a museum after hours. The exhibits hold more than art — five clues are hidden among them, and the clock is already running.

---

## Table of Contents

- [Overview](#overview)
- [Gameplay Video](#gameplay-video)
- [Screenshots](#screenshots)
- [How to Play](#how-to-play)
- [Game Flow](#game-flow)
- [Systems &amp; Architecture](#systems--architecture)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Team](#team)
- [Tech Stack &amp; Assets](#tech-stack--assets)
- [Known Limitations &amp; Roadmap](#known-limitations--roadmap)
- [Academic Context](#academic-context)

---

## Overview

**The Silent Gallery** is a short first-person exploration game built in Unity. The player wakes up locked inside a museum gallery after closing time and has to piece together what happened by finding five hidden clues — a torn letter, a smudged painting, a curator's ledger, and more — before time runs out.

| | |
|---|---|
| **Genre** | First-person mystery / clue hunt |
| **Playtime** | ~5 minutes per run |
| **Win condition** | Reveal all 5 clues before the timer expires |
| **Lose condition** | Timer reaches zero first |
| **Scenes** | Main Menu → Gallery (gameplay) |

## Gameplay Video

> 🎬 **TODO:** Replace with the team's recorded YouTube walkthrough (5–10 min, narrated) before submission.
>
> [Watch on YouTube](#)

## Screenshots

> 🖼️ **TODO:** Add screenshots before submission. Suggested shots:

| Shot | Description |
|---|---|
| Main Menu | Title screen with Play / How to Play / Settings / Credits |
| Gallery interior | A room showing the exhibit layout and lighting |
| Clue popup | The reveal panel showing a found clue's title and text |
| Pause Menu | In-game pause overlay |
| Win / Lose screen | End state screen with Restart / Main Menu options |

## How to Play

### Objective

Explore the gallery and find **5 clues** before the **5-minute countdown** reaches zero.

### Controls

| Action | Input |
|---|---|
| Move | `W` `A` `S` `D` |
| Look around | Mouse |
| Jump | `Space` |
| Interact / Inspect | `E` |
| Pause | `Esc` |

The interact prompt ("Press E to inspect...") appears automatically whenever you're looking at something within reach — no guessing required.

### Win / Lose

- **Win:** reveal all 5 clues before the timer runs out.
- **Lose:** the countdown reaches `00:00` first.

Either outcome ends the game on a dedicated end screen with the option to restart or return to the Main Menu.

## Game Flow

```
Main Menu
 ├─ Play          → loads Gallery scene
 ├─ How to Play    → instructions panel
 ├─ Settings       → settings panel
 ├─ Credits        → credits panel
 └─ Quit           → exits the application

Gallery (gameplay)
 ├─ Esc            → Pause Menu (Resume / Restart / Main Menu / Quit)
 └─ Win or Lose     → End Screen (Restart / Main Menu)
```

## Systems &amp; Architecture

The codebase is split into independent systems under `Assets/Scripts/`, each with its own README:

| System | Scripts | Responsibility |
|---|---|---|
| [Player Movement](Assets/Scripts/PlayerMovement/README.md) | `FirstPersonController` | `CharacterController`-based WASD movement, mouse look, jump, gravity |
| [Interaction System](Assets/Scripts/InteractionSystem/README.md) | `IInteractable`, `InteractionController`, `InteractionPromptUI` | Camera raycast detection, focus/interact events, on-screen prompt |
| [Clue System](Assets/Scripts/ClueSystem/README.md) | `ClueData`, `ClueObject`, `ClueManager`, `ClueDisplayUI` | Clue data (ScriptableObject), reveal tracking, win-threshold event, popup UI |
| [Timer System](Assets/Scripts/TimerSystem/README.md) | `CountdownTimer`, `TimerDisplayUI` | Countdown logic (pauses with `Time.timeScale`), mm:ss display |
| [UI System](Assets/Scripts/UISystem/README.md) | `MainMenuController`, `PauseMenuController`, `MenuPanelController`, `SceneNames` | Menu navigation, pause logic, How to Play / Settings / Credits panels |
| [Win/Lose System](Assets/Scripts/WinLoseSystem/README.md) | `GameOutcomeManager`, `EndScreenController` | Listens to Clue + Timer systems, decides the outcome, drives the end screen |

Systems communicate through C# events (`Action`/`Action<T>`) rather than direct references where possible, so e.g. the UI layer never has to poll game state — it just reacts to `OnClueRevealed`, `OnTimerExpired`, `OnGameWon`, etc.

## Getting Started

**Requirements:** Unity **2022.3.62f3** (or a compatible 2022 LTS patch), via Unity Hub.

1. Clone the repository:
   ```bash
   git clone https://github.com/marwahoshia/SilentGallery.git
   ```
2. In Unity Hub → **Add** → select the cloned folder.
3. Open the project. Let Unity import (first import may take a few minutes).
4. Open `Assets/MainMenu.unity` and press **Play** — or go to **File → Build Settings → Build** to produce a standalone build. Both scenes are already registered in Build Settings, with `MainMenu` as the starting scene.

## Project Structure

```
Assets/
├── MainMenu.unity            # Entry-point scene
├── Gallery.unity              # Gameplay scene
├── Scripts/
│   ├── PlayerMovement/
│   ├── InteractionSystem/
│   ├── ClueSystem/
│   ├── TimerSystem/
│   ├── UISystem/
│   └── WinLoseSystem/
└── ...                         # Environment, prop, and material assets
```

## Team

Built by a 4-person team for a Unity course project.

| Member | System(s) Owned |
|---|---|
| Fatima Abu Abed | Player Movement + Interaction System |
| Marwa Hoshia | UI — Main Menu &amp; Pause Menu, Win/Lose End Screen; overall codebase lead |
| Hiba Abdo | Clue Reveal System |
| Haneen Shqerat | Level Design + Countdown Timer |

## Tech Stack &amp; Assets

- **Engine:** Unity 2022.3.62f3, Built-in Render Pipeline
- **Language:** C#
- **Packages:** TextMeshPro 3.0.9, ProBuilder 5.2.4

**Third-party environment/prop assets** (used for educational, non-commercial coursework under their respective licenses):

- 3D Free Modular Kit — Barking_Dog
- Greek Statue
- Oil Paintings — PolyKebap
- Bust of Sappho, Owl Statue — AK Studio Art
- Ancient Pots
- Books pack
- Free Statue Pack
- Concrete Bench — H&amp;L Assets
- Furniture (Bandaji, folding screens, tables) — KCDF

## Known Limitations &amp; Roadmap

- [ ] On-screen clue progress counter (e.g. "3 / 5 clues found") — tracking data already exists in `ClueManager`
- [ ] Background music / sound effects
- [ ] Functional Settings panel (volume, sensitivity, etc.) — currently a placeholder
- [ ] Settings option from the Pause Menu
- [ ] Additional animated objects/characters
- [ ] Cheat manager for testing/debugging

## Academic Context

Developed as a **Unity Course Project — Student Choice** assignment. See [`CHANGELOG.md`](CHANGELOG.md) for a history of notable changes, and [`CLAUDE.md`](CLAUDE.md) for internal development conventions.
