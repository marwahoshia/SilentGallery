# Clue Reveal System (B.3)

Owner: Member 3

## Scripts

- **ClueData.cs** — `ScriptableObject` holding a clue's id, title, and text. Create via **Assets > Create > Silent Gallery > Clue Data**.
- **ClueManager.cs** — singleton (`ClueManager.Instance`); tracks revealed clue ids, raises `OnClueRevealed(ClueData)` and `OnAllRequiredCluesRevealed`. One instance per gameplay scene.
- **ClueObject.cs** — implements `IInteractable` (from the Interaction System, B.2); put on any in-scene object that should reveal a clue. Requires a Collider on the same GameObject.
- **ClueDisplayUI.cs** — shows a popup panel with the clue's title/text when revealed; has a `CloseClueDisplay()` method for a Close button.

## ⚠️ Open question — not yet resolved

**How many clues does the player need to find to win?** This is currently a placeholder:

```csharp
// ClueManager.cs
private const int CLUES_REQUIRED_TO_WIN = 3; // PLACEHOLDER: confirm with team
```

Change this one line once the team decides. The Win/Lose End Screen (B.7) listens for `ClueManager.OnAllRequiredCluesRevealed`, so no other code needs to change.

## Placeholder test data (for development/testing before real content exists)

Create 3 `ClueData` assets (Assets > Create > Silent Gallery > Clue Data) with these values, matching `CLUES_REQUIRED_TO_WIN = 3`:

| Clue Id | Title | Clue Text |
|---|---|---|
| `clue_001` | The Torn Letter | "...if anyone asks, I was never in the east wing that night. — M." |
| `clue_002` | The Smudged Painting | Behind the canvas, a faint outline of a safe combination: 4 - 17 - 9. |
| `clue_003` | The Curator's Ledger | A ledger entry circled in red ink: "Painting #12 — appraised value does not match insurance record." |

Save these as `ClueData_TornLetter.asset`, `ClueData_SmudgedPainting.asset`, `ClueData_CuratorsLedger.asset` (suggested) inside a new `Assets/Resources/Clues/` or `Assets/Data/Clues/` folder — either works since they're assigned by reference in the Inspector, not loaded by path.

## Manual Unity Editor setup

1. Create the 3 placeholder `ClueData` assets above (or your own real clue content).
2. Create an empty GameObject `ClueManager` in the gameplay scene and add the `ClueManager` script. Only one per scene.
3. For each clue-bearing object in the level (painting, drawer, etc.): add a Collider, add the `ClueObject` script, and assign one `ClueData` asset to it.
4. In the gameplay Canvas, build a clue popup: a Panel (e.g. `CluePanel`, inactive by default) with a title TextMeshPro label, a body TextMeshPro label, and an optional "Close" button.
5. Create a UI GameObject (e.g. `ClueDisplayUI`), add the `ClueDisplayUI` script, and assign `CluePanel`, the title label, and the body label.
6. If you added a Close button, wire its `OnClick()` to `ClueDisplayUI.CloseClueDisplay`.

## Dependencies

- Requires `IInteractable` from the Interaction System (B.2) — `ClueObject` implements it.
- The Win/Lose End Screen (B.7) depends on `ClueManager.OnAllRequiredCluesRevealed` to trigger the Win state.
