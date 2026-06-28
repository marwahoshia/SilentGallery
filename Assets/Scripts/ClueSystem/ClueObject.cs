using SilentGallery.InteractionSystem;
using UnityEngine;

namespace SilentGallery.ClueSystem
{
    /// <summary>
    /// Placed on any in-scene object (painting, drawer, note, etc.) that reveals a
    /// clue when the player interacts with it. Requires a Collider on the same
    /// GameObject so the Interaction System's raycast can detect it.
    /// </summary>
    public class ClueObject : MonoBehaviour, IInteractable
    {
        /// <summary>The clue data revealed when the player interacts with this object.</summary>
        [SerializeField]
        private ClueData clueData;

        /// <inheritdoc/>
        public void Interact(GameObject interactor)
        {
            if (clueData == null)
            {
                Debug.LogWarning($"ClueObject on '{name}' has no ClueData assigned.", this);
                return;
            }
            ClueManager.Instance.RevealClue(clueData);
        }

        /// <inheritdoc/>
        public string GetInteractionPrompt()
        {
            if (clueData == null)
            {
                return "Press E to inspect";
            }
            return $"Press E to inspect {clueData.Title}";
        }
    }
}
