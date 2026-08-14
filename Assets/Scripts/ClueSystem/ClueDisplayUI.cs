using TMPro;
using UnityEngine;

namespace SilentGallery.ClueSystem
{
    /// <summary>
    /// Shows a popup panel with the title and text of a clue.
    /// </summary>
    public class ClueDisplayUI : MonoBehaviour
    {
        /// <summary>
        /// The root panel GameObject to show/hide.
        /// </summary>
        [SerializeField]
        private GameObject cluePanel;

        /// <summary>
        /// Label that displays the clue's title.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI titleText;

        /// <summary>
        /// Label that displays the clue's full text.
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI bodyText;

        private void Start()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueRevealed += ShowClue;
            }

            if (cluePanel != null)
            {
                cluePanel.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueRevealed -= ShowClue;
            }
        }

        private void ShowClue(ClueData clue)
        {
            if (titleText != null)
            {
                titleText.text = clue.Title;
            }

            if (bodyText != null)
            {
                bodyText.text = clue.ClueText;
            }

            if (cluePanel != null)
            {
                cluePanel.SetActive(true);
            }

            // Unlock and show the mouse cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// Hides the clue popup panel.
        /// Connect this function to the Close button's OnClick event.
        /// </summary>
        public void CloseClueDisplay()
{
    Debug.Log("CLOSE BUTTON CLICKED");

    if (cluePanel != null)
    {
        cluePanel.SetActive(false);
    }

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}
    }
}