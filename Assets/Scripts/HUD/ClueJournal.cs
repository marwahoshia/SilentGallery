using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SilentGallery.ClueSystem;
using SilentGallery.WinLoseSystem;

namespace SilentGallery.Hud
{
    public class ClueJournal : MonoBehaviour
    {
        [Header("UI references")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text journalText;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Button closeButton;

        [Header("Settings")]
        [SerializeField] private KeyCode toggleKey = KeyCode.J;
        [SerializeField] private GameOutcomeManager gameOutcomeManager;

        private readonly List<ClueData> clues = new List<ClueData>();
        private readonly HashSet<string> clueIds = new HashSet<string>();
        private ClueManager clueManager;
        private bool isOpen;

        private void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }
        }

        private void Start()
        {
            clueManager = ClueManager.Instance;
            if (clueManager != null)
            {
                clueManager.OnClueRevealed += HandleClueRevealed;
                clueManager.OnClueRevealedSilently += HandleClueRevealed;
                clueManager.OnClueRemoved += HandleClueRemoved;
                clueManager.OnCluesReset += HandleCluesReset;
            }
            else
            {
                Debug.LogWarning("ClueJournal: no ClueManager found in the scene.");
            }

            if (gameOutcomeManager == null)
            {
                gameOutcomeManager = FindObjectOfType<GameOutcomeManager>();
            }
            if (gameOutcomeManager != null)
            {
                gameOutcomeManager.OnGameLost += Close;
            }

            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (clueManager != null)
            {
                clueManager.OnClueRevealed -= HandleClueRevealed;
                clueManager.OnClueRevealedSilently -= HandleClueRevealed;
                clueManager.OnClueRemoved -= HandleClueRemoved;
                clueManager.OnCluesReset -= HandleCluesReset;
            }
            if (gameOutcomeManager != null)
            {
                gameOutcomeManager.OnGameLost -= Close;
            }
            // Intentionally no UIState.ClosePanel() here: UIState resets itself
            // on scene load, and calling it would re-lock the cursor in menus.
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

        private void HandleClueRevealed(ClueData clue)
        {
            if (clue == null) return;
            if (clueIds.Add(clue.ClueId))
            {
                clues.Add(clue);
                if (isOpen) Rebuild();
            }
        }

        private void HandleClueRemoved(ClueData clue)
        {
            if (clue == null) return;
            if (clueIds.Remove(clue.ClueId))
            {
                clues.RemoveAll(c => c.ClueId == clue.ClueId);
                if (isOpen) Rebuild();
            }
        }

        private void HandleCluesReset()
        {
            clues.Clear();
            clueIds.Clear();
            if (isOpen) Rebuild();
        }

        private void Open()
        {
            if (isOpen) return;
            isOpen = true;
            Rebuild();
            if (panel != null) panel.SetActive(true);
            UIState.OpenPanel();

            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 1f;
            }
        }

        private void Close()
        {
            if (!isOpen) return;
            isOpen = false;
            if (panel != null) panel.SetActive(false);
            UIState.ClosePanel();
        }

        private void Rebuild()
        {
            if (journalText == null) return;

            if (clues.Count == 0)
            {
                journalText.text = "<i>No evidence collected yet.</i>";
                return;
            }

            var sb = new StringBuilder();
            for (int i = 0; i < clues.Count; i++)
            {
                ClueData c = clues[i];
                sb.Append("<b><color=#FFD27F>")
                  .Append(i + 1).Append(". ").Append(c.Title)
                  .AppendLine("</color></b>");
                sb.AppendLine(c.ClueText);
                sb.AppendLine();
            }
            journalText.text = sb.ToString();
        }
    }
}