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

        private readonly List<ClueData> found = new List<ClueData>();
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
            manager.OnClueRevealed += HandleClueAdded;
            manager.OnClueRevealedSilently += HandleClueAdded;
            manager.OnClueRemoved += HandleClueRemoved;
            manager.OnCluesReset += HandleCluesReset;
            Refresh();
        }

        private void OnDestroy()
        {
            if (manager != null)
            {
                manager.OnClueRevealed -= HandleClueAdded;
                manager.OnClueRevealedSilently -= HandleClueAdded;
                manager.OnClueRemoved -= HandleClueRemoved;
                manager.OnCluesReset -= HandleCluesReset;
            }
        }

        private void HandleClueAdded(ClueData clue)
        {
            if (clue == null) return;
            if (!found.Exists(c => c.ClueId == clue.ClueId))
            {
                found.Add(clue);
            }
            Refresh();
        }

        private void HandleClueRemoved(ClueData clue)
        {
            if (clue == null) return;
            found.RemoveAll(c => c.ClueId == clue.ClueId);
            customObjectiveSet = false;
            Refresh();
        }

        private void HandleCluesReset()
        {
            found.Clear();
            customObjectiveSet = false;
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
                counterText.text = $"Clues: {found.Count}/{required}";
            }

            if (clueListText != null)
            {
                if (found.Count == 0)
                {
                    clueListText.text = "No clues found yet";
                }
                else
                {
                    var sb = new StringBuilder();
                    foreach (ClueData clue in found)
                    {
                        sb.Append("- ").AppendLine(clue.Title);
                    }
                    clueListText.text = sb.ToString();
                }
            }

            if (objectiveText != null && !customObjectiveSet)
            {
                objectiveText.text = found.Count >= required
                    ? objectiveEnoughEvidence
                    : objectiveFindClues;
            }
        }
    }
}