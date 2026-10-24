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

            if (UIState.IsBlocking)
            {
                if (currentInteractable != null)
                {
                    SetHighlight(currentInteractable, false);
                    OnInteractableLost?.Invoke();
                    currentInteractable = null;
                }
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
                
                hitInteractable = hit.collider.GetComponentInParent<IInteractable>();

                
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

                SetHighlight(currentInteractable, false);
                currentInteractable = hitInteractable;
                SetHighlight(currentInteractable, true);
            }
        }

        private void OnDisable()
        {
            SetHighlight(currentInteractable, false);
        }

        private static void SetHighlight(IInteractable target, bool on)
        {
            if (!(target is Component c)) return;
            InteractHighlight h = c.GetComponent<InteractHighlight>();
            if (h == null)
            {
                if (!on) return;
                h = c.gameObject.AddComponent<InteractHighlight>();
            }
            h.SetHighlighted(on);
        }
    }
}