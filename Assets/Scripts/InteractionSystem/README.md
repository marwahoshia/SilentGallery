# Interaction System (B.2)

Owner: Member 1

## Scripts

- **IInteractable.cs** — interface implemented by anything the player can interact with (`Interact(GameObject)`, `GetInteractionPrompt()`). The Clue Reveal System's `ClueObject` will implement this.
- **InteractionController.cs** — raycasts forward from a camera each frame, detects `IInteractable` objects in range, raises `OnInteractableFocused(string prompt)` / `OnInteractableLost` events, and calls `Interact()` on key press (default `E`). Attach to the Player.
- **InteractionPromptUI.cs** — listens to `InteractionController`'s events and shows/hides a TextMeshPro prompt label (e.g. "Press E to inspect painting"). Attach to a UI GameObject.

## Manual Unity Editor setup

1. On the `Player` GameObject (from Player Movement), add the `InteractionController` script.
2. Drag the Player's child `Camera` (the one used for mouse look) into the **Interaction Camera** field.
3. Adjust **Interaction Range** (default 3m) and **Interact Key** (default `E`) if desired.
4. In the gameplay scene's Canvas, create a TextMeshPro - Text (UI) element for the prompt (e.g. `InteractionPromptText`), positioned near the bottom-center of the screen. Set it **inactive** by default.
5. Create an empty UI GameObject (e.g. `InteractionPromptUI`), add the `InteractionPromptUI` script, and assign the Player's `InteractionController` and the `InteractionPromptText` element to its fields.
6. Make sure any objects meant to be interactable have a **Collider** (e.g. Box Collider) — `Physics.Raycast` only detects colliders, and the script that implements `IInteractable` (e.g. `ClueObject` from the Clue Reveal System) must be on the same GameObject as that collider.

## Dependencies

- Requires a Camera reference from the Player Movement system (B.1).
- The Clue Reveal System (B.3) depends on `IInteractable` — its `ClueObject` script implements this interface so clues can be picked up via this raycast system.

## Open questions

- None blocking.
