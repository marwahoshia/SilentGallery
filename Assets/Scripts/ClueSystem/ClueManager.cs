using System;
using System.Collections.Generic;
using UnityEngine;

namespace SilentGallery.ClueSystem
{
    public class ClueManager : MonoBehaviour
    {
        private const int CLUES_REQUIRED_TO_WIN = 5;

        public static ClueManager Instance { get; private set; }

        public event Action<ClueData> OnClueRevealed;
        public event Action<ClueData> OnClueRevealedSilently;
        public event Action<ClueData> OnClueRemoved;
        public event Action OnCluesReset;
        public event Action OnAllRequiredCluesRevealed;

        private readonly HashSet<string> revealedClueIds = new HashSet<string>();
        private bool winConditionAlreadyMet;

        public int RevealedClueCount => revealedClueIds.Count;
        public int CluesRequiredToWin => CLUES_REQUIRED_TO_WIN;

        private void Awake()
        {
            Instance = this;
        }

        public bool IsClueRevealed(string clueId)
        {
            return revealedClueIds.Contains(clueId);
        }

        public void RevealClue(ClueData clue)
        {
            if (clue == null)
            {
                return;
            }

            OnClueRevealed?.Invoke(clue);

            AddToProgress(clue);
        }

        public void RevealClueSilently(ClueData clue)
        {
            if (clue == null || revealedClueIds.Contains(clue.ClueId))
            {
                return;
            }

            OnClueRevealedSilently?.Invoke(clue);

            AddToProgress(clue);
        }

        public void RemoveClue(ClueData clue)
        {
            if (clue == null || !revealedClueIds.Remove(clue.ClueId))
            {
                return;
            }

            if (revealedClueIds.Count < CLUES_REQUIRED_TO_WIN)
            {
                winConditionAlreadyMet = false;
            }

            OnClueRemoved?.Invoke(clue);
        }

        public void ResetAllClues()
        {
            revealedClueIds.Clear();
            winConditionAlreadyMet = false;
            OnCluesReset?.Invoke();
        }

        private void AddToProgress(ClueData clue)
        {
            if (revealedClueIds.Contains(clue.ClueId))
            {
                return;
            }

            revealedClueIds.Add(clue.ClueId);

            if (!winConditionAlreadyMet &&
                revealedClueIds.Count >= CLUES_REQUIRED_TO_WIN)
            {
                winConditionAlreadyMet = true;
                OnAllRequiredCluesRevealed?.Invoke();
            }
        }
    }
}