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

            // Show panel
            cluePanel.SetActive(true);

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

            if (cluePanel != null)
            {
                cluePanel.SetActive(false);
            }

            closeCoroutine = null;
        }
    }
}