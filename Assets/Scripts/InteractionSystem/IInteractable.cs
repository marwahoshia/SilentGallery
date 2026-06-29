using UnityEngine;

namespace SilentGallery.InteractionSystem
{
    /// <summary>
    /// Implemented by any object the player can interact with (clues, doors, switches, etc.).
    /// <see cref="InteractionController"/> looks for this interface on whatever it raycasts against.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Called when the player presses the interact key while looking at this object.
        /// </summary>
        /// <param name="interactor">The GameObject that performed the interaction (typically the Player).</param>
        void Interact(GameObject interactor);

        /// <summary>
        /// Short prompt text shown to the player while looking at this object,
        /// e.g. "Press E to inspect painting".
        /// </summary>
        string GetInteractionPrompt();
    }
}
