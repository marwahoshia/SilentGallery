using TMPro;
using UnityEngine;

namespace SilentGallery.ClueSystem
{
    /// <summary>
    /// Shows a popup panel with the title and text of a clue whenever
    /// <see cref="ClueManager.OnClueRevealed"/> fires. Attach to a UI GameObject
    /// in the gameplay scene and assign all fields in the Inspector.
    /// </summary>
    public class ClueDisplayUI : MonoBehaviour
    {
        /// <summary>The root panel GameObject to show/hide. Should start inactive in the scene.</summary>
        [SerializeField]
        private GameObject cluePanel;

        /// <summary>Label that displays the clue's title.</summary>
        [SerializeField]
        private TextMeshProUGUI titleText;

        /// <summary>Label that displays the clue's full text.</summary>
        [SerializeField]
        private TextMeshProUGUI bodyText;

        private void Start()
        {
            // Subscribing in Start (not OnEnable) guarantees ClueManager.Awake has
            // already run and set Instance, since Unity calls all Awake methods
            // before any Start method.
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
        }

        /// <summary>
        /// Hides the clue popup panel. Wire this to a "Close" button's OnClick event.
        /// </summary>
        public void CloseClueDisplay()
        {
            if (cluePanel != null)
            {
                cluePanel.SetActive(false);
            }
        }
    }
}
