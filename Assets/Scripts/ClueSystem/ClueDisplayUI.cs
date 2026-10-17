using System.Collections;
using TMPro;
using UnityEngine;

namespace SilentGallery.ClueSystem
{
    public class ClueDisplayUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject cluePanel;

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI bodyText;

        [SerializeField]
        private float displayDuration = 5f;

        private Coroutine closeCoroutine;

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
            if (clue == null || cluePanel == null)
            {
                return;
            }

            // Update clue text
            if (titleText != null)
            {
                titleText.text = clue.Title;
            }

            if (bodyText != null)
            {
                bodyText.text = clue.ClueText;
            }

            // Show panel. Only register with UIState on the closed -> open
            // transition, since a new clue can arrive while one is already
            // showing (the timer just restarts) - calling OpenPanel again
            // here would desync the open-panel count.
            bool wasAlreadyOpen = cluePanel.activeSelf;
            cluePanel.SetActive(true);
            if (!wasAlreadyOpen)
            {
                UIState.OpenPanel();
            }

            // If an old timer is running, restart it
            if (closeCoroutine != null)
            {
                StopCoroutine(closeCoroutine);
            }

            closeCoroutine = StartCoroutine(AutoCloseClue());
        }

        private IEnumerator AutoCloseClue()
        {
            yield return new WaitForSeconds(displayDuration);
            closeCoroutine = null;
            CloseClueDisplay();
        }

        /// <summary>
        /// Hides the clue popup and releases its hold on the cursor. Safe to
        /// call whether closing happens automatically or via the Close button;
        /// calling it twice for the same open popup has no extra effect.
        /// </summary>
        public void CloseClueDisplay()
        {
            if (closeCoroutine != null)
            {
                StopCoroutine(closeCoroutine);
                closeCoroutine = null;
            }

            if (cluePanel != null && cluePanel.activeSelf)
            {
                cluePanel.SetActive(false);
                UIState.ClosePanel();
            }
        }
    }
}