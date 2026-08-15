using UnityEngine;

namespace SilentGallery.InteractionSystem
{
    /// <summary>
    /// Casts a ray forward from the player's camera every frame to detect nearby
    /// interactable objects and allows the player to interact using the E key.
    /// </summary>
    public class InteractionController : MonoBehaviour
    {
        [SerializeField]
        private Camera interactionCamera;

        [SerializeField]
        private float interactionRange = 3f;

        [SerializeField]
        private KeyCode interactKey = KeyCode.E;

        public event System.Action<string> OnInteractableFocused;
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

            Ray ray = new Ray(
                interactionCamera.transform.position,
                interactionCamera.transform.forward
            );

            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
            {
                // First: look on the exact object or one of its parents
                hitInteractable = hit.collider.GetComponentInParent<IInteractable>();

                // If not found, try children too
                if (hitInteractable == null)
                {
                    hitInteractable = hit.collider.GetComponentInChildren<IInteractable>();
                }

                Debug.DrawRay(
                    interactionCamera.transform.position,
                    interactionCamera.transform.forward * interactionRange,
                    Color.red
                );
            }

            if (hitInteractable != currentInteractable)
            {
                if (hitInteractable != null)
                {
                    OnInteractableFocused?.Invoke(
                        hitInteractable.GetInteractionPrompt()
                    );
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