using System.Collections.Generic;
using SilentGallery.ClueSystem;
using UnityEngine;
using UnityEngine.UI;

namespace SilentGallery.Cheats
{
    /// <summary>
    /// Developer cheat panel (F10). Compiled only in the Editor and development
    /// builds; in a release build the GameObject destroys itself.
    /// </summary>
    public class CheatManager : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [Header("UI")]
        [SerializeField] private GameObject panel;
        [SerializeField] private Button addCluesButton;
        [SerializeField] private Button closeButton;

        [Header("Settings")]
        [SerializeField] private ClueData[] allClues;
        [SerializeField] private int cluesToAdd = 5;
        [SerializeField] private KeyCode toggleKey = KeyCode.F10;

        private readonly HashSet<string> seenClueIds = new HashSet<string>();
        private bool isOpen;

        private void Start()
        {
            panel.SetActive(false);
            addCluesButton.onClick.AddListener(AddClues);
            closeButton.onClick.AddListener(Close);

            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueRevealed += HandleClueRevealed;
            }
        }

        private void OnDestroy()
        {
            if (ClueManager.Instance != null)
            {
                ClueManager.Instance.OnClueRevealed -= HandleClueRevealed;
            }
            if (isOpen)
            {
                UIState.ClosePanel();
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(toggleKey)) return;

            if (isOpen)
            {
                Close();
            }
            else if (!UIState.IsBlocking && Time.timeScale > 0f)
            {
                Open();
            }
        }

        private void Open()
        {
            isOpen = true;
            panel.SetActive(true);
            UIState.OpenPanel();
        }

        private void Close()
        {
            if (!isOpen) return;
            isOpen = false;
            panel.SetActive(false);
            UIState.ClosePanel();
        }

        private void HandleClueRevealed(ClueData clue)
        {
            seenClueIds.Add(clue.ClueId);
        }

        private void AddClues()
        {
            if (ClueManager.Instance == null) return;

            // Close first so UIState is balanced before clue popups open.
            Close();

            int added = 0;
            foreach (ClueData clue in allClues)
            {
                if (added >= cluesToAdd) break;
                if (clue == null || seenClueIds.Contains(clue.ClueId)) continue;

                ClueManager.Instance.RevealClue(clue);
                added++;
            }
        }
#else
        private void Awake()
        {
            Destroy(gameObject);
        }
#endif
    }
}