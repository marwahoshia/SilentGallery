# Countdown Timer (B.5)

Owner: Member 4

## Scripts

- **CountdownTimer.cs** — counts down from a starting duration; raises `OnTimeUpdated(float remainingSeconds)` each frame and `OnTimerExpired` once at zero. Automatically respects pause, since it's driven by `Time.deltaTime` (frozen when `Time.timeScale == 0`).
- **TimerDisplayUI.cs** — formats remaining time as `mm:ss` and writes it to a TextMeshPro label.

## ⚠️ Open question — not yet resolved

**How long should the player have to find the clues?** Placeholder:

```csharp
// CountdownTimer.cs
private float startingDurationSeconds = 180f; // PLACEHOLDER: confirm with team (currently 3 minutes)
```

This is a `[SerializeField]`, so it can also be tuned per-scene directly in the Inspector without touching code.

## Manual Unity Editor setup

1. Create an empty GameObject `CountdownTimer` in the gameplay scene and add the `CountdownTimer` script. Adjust `Starting Duration Seconds` if you don't want the 180s placeholder, and leave `Auto Start On Scene Load` checked so it starts when the level loads.
2. In the gameplay Canvas, create a TextMeshPro - Text (UI) element (e.g. `TimerText`) wherever the on-screen clock should appear.
3. Create a UI GameObject (e.g. `TimerDisplayUI`), add the `TimerDisplayUI` script, and assign the `CountdownTimer` GameObject and the `TimerText` element to its fields.

## Dependencies

- The Win/Lose End Screen (B.7) depends on `CountdownTimer.OnTimerExpired` to trigger the Lose state if the player hasn't found enough clues yet.
- Already compatible with the Pause Menu (B.6) — no extra wiring needed, since `Time.timeScale = 0` naturally freezes this timer.
