using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using SilentGallery.ClueSystem;

namespace SilentGallery.Hud
{
    public class ClueHud : MonoBehaviour
    {
        [Header("UI references")]
        [SerializeField] private TMP_Text counterText;
        [SerializeField] private TMP_Text clueListText;
        [SerializeField] private TMP_Text objectiveText;

        [Header("Objective messages")]
        [SerializeField] private string objectiveFindClues =
            "Objective: Search the gallery and find the clues";
        [SerializeField] private string objectiveEnoughEvidence =
            "Objective: You have enough evidence. Draw your conclusion!";

        private readonly List<string> foundTitles = new List<string>();
        private readonly HashSet<string> foundIds = new HashSet<string>();
        private ClueManager manager;
        private bool customObjectiveSet;

        private void Start()
        {
            manager = ClueManager.Instance;
            if (manager == null)
            {
                Debug.LogWarning("ClueHud: no ClueManager found in the scene.");
                return;
            }
            manager.OnClueRevealed += HandleClueRevealed;
            Refresh();
        }

        private void OnDestroy()
        {
            if (manager != null)
            {
                manager.OnClueRevealed -= HandleClueRevealed;
            }
        }

        private void HandleClueRevealed(ClueData clue)
        {
            if (clue == null) return;
            if (foundIds.Add(clue.ClueId))
            {
                foundTitles.Add(clue.Title);
            }
            Refresh();
        }

        /// <summary>Lets other systems (e.g. the final objective) set a custom objective text.</summary>
        public void SetObjective(string text)
        {
            customObjectiveSet = true;
            if (objectiveText != null) objectiveText.text = text;
        }

        private void Refresh()
        {
            int required = manager != null ? manager.CluesRequiredToWin : 5;

            if (counterText != null)
            {
                counterText.text = $"Clues: {foundIds.Count}/{required}";
            }

            if (clueListText != null)
            {
                if (foundTitles.Count == 0)
                {
                    clueListText.text = "No clues found yet";
                }
                else
                {
                    var sb = new StringBuilder();
                    foreach (string title in foundTitles)
                    {
                        sb.Append("- ").AppendLine(title);
                    }
                    clueListText.text = sb.ToString();
                }
            }

            if (objectiveText != null && !customObjectiveSet)
            {
                objectiveText.text = foundIds.Count >= required
                    ? objectiveEnoughEvidence
                    : objectiveFindClues;
            }
        }
    }
}