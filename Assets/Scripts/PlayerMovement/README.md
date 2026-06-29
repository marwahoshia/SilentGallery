# Player Movement (B.1)

Owner: Member 1

## Scripts

- **FirstPersonController.cs** — WASD/arrow movement, mouse look, jump, gravity. Custom `CharacterController`-based controller (no Asset Store package).

## Manual Unity Editor setup

1. Create an empty GameObject named `Player`. Add a **Character Controller** component to it (Component > Physics > Character Controller). Default radius/height (0.5 / 2) work fine to start.
2. Add the `FirstPersonController` script to `Player`.
3. Create a `Camera` as a **child** of `Player` (right-click `Player` > Camera), positioned at roughly head height (e.g. local position `(0, 1.6, 0)`). If this is the only camera in the scene, you can keep its default `MainCamera` tag.
4. On the `Player`'s `FirstPersonController` component, drag the child `Camera`'s Transform into the **Camera Transform** field.
5. Tune `Move Speed`, `Jump Speed`, `Gravity`, `Mouse Sensitivity`, `Max Look Angle` in the Inspector as needed — defaults are reasonable starting points, not final game-feel values.
6. Make sure Edit > Project Settings > Input Manager has the default `Horizontal`, `Vertical`, `Mouse X`, `Mouse Y`, and `Jump` axes (these exist by default in a new Unity project — only relevant if Input axes were ever deleted/customized).

## Dependencies

- The Interaction System (B.2) needs a reference to this player's Camera Transform to raycast for interactable objects — use the same child Camera created in step 3.

## Known integration follow-up (flag for later merge)

`FirstPersonController` locks and hides the cursor in `Start()` so mouse-look works immediately. Once this branch is merged together with the UI System (`PauseMenuController`), the cursor needs to be **unlocked and made visible** while the Pause Menu is open, otherwise the player can't click its buttons. Add to `PauseMenuController`:

- In `PauseGame()`: `Cursor.lockState = CursorLockMode.None; Cursor.visible = true;`
- In `ResumeGame()`: `Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;`

This wasn't added directly to `PauseMenuController` here because that file lives on a separate, not-yet-merged branch (`feature/ui-main-pause-menu`).

## Open questions

- None blocking. Movement feel (speed/sensitivity/jump height) is tunable in the Inspector and not hardcoded.
