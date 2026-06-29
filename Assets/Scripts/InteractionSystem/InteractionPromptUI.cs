using TMPro;
using UnityEngine;

namespace SilentGallery.InteractionSystem
{
    /// <summary>
    /// Shows/hides a small on-screen prompt (e.g. "Press E to inspect painting")
    /// based on events raised by <see cref="InteractionController"/>. Attach to a
    /// UI GameObject and assign both fields in the Inspector.
    /// </summary>
    public class InteractionPromptUI : MonoBehaviour
    {
        /// <summary>The InteractionController to listen to. Assign the Player's InteractionController.</summary>
        [SerializeField]
        private InteractionController interactionController;

        /// <summary>The TextMeshPro label that displays the prompt text.</summary>
        [SerializeField]
        private TextMeshProUGUI promptText;

        private void OnEnable()
        {
            if (interactionController != null)
            {
                interactionController.OnInteractableFocused += ShowPrompt;
                interactionController.OnInteractableLost += HidePrompt;
            }
            HidePrompt();
        }

        private void OnDisable()
        {
            if (interactionController != null)
            {
                interactionController.OnInteractableFocused -= ShowPrompt;
                interactionController.OnInteractableLost -= HidePrompt;
            }
        }

        private void ShowPrompt(string prompt)
        {
            if (promptText == null)
            {
                return;
            }
            promptText.text = prompt;
            promptText.gameObject.SetActive(true);
        }

        private void HidePrompt()
        {
            if (promptText != null)
            {
                promptText.gameObject.SetActive(false);
            }
        }
    }
}
