using UnityEngine;

namespace SilentGallery.InteractionSystem
{
    /// <summary>
    /// Casts a ray forward from the player's camera every frame to detect nearby
    /// <see cref="IInteractable"/> objects, raises events for UI prompt display,
    /// and calls <see cref="IInteractable.Interact"/> when the player presses the
    /// interact key. Attach to the Player GameObject.
    /// </summary>
    public class InteractionController : MonoBehaviour
    {
        /// <summary>
        /// The camera the raycast is cast from. Assign the Player's child Camera
        /// (the same one used by Player Movement's look controller) in the Inspector.
        /// </summary>
        [SerializeField]
        private Camera interactionCamera;

        /// <summary>Maximum distance, in meters, at which an interactable can be detected.</summary>
        [SerializeField]
        private float interactionRange = 3f;

        /// <summary>The key that triggers <see cref="IInteractable.Interact"/> on the currently focused object.</summary>
        [SerializeField]
        private KeyCode interactKey = KeyCode.E;

        /// <summary>
        /// Raised when a new interactable object enters focus (the raycast starts hitting it).
        /// Passes the prompt text to display, e.g. "Press E to inspect painting".
        /// </summary>
        public event System.Action<string> OnInteractableFocused;

        /// <summary>
        /// Raised when the previously focused interactable object leaves focus
        /// (the raycast no longer hits it, or it's out of range).
        /// </summary>
        public event System.Action OnInteractableLost;

        private IInteractable currentInteractable;

        private void Update()
        {
            if (interactionCamera == null)
            {
                return;
            }

            UpdateFocusedInteractable();

            if (currentInteractable != null && Input.GetKeyDown(interactKey))
            {
                currentInteractable.Interact(gameObject);
            }
        }

        private void UpdateFocusedInteractable()
        {
            IInteractable hitInteractable = null;

            Ray ray = new Ray(interactionCamera.transform.position, interactionCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
            {
                hitInteractable = hit.collider.GetComponent<IInteractable>();
            }

            if (hitInteractable != currentInteractable)
            {
                if (hitInteractable != null)
                {
                    OnInteractableFocused?.Invoke(hitInteractable.GetInteractionPrompt());
                }
                else
                {
                    OnInteractableLost?.Invoke();
                }
                currentInteractable = hitInteractable;
            }
        }
    }
}
