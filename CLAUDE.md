# CLAUDE.md

This file gives Claude Code (and any future contributor) the persistent context needed to work on **The Silent Gallery**.

## Project Overview

**The Silent Gallery** is a Unity 3D game built by a 4-person student team for an MVP milestone.

- Engine: Unity 2022 LTS
- Language: C#
- Key packages: ProBuilder, TextMeshPro
- Platform: Windows Desktop
- Repo: https://github.com/marwahoshia/SilentGallery.git

## Team & System Ownership

| Member | Systems Owned |
|---|---|
| Member 1 | Player Movement (B.1) + Interaction System (B.2) |
| Member 2 | UI — Main Menu & Pause Menu (B.6) |
| Member 3 | Clue Reveal System (B.3) + Win/Lose End Screen (B.7) |
| Member 4 | Level Design (B.4) + Countdown Timer (B.5) |

Each member owns their system's design and gameplay decisions. Claude (acting as lead developer) writes the C# implementation for every system, but should flag open design questions back to the project manager rather than guessing silently.

## Claude's Responsibilities

1. Write all C# scripts for every system with full XML doc comments.
2. Manage Git: create a feature branch per system (`feature/system-name`), write clean commit messages, never commit directly to `main`.
3. Generate placeholder test data (clue texts, object IDs, timer configs) so systems are testable before real content exists.
4. Maintain documentation: this CLAUDE.md, a per-system README under each script folder, and CHANGELOG.md.
5. After every script, explain exactly what manual steps are needed in the Unity Editor to wire it up (creating GameObjects, assigning references in the Inspector, setting up prefabs, etc.) — Claude cannot touch the Unity Editor directly.

## Project Structure

```
Assets/
  Scripts/
    ClueSystem/         -- Member 3: Clue Reveal System (B.3)
    TimerSystem/         -- Member 4: Countdown Timer (B.5)
    UISystem/            -- Member 2: Main Menu & Pause Menu (B.6)
    InteractionSystem/   -- Member 1: Interaction System (B.2)
```

Player Movement (B.1) and Win/Lose End Screen (B.7) scripts will get their own folders when those systems are started.

## Git Workflow Rules

- **Never commit directly to `main`.** All work happens on a feature branch named `feature/<system-name>` (e.g. `feature/clue-system`, `feature/countdown-timer`).
- Branches are created *before* any files for that system are touched.
- Commit messages follow conventional style: `feat:`, `fix:`, `chore:`, `docs:`.
- After a system is complete, summarize for the PM: what was built, what manual Unity Editor setup is required, and what other systems depend on it.

## Placeholder / Open Question Convention

Where a design decision is unresolved (e.g. "how many clues are needed to win"), Claude will:
- Flag it explicitly to the PM instead of guessing.
- Implement a `const` placeholder value clearly named and commented so it's a one-line change later (e.g. `private const int CLUES_REQUIRED_TO_WIN = 3; // PLACEHOLDER: confirm win condition with team`).

## Open Questions Log

- Win condition: exact number of clues required to win is not yet finalized. Placeholder: `ClueManager.CLUES_REQUIRED_TO_WIN = 3` in `Assets/Scripts/ClueSystem/ClueManager.cs`.
- Timer duration: exact countdown length is not yet finalized. Placeholder: `CountdownTimer.startingDurationSeconds = 180` (3 min) in `Assets/Scripts/TimerSystem/CountdownTimer.cs`.

## Documentation

- `CHANGELOG.md` at repo root tracks notable changes per system/feature.
- Each `Assets/Scripts/<System>/` folder gets its own `README.md` once that system is implemented, describing the scripts, public API, and Inspector wiring.
