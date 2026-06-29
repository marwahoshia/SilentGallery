using System;
using System.Collections.Generic;
using UnityEngine;

namespace SilentGallery.ClueSystem
{
    /// <summary>
    /// Tracks which clues have been revealed during gameplay and raises events
    /// when a clue is revealed and when the win condition (enough clues revealed)
    /// is met. Place exactly one instance of this in the gameplay scene.
    /// </summary>
    public class ClueManager : MonoBehaviour
    {
        /// <summary>
        /// PLACEHOLDER: open question, confirm with the team how many clues
        /// the player must find to win. Change this single value once decided.
        /// </summary>
        private const int CLUES_REQUIRED_TO_WIN = 3;

        /// <summary>The shared instance for this scene. Set automatically in Awake.</summary>
        public static ClueManager Instance { get; private set; }

        /// <summary>Raised whenever a new (not previously revealed) clue is revealed.</summary>
        public event Action<ClueData> OnClueRevealed;

        /// <summary>Raised once, the moment the player has revealed enough clues to win.</summary>
        public event Action OnAllRequiredCluesRevealed;

        private readonly HashSet<string> revealedClueIds = new HashSet<string>();
        private bool winConditionAlreadyMet;

        /// <summary>The number of distinct clues revealed so far.</summary>
        public int RevealedClueCount => revealedClueIds.Count;

        /// <summary>The number of clues required to win (see <see cref="CLUES_REQUIRED_TO_WIN"/>).</summary>
        public int CluesRequiredToWin => CLUES_REQUIRED_TO_WIN;

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Marks a clue as revealed. Safe to call multiple times for the same clue;
        /// duplicates are ignored. Raises <see cref="OnClueRevealed"/> and, if the
        /// win threshold is newly reached, <see cref="OnAllRequiredCluesRevealed"/>.
        /// </summary>
        /// <param name="clue">The clue that was just revealed.</param>
        public void RevealClue(ClueData clue)
        {
            if (clue == null || revealedClueIds.Contains(clue.ClueId))
            {
                return;
            }

            revealedClueIds.Add(clue.ClueId);
            OnClueRevealed?.Invoke(clue);

            if (!winConditionAlreadyMet && revealedClueIds.Count >= CLUES_REQUIRED_TO_WIN)
            {
                winConditionAlreadyMet = true;
                OnAllRequiredCluesRevealed?.Invoke();
            }
        }
    }
}
